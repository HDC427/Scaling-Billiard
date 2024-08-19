using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject cueBall;
    [SerializeField] bool scaleUp;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        int score = other.gameObject.GetComponent<BallBehaviour>().score;
        other.gameObject.SetActive(false);
        GameManager.GM.numBallsRolling -= 1;
        GameManager.GM.addScore(score);
        if (scaleUp)
        {
            cueBall.transform.localScale *= (1 + score / 10.0f);
        }
        else
        {
            cueBall.transform.localScale /= (1 + score / 10.0f);
        }
    }
}
