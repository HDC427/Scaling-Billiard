using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject cueBall, cueCamera;
    private Vector3 cueBallCenter;
    private Vector3 facingDirection = Vector3.forward;
    private float slopingAngle = 0;
    [SerializeField] private float rotationSpeed = 30;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.playerTurn > 0)
        {
            cueBallCenter = cueBall.transform.position;
            handleRotation();
        }
    }

    void handleRotation()
    {
        float hRotation = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;
        transform.RotateAround(cueBallCenter, Vector3.up, -hRotation);
        cueCamera.transform.RotateAround(cueBallCenter, Vector3.up, -hRotation);
        float hRadian = hRotation / 180 * Mathf.PI;
        facingDirection = new Vector3(facingDirection.x * Mathf.Cos(hRadian) - facingDirection.z * Mathf.Sin(hRadian), 0, facingDirection.x * Mathf.Sin(hRadian) + facingDirection.z * Mathf.Cos(hRadian));
        
        float vRotation = Input.GetAxis("Vertical") * Time.deltaTime * rotationSpeed;
        Vector3 rotateAxis = new Vector3(facingDirection.z, 0, -facingDirection.x);
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
}
