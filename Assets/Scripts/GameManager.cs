using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    public int numBalls = 0;
    const int totalColorBalls = 6;
    public bool clearColorPhase = false;
    public int[] playerScore;
    [SerializeField] GameObject cue;
    [SerializeField] TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        GM = this;
        initialize();
    }

    // Update is called once per frame
    void Update()
    {
        handleGameState();
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    void initialize()
    {
        gameState = GameState.playerControl;
        numBallsRolling = 0;
        playerTurn = 0;
        acceptableBall = 1;
        firstHit = false;
        successHit = successPool = faulPool = false;
        clearColorPhase = false;
        playerScore[0] = playerScore[1] = 0;
        scoreText.text = $"Player1: {playerScore[0]}\nPlayer2: {playerScore[1]}";
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
            // Succeeded a shot
            if (acceptableBall == 1)
            {
                // Successfully pooled a red ball, next shot can be any colored ball
                acceptableBall = 0;
            } // Successfully pooled a colored ball
            else if (clearColorPhase)
            {
                // In clear color phase, next shot is the next colored ball
                acceptableBall += 1;
            }
            else if (numBalls == totalColorBalls)
            {
                // All red balls are pooled, enter clear color phase
                acceptableBall = 2;
                clearColorPhase = true;
            }
            else
            {
                // There are still red balls remaining, next shot is a red ball
                acceptableBall = 1;
            }
        }
        else
        {
            // Failed a shot, switch player
            playerTurn = (playerTurn + 1) % 2;
            if (!clearColorPhase)
            {
                if (numBalls == totalColorBalls)
                {
                    // All red balls are pooled, enter clear color phase
                    acceptableBall = 2;
                    clearColorPhase = true;
                }
                else
                {
                    // There are still red balls remaining, next shot is a red ball
                    acceptableBall = 1;
                }
            }
        }
        // After results are calculated, reset flags
        firstHit = successHit = successPool = faulPool = false;
    }

    public void addScore(int score)
    {
        playerScore[playerTurn] += score;
        scoreText.text = $"Player1: {playerScore[0]}\nPlayer2: {playerScore[1]}";
    }

    public void addScoreToOpponent(int score)
    {
        playerScore[(playerTurn + 1) % 2] += score > 4 ? score : 4;
        scoreText.text = $"Player1: {playerScore[0]}\nPlayer2: {playerScore[1]}";
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
        initialize();
    }
}
