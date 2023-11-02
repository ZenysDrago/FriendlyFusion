using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBox : MonoBehaviour, IPlayerListener
{
    //[SerializeField] private Rigidbody rbody;
    [SerializeField] private PlayerEvent events;
    [SerializeField] private HeavyPlayer playerHeavy;
    [SerializeField] private bool canBePushed;
    [SerializeField] private bool inZone;
    private GameController gc;
    [HideInInspector] public float pushStrength ;

    // Start is called before the first frame update
    void Start()
    {
        events.Subscribe(this);
        gc = FindObjectOfType<GameController>();
        if(!gc.HeavyBehaviour.TryGetComponent(out playerHeavy))
            Debug.LogError("Heavy not found");
    }

    public void EnterZone(bool entered)
    {
        inZone = entered;
    }

    #region InterfaceFunction

    public void NotifyMovement(Vector3 current)
    {
        if (inZone && playerHeavy && playerHeavy.InPushMode && gc.entityControlled == EntityControlled.HEAVY/* && canBePushed*/)
        {
            if (playerHeavy)
            {
                playerHeavy.boxPushed = this;
                playerHeavy.Push = true;
            }

            //rbody.isKinematic = false;

            if (current != Vector3.zero)
            {
                /*TODO PLAY SOUND HERE*/
            }
             
            // // /* TODO FIX THIS push & pull not same speed is not normal but push won't work otherwise */
            // if(HelperFunction.VectorInSameDir(playerHeavy.transform.forward, current, 90))
            //     rbody.AddForce(current * (pushStrength * 5f), ForceMode.Force);
            // else
            //     rbody.AddForce(current * (pushStrength * 9.75f), ForceMode.Force);
            // // rbody.velocity = current;
        }
        else {
            //rbody.isKinematic = true;
            transform.parent = null;
            if (playerHeavy)
            {
                playerHeavy.boxPushed = null;
                playerHeavy.Push = false;
            }
        }
    }

    public void NotifyGround(bool onGround)
    {
        canBePushed = onGround;
    }

    public void NotifySwitch(EntityControlled controlled)
    {
        
    }
    #endregion
    
}
