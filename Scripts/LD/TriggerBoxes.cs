using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoxes : MonoBehaviour
{
    [SerializeField] private MoveableBox box;
    private void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;
        if (col.layer == 3 /*player layer*/)
        {
            box.EnterZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject col = other.gameObject;
        if (col.layer == 3 /*player layer*/)
        {
            box.EnterZone(false);
        }
    }
}
