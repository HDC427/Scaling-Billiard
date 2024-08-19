using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBall : BallBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.GM.numRedBalls += 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void afterPool()
    {
        GameManager.GM.numRedBalls -= 1;
    }
}
