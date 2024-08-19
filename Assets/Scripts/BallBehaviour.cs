using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public int score;
    public bool isRolling = false;
    private Vector3 positionLastCheck;
    public float checkInterval = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        positionLastCheck = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isRolling && !collision.gameObject.CompareTag("Table"))
        {
            // A still ball starts moving
            isRolling = true;
            GameManager.GM.numBallsRolling += 1;
            GameManager.GM.gameState = GameState.ballRolling;
            // Check every checkInterval seconds the position of the ball,
            // if the position is the same as the last check, regard the ball as still
            StartCoroutine(checkMotion());
        }
    }

    IEnumerator checkMotion()
    {
        while (isRolling)
        {
            yield return new WaitForSeconds(checkInterval);
            if (transform.position == positionLastCheck)
            {
                isRolling = false;
                GameManager.GM.numBallsRolling -= 1;
            }
            positionLastCheck = transform.position;
        }
        
    }
}
