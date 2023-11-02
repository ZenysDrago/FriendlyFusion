using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityControlled
{
    LITE,
    HEAVY,
    [InspectorName(null)]
    FUSED,
}

public class GameController : MonoBehaviour
{
    public PlayerEvent playerEvent;
    public Transform cam;
    public GameObject LiteBehaviour;
    public GameObject HeavyBehaviour;
    public GameObject Fused;
    public Animator transition;
    public bool canSwitch = true;    
    public EntityControlled entityControlled;
    public void SwitchPlayer()
    {
        playerEvent.NotifyAllSwitch(entityControlled);
    }

    public void EnableSwitch(bool enable)
    {
        canSwitch = enable;
    }
}
