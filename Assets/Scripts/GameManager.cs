using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState
{
    placingCueBall,
    playerControl,
    ballRolling
};


[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{
    
    public static GameManager GM;
    public GameState gameState;
    public int numBallsRolling = 0;
    public int playerTurn = 0;
    public int acceptableBall = 1;
    public int faulScore = 4;
    public bool firstHit = false;
    public bool successHit, successPool, faulPool, cuePool;
    public int numBalls = 0;
    const int totalColorBalls = 6;
    public bool clearColorPhase = false;
    public int[] playerScore;
    [SerializeField] GameObject cueBall;
    [SerializeField] TextMeshProUGUI infoText, scoreText;
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
        gameState = GameState.placingCueBall;
        numBallsRolling = 0;
        playerTurn = 0;
        acceptableBall = 1;
        faulScore = 4;
        firstHit = false;
        successHit = successPool = faulPool = cuePool = false;
        clearColorPhase = false;
        playerScore[0] = playerScore[1] = 0;
        scoreText.text = $"Player1: {playerScore[0]}\nPlayer2: {playerScore[1]}";
    }
    void handleGameState()
    {
        switch (gameState)
        {
            case GameState.ballRolling:
                if (numBallsRolling == 0)
                {
                    // All rolling balls come to a stop, determine next shot according to flags
                    handleShotResult();
                    cueBall.GetComponent<CueBall>().checkPooled();
                }
                break;

            case GameState.playerControl:
                infoText.text = $"Player{playerTurn + 1}'s turn";
                break;

            case GameState.placingCueBall:
                infoText.text = $"Player{playerTurn + 1} place the cue ball";
                break;
        }
    }

    void handleShotResult()
    {
        if (!firstHit || !successHit || faulPool)
        {
            // Faul
            addScoreToOpponent(faulScore);
        }
        if (successHit && successPool && !faulPool)
        {
            // Succeeded a shot, accecptableBall can't be 0
            addScore(acceptableBall);
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

    public void setFaulScore(int score)
    {
        if (score > faulScore)
        {
            faulScore = score;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
        initialize();
    }
}
