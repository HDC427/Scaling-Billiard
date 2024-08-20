using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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
        other.gameObject.GetComponent<BallBehaviour>().handlePool();
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
