using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LitePlayer : SpecificPlayerBehaviour 
{
    [SerializeField] private bool hasDoubleJump;
    [SerializeField] private float doubleJumpForce;

    
    // Start is called before the first frame update
    void Start()
    {
        playerBase = GetComponent<PlayerBase>();
        gc = FindObjectOfType<GameController>();
        gc.playerEvent.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #region InputEvents

    public override void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        
        Debug.Log("JUMP");

        if (playerBase.CanJump)
        {
            playerBase.Rigidbody.velocity = new Vector3(playerBase.Rigidbody.velocity.x, jumpForce, playerBase.Rigidbody.velocity.z);
            hasDoubleJump = true;
        }
        else if(hasDoubleJump)
        {
            playerBase.Rigidbody.velocity = new Vector3(playerBase.Rigidbody.velocity.x, doubleJumpForce, playerBase.Rigidbody.velocity.z);
            hasDoubleJump = false;
        }
    }

    public override void Action(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        /* Nothing to do with this input */
        Debug.Log("ACTION");

    }
    #endregion

    #region InterfaceFunction

    public override void NotifyMovement(Vector3 current)
    {
        /*not needed*/
    }

    public override void NotifyGround(bool onGround)
    {
        if (onGround)
            hasDoubleJump = true;
    }

    public override void NotifySwitch(EntityControlled controlled)
    {
        
    }
    #endregion
}
