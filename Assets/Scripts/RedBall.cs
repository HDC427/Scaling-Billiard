using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBall : BallBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void afterPool()
    {
        GameManager.GM.numBalls -= 1;
    }
}
