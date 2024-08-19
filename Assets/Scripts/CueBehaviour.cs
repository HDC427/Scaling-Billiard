using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject cueBall, cueCamera;
    private Vector3 cueBallCenter;
    private float facingAngle = 0, slopingAngle = 0;
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
        facingAngle += hRotation;

        float vRotation = Input.GetAxis("Vertical") * Time.deltaTime * rotationSpeed;
        float facingRadian = facingAngle / 180 * Mathf.PI;
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
}
