using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool isActive;
    private Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.playerTurn > 0)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                cam.enabled = !cam.enabled;
            }
        }
    }
}
