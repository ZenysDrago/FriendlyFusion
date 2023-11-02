using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events" , fileName = "PlayerEvents")]
public class PlayerEvent : ScriptableObject
{
    private List<IPlayerListener> listeners = new List<IPlayerListener>();

    public void Reset()
    {
        listeners.Clear();    
    }
    
    public void Subscribe(IPlayerListener newListener)
    {
        if (listeners.Find(listener => listener == newListener) == null)
            listeners.Add(newListener);
    }

    public void UnSubscribe(IPlayerListener listener)
    {
        listeners.Remove(listener);
    }

    public void NotifyAllGround(bool isGrounded)
    {
        listeners.ForEach(playerListener => playerListener.NotifyGround(isGrounded));
    }

    public void NotifyAllMovement(Vector3 forceAdded)
    {
        listeners.ForEach(listener => listener.NotifyMovement(forceAdded));
    }

    public void NotifyAllSwitch(EntityControlled controlled)
    {
        listeners.ForEach(listener => listener.NotifySwitch(controlled));
    }

}
