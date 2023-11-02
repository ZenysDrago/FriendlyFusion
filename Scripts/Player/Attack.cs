using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    private bool hasTouched = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        Breakable breakable = other.GetComponentInParent<Breakable>();
        if (breakable)
        {
            Debug.Log("Attack Success");
            breakable.HitBoxCollide(gameObject);
            hasTouched = true;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(!hasTouched)
            Debug.Log("Attack Missed");
    }
}
