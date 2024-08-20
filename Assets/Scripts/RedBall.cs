using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBall : BallBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.GM.numBalls += 1;
    }
    protected override void afterPool()
    {
        GameManager.GM.numBalls -= 1;
        gameObject.SetActive(false);
    }
}
