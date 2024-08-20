using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public int score;
    public bool isRolling = false;
    private Vector3 positionLastCheck;
    public float checkInterval = 0.1f;

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.GM.gameState == GameState.ballRolling && !isRolling)
        {
            // A still ball starts moving
            isRolling = true;
            GameManager.GM.numBallsRolling += 1;

            // check first hit
            if (!GameManager.GM.firstHit)
            {
                if (collision.gameObject.CompareTag("CueBall"))
                {
                    GameManager.GM.firstHit = true;
                    if (GameManager.GM.acceptableBall > 0)
                    {
                        if (score == GameManager.GM.acceptableBall)
                        {
                            GameManager.GM.successHit = true;
                        }
                        else
                        {
                            GameManager.GM.setFaulScore(score);
                        }
                    }
                    else // acceptableBall == 0, first hit can be any colored ball
                    {
                        if (score > 1)
                        {
                            GameManager.GM.successHit = true;
                            GameManager.GM.acceptableBall = score;
                        }
                        else
                        {
                            GameManager.GM.setFaulScore(score);
                        }
                    }
                    
                }
            }

            // Check every checkInterval seconds the position of the ball,
            // if the position is the same as the last check, regard the ball as still
            StartCoroutine(checkMotion());
        }
    }

    protected IEnumerator checkMotion()
    {
        while (isRolling)
        {
            yield return new WaitForSeconds(checkInterval);
            if (Vector3.Distance(transform.position, positionLastCheck) < 1e-4)
            {
                isRolling = false;
                GameManager.GM.numBallsRolling -= 1;
            }
            positionLastCheck = transform.position;
        }
        
    }

    public void handlePool()
    {
        if (score == GameManager.GM.acceptableBall)
        {
            GameManager.GM.successPool = true;
        }
        else
        {
            GameManager.GM.faulPool = true;
            GameManager.GM.setFaulScore(score);
        }
        GameManager.GM.numBallsRolling -= 1;
        isRolling = false;
        afterPool();
    }

    protected virtual void afterPool() { }
}
