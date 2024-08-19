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
                gameState = GameState.playerControl;
                playerTurn = (playerTurn + 1) % 2;
                cue.SetActive(true);
                cue.GetComponent<Collider>().isTrigger = true;
                cue.GetComponent<CueBehaviour>().resetPosition();
            }
        }
    }
}
