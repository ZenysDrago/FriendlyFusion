using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{

    [SerializeField] private float timeDestruction = 0.5f;

    private float timer = 0;

    // Update is called once per frame
    void Update()
    {
        if(timer >= timeDestruction)
            Destroy(gameObject);

        timer += Time.deltaTime;
    }
}
