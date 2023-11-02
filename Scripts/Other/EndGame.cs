using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    private GameController gc;

    private void Start()
    {
        gc = FindObjectOfType<GameController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 /* player layer*/)
        {
            Debug.Log("Player entered");
            SceneManager.LoadScene((int)gc.entityControlled);
        }
    }
}
