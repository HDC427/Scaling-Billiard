using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CueBall : BallBehaviour
{
    private bool needsReset;
    private Vector3 startingPosition;
    private Vector3 startingCenter;
    private float startingRadius;
    [SerializeField] private GameObject browBall;
    [SerializeField] Camera topCamera, cueCamera;
    [SerializeField] private GameObject cue;
    // Start is called before the first frame update
    void Start()
    {
        // Cue ball does not add to numBalls
        needsReset = false;
        startingPosition = transform.position;
        startingCenter = browBall.transform.position;
        startingRadius = Vector3.Distance(startingPosition, startingCenter);
    }

    void Update()
    {
        if (GameManager.GM.gameState == GameState.placingCueBall)
        {
            gameObject.GetComponent<Collider>().isTrigger = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            Camera cam = topCamera.GetComponent<Camera>().enabled ? topCamera : cueCamera;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.point.z < startingCenter.z && Vector3.Distance(hit.point, startingCenter) <= startingRadius)
                {
                    transform.position = new(hit.point.x, transform.position.y, hit.point.z);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                needsReset = false;
                GameManager.GM.gameState = GameState.playerControl;
                cue.GetComponent<CueBehaviour>().resetPosition();
                gameObject.GetComponent<Collider>().isTrigger = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cue"))
        {
            isRolling = true;
            GameManager.GM.numBallsRolling += 1;
            GameManager.GM.gameState = GameState.ballRolling;
            StartCoroutine(checkMotion());
        }
    }

    protected override void afterPool()
    {
        transform.position = new Vector3(0, 0, -10);
        needsReset = true;
    }

    public void checkPooled()
    {
        if (needsReset)
        {
            GameManager.GM.gameState = GameState.placingCueBall;

            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = startingPosition;
            GetComponent<Rigidbody>().isKinematic = true;

            cue.SetActive(true);
            cue.GetComponent<CueBehaviour>().resetPosition(true);
        }
        else
        {
            GameManager.GM.gameState = GameState.playerControl;

            cue.SetActive(true);
            cue.GetComponent<CueBehaviour>().resetPosition();
        }
    }
}
