using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeavyPlayer : SpecificPlayerBehaviour
{
    [SerializeField] private float timeTranslation = 0.2f;
    private bool inPushMode;
    private bool pushing;
    
    public MoveableBox boxPushed;
    public bool Push
    {
        set => PushSetup(value);
    }
    
    public bool InPushMode => inPushMode;

    // Start is called before the first frame update
    void Start()
    {
        playerBase = GetComponent<PlayerBase>();
        rbody = GetComponentInParent<Rigidbody>();
        gc = FindObjectOfType<GameController>();
        gc.playerEvent.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void PushSetup(bool value)
    {
        if (value && pushing)
            return;
        
        pushing = value;
        playerBase.pushingMovement = value;
        
        if (value)
        {
            StartCoroutine(TranslatePosition());
        }
    }


    private IEnumerator TranslatePosition()
    {
        rbody.velocity = new Vector3(0, 0, 0);
        Transform trParent = transform.parent;
        boxPushed.pushStrength = playerBase.Speed / 2;
        Transform boxTransform = boxPushed?.transform;
        Vector3 parentInitPos = trParent.position;
        Vector3 parentInitFor = trParent.forward;
        Vector3 dir = parentInitPos - boxTransform.position;
        Vector3 scale = boxTransform.right * ((boxTransform.localScale.x  + transform.localScale.x) * (HelperFunction.VectorInSameDir(boxTransform.right,dir,89) ? 1 : -1));
        Vector3 newPosition = boxTransform.position + scale;
        float timer = 0f;
        Vector3 finalDir= new Vector3(newPosition.x, trParent.position.y, newPosition.z) - boxTransform.position;
        playerBase.movementLock = true;

        while (timer < timeTranslation)
        {
            timer += Time.deltaTime;
            trParent.position = Vector3.Slerp(parentInitPos, new Vector3(newPosition.x, trParent.position.y, newPosition.z),
                timer / timeTranslation);
            trParent.forward = Vector3.Slerp(parentInitFor,new Vector3(-finalDir.x, 0, -finalDir.z), timer/timeTranslation);
           yield return null;
        }

        playerBase.movementLock = false;
        rbody.velocity = new Vector3(0, 0, 0);
        if(boxPushed)
            boxPushed.transform.parent = transform.parent;
    }
    
    #region InputEvents

    public override void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed || inPushMode)
            return;
        
        Debug.Log("JUMP");

        if (playerBase.CanJump)
            playerBase.Rigidbody.velocity = new Vector3(playerBase.Rigidbody.velocity.x, jumpForce, playerBase.Rigidbody.velocity.z);
    }

    public override void Action(InputAction.CallbackContext context)
    {
        // if (context.started && boxCollided != null)
        //     StartCoroutine(PlacePlayer());

        inPushMode = !context.canceled;
        if(!inPushMode)
            StopCoroutine(TranslatePosition());
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
