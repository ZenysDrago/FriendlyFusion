using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private List<PressurePlate> plates = new List<PressurePlate>();
    [SerializeField] private string triggerName;
    [SerializeField] private Animator anim;
    [SerializeField] private bool lockPlates = true;
    private List<Tuple<PressurePlate, bool>> plateActive = new List<Tuple<PressurePlate, bool>>();

    [SerializeField] private GameObject wall;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(PressurePlate pressurePlate in plates)
        {
            plateActive.Add(new Tuple<PressurePlate, bool>(pressurePlate, false));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenDoor()
    {
        anim.SetTrigger(triggerName);
        wall.SetActive(false);
        
        if (lockPlates)
        {
            foreach (Tuple<PressurePlate,bool> tuple in plateActive)
            {
                tuple.Item1.LockActive();
            }
        }
    }

    private void CloseDoor()
    {
        anim.SetTrigger(triggerName);
        wall.SetActive(true);
    }
    
    private void CheckList()
    {
        foreach (Tuple<PressurePlate,bool> tuple in plateActive)
        {
            if (!tuple.Item2)
            {
                CloseDoor();
                return;
            }
        }
        
        OpenDoor();
    }
    
    public void PlateEvent(bool active, PressurePlate plate)
    {
        var tupleFind = plateActive.Find(tuple => tuple.Item1 == plate);
        if (tupleFind != null)
        {
            plateActive.Remove(tupleFind);
            plateActive.Add(Tuple.Create(tupleFind.Item1, active));
        }

        CheckList();
    }
}
