using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBall : BallBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Cue ball does not add to numBalls
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
}
