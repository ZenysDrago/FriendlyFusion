using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] private bool isActive;
    [SerializeField] private bool locked;
    [SerializeField] private string nameTrigger;
    [SerializeField] private Animator anim;
    
    
    [SerializeField] private MeshRenderer TMPMesh;
    [SerializeField] private Material matPressed;
    [SerializeField] private Material matLeave;
    
    private bool playerOnTop;
    
    public void LockActive()
    {
        locked = !locked;
        if (!locked)
        {
            if (!playerOnTop)
            {
                anim.SetTrigger(nameTrigger);
                door.PlateEvent(playerOnTop, this);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (locked)
            return;
        
        GameObject col = other.gameObject;
        if (col.layer == 3 /*player layer*/)
        {
            playerOnTop = true;
            anim.SetTrigger(nameTrigger);
            TMPMesh.material = matPressed;
            door.PlateEvent(true, this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (locked)
            return;
        
        GameObject col = other.gameObject;
        if (col.layer == 3 /*player layer*/)
        {
            playerOnTop = false;
            anim.SetTrigger(nameTrigger);
            TMPMesh.material = matLeave;

            door.PlateEvent(false, this);
        }
    }
}
