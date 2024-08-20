using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CueBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject cueBall, cueCamera;

    private Vector3 cueBallCenter;
    private Vector3 offSetFromCueBall = new(0, 0, -1.85f);
    private Vector3 offSetCueBallToCueCamera = new(0, 0.2f, -0.35f);
    private Quaternion initialCameraRotation;
    [SerializeField] private float facingAngle = 0, slopingAngle = 0;
    private Vector3 facingDirection = Vector3.forward;
    [SerializeField] private float rotationSpeed = 30;

    [SerializeField] GameObject forceIndicator;
    [SerializeField] private float forceChangeRate, minForce, maxForce, shootForce;
    // The cue moves forward or backward proportional to the force to be applied
    [SerializeField] private float forceToDisplacement;
    // The cue lingers for a short time after shot before disappearing
    [SerializeField] private float disappearAfterShot;
    void Start()
    {
        initialCameraRotation = cueCamera.transform.rotation;
        resetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GM.gameState == GameState.playerControl)
        {
            handleRotation();
            handleShoot();
        }
    }

    public void resetPosition()
    {
        GetComponent<Rigidbody>().isKinematic = true;

        cueBallCenter = cueBall.transform.position;
        transform.position = cueBallCenter + offSetFromCueBall;
        transform.position -= new Vector3(0, 0, (cueBall.transform.localScale.x - 1) * 0.035f);
        transform.rotation = Quaternion.identity;
        slopingAngle = 0;
        transform.RotateAround(cueBallCenter, Vector3.up, -facingAngle);

        cueCamera.transform.position = cueBallCenter + offSetCueBallToCueCamera;
        cueCamera.transform.rotation = initialCameraRotation;
        cueCamera.transform.RotateAround(cueBallCenter, Vector3.up, -facingAngle);

        GetComponent<Rigidbody>().isKinematic = false;
    }

    void handleRotation()
    {
        float hRotation = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;
        transform.RotateAround(cueBallCenter, Vector3.up, -hRotation);
        cueCamera.transform.RotateAround(cueBallCenter, Vector3.up, -hRotation);
        facingAngle += hRotation;
        float facingRadian = facingAngle / 180 * Mathf.PI;
        facingDirection = new Vector3(-Mathf.Sin(facingRadian), 0, Mathf.Cos(facingRadian));

        float vRotation = Input.GetAxis("Vertical") * Time.deltaTime * rotationSpeed;
        Vector3 rotateAxis = new Vector3(Mathf.Cos(facingRadian), 0, Mathf.Sin(facingRadian));
        if (vRotation < 0)
        {
            if (slopingAngle >= 0)
            {
                slopingAngle += vRotation;
                transform.RotateAround(cueBallCenter, rotateAxis, vRotation);
            }

        }
        else
        {
            if (slopingAngle <= 90)
            {
                slopingAngle += vRotation;
                transform.RotateAround(cueBallCenter, rotateAxis, vRotation);
            }
        }
    }

    void handleShoot()
    {
        Debug.DrawRay(cueBallCenter, facingDirection);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shootForce = minForce;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            float forceChange = forceChangeRate * Time.deltaTime;
            shootForce += forceChange;
            updateForceIndicator();
            transform.Translate(forceToDisplacement * forceChange * Vector3.back);
            if (shootForce < minForce || shootForce > maxForce)
            {
                forceChangeRate = -forceChangeRate;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddRelativeForce(shootForce * Vector3.forward, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CueBall"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(facingDirection, ForceMode.VelocityChange);
            StartCoroutine(hideCueAfterShot());
        }
    }

    IEnumerator hideCueAfterShot()
    {
        yield return new WaitForSeconds(disappearAfterShot);
        gameObject.SetActive(false);
    }

    void updateForceIndicator()
    {
        forceIndicator.GetComponent<Slider>().value = shootForce;
    }
}
