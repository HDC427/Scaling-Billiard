using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    playerControl,
    ballRolling,
};

[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{
    
    public static GameManager GM;
    public GameState gameState = GameState.playerControl;
    public int numBallsRolling = 0;
    public int playerTurn = 0;
    public int acceptableBall = 1;
    public bool firstHit = false;
    public bool successHit, successPool, faulPool;
    public int numRedBalls = 0;
    public int[] playerScore = { 0, 0 };
    [SerializeField] GameObject cue;
    // Start is called before the first frame update
    void Start()
    {
        GM = this;
    }

    // Update is called once per frame
    void Update()
    {
        handleGameState();
    }

    void handleGameState()
    {
        if (gameState == GameState.ballRolling)
        {
            if(numBallsRolling == 0)
            {
                // When all rolling balls come to a stop, a new shot start
                gameState = GameState.playerControl;
                handleShotResult();

                cue.SetActive(true);
                cue.GetComponent<Collider>().isTrigger = true;
                cue.GetComponent<CueBehaviour>().resetPosition();
            }
        }
    }

    void handleShotResult()
    {
        if (!firstHit)
        {
            addScoreToOpponent(4);
        }
        if (successHit && successPool && !faulPool)
        {
            // When successHit = true, acceptableBall can't be 0
            if (acceptableBall == 1)
            {
                acceptableBall = 0;
            }
            else if (numRedBalls > 0)
            {
                acceptableBall = 1;
            }
            else
            {
                acceptableBall += 1;
            }
        }
        else
        {
            playerTurn = (playerTurn + 1) % 2;
            if (numRedBalls > 0)
            {
                acceptableBall = 1;
            }else if (acceptableBall == 0)
            {
                // The last red ball is pooled, the next colored ball not,
                // then start from yellow ball
                acceptableBall = 2;
            }
        }
    }

    public void addScore(int score)
    {
        playerScore[playerTurn] += score;
    }

    public void addScoreToOpponent(int score)
    {
        playerScore[(playerTurn + 1) % 2] += score;
    }
}
