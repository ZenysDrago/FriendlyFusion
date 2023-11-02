using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FusionPlayer : SpecificPlayerBehaviour
{
    [SerializeField] private GameObject hitboxPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        playerBase = GetComponent<PlayerBase>();
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
        
        Debug.Log("STOMP");
        Vector3 posHitbox = transform.position + (transform.up * -1) * transform.localScale.magnitude * 2 ;
        Instantiate(hitboxPrefab, posHitbox, Quaternion.identity);
    }

    public override void Action(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        /* Nothing to do with this input */
        Debug.Log("ACTION");
        Vector3 posHitbox = transform.position + transform.forward * transform.localScale.magnitude * 2;
        Instantiate(hitboxPrefab, posHitbox, Quaternion.identity);
    }
    #endregion

    
    #region InterfaceFunction

    public override void NotifyMovement(Vector3 current)
    {
        /*not needed*/
    }

    public override void NotifyGround(bool onGround)
    {
    }
    
    public override void NotifySwitch(EntityControlled controlled)
    {
        
    }
    #endregion
}
