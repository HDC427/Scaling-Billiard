using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBall : BallBehaviour
{
    private Vector3 initialPosition;
    private bool needsReset;
    void Start()
    {
        GameManager.GM.numBalls += 1;
        initialPosition = transform.position;
        needsReset = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GM.gameState != GameState.ballRolling && needsReset)
        {
            resetPosition();
        }
    }
    protected override void afterPool()
    {
        if (GameManager.GM.clearColorPhase)
        {
            GameManager.GM.numBalls -= 1;
            gameObject.SetActive(false);
        }
        else
        {
            transform.position = new Vector3(0, 0, -10);
            needsReset = true;
        }
    }

    private void resetPosition()
    {
        GetComponent<Rigidbody>().isKinematic = true;

        transform.position = initialPosition;
        needsReset = false;

        GetComponent<Rigidbody>().isKinematic = false;
    }
}
