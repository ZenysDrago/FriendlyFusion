using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEvents : MonoBehaviour
{
    private GameController gc;

    private void Start()
    {
        gc = FindObjectOfType<GameController>();
    }

    public void CallSwitch()
    {
        gc.SwitchPlayer();
    }

    public void EndTransition()
    {
        gc.EnableSwitch(true);
    }
}
