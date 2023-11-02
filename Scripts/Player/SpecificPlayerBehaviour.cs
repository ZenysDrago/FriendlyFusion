using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class SpecificPlayerBehaviour : MonoBehaviour , IPlayerListener
{
    [SerializeField] protected float jumpForce;
    [SerializeField] protected GameController gc;
    protected PlayerBase playerBase;
    protected Rigidbody rbody;
    public abstract void Jump(InputAction.CallbackContext context);

    public abstract void Action(InputAction.CallbackContext context);

    public abstract void NotifyMovement(Vector3 current);
    public abstract void NotifyGround(bool onGround);

    public abstract void NotifySwitch(EntityControlled controlled);
}
