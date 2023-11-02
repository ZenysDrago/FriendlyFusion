using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private GameController gc;
    [SerializeField] private StudioEventEmitter fmodEmitter;
    [SerializeField] private GameObject particles;
    private void Start()
    {
        gc = FindObjectOfType<GameController>();
    }

    public void HitBoxCollide(GameObject hitbox)
    {
        /* Sounds */
        // fmodEmitter.SendMessage("Play");
        // emitterGameEvent.TriggerParameters();

        Vector3 dir = hitbox.transform.position - transform.position;
        Quaternion newRot = Quaternion.Euler(dir);
        Debug.Log(Instantiate(particles, transform.position,newRot));
        
        Destroy(gameObject);
    }
}
