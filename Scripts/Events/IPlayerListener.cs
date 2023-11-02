using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPlayerListener
{
    public void NotifyMovement(Vector3 current);
    public void NotifyGround(bool isGrounded);

    public void NotifySwitch(EntityControlled controlled);
}
