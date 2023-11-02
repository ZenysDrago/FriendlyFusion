using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

public class PlayerBase : MonoBehaviour
{
    [Header("Movements")]
    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float groundDrag = 5;
    [SerializeField] private float airMultiplier = 0.6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    private Vector2 move;
    private Vector3 direction;
    private float turnSmoothVelocity;
    [SerializeField] private bool isGrounded;
    private bool canJump = true;
    [HideInInspector] public bool CanJump => canJump;

    [Header("Jump")]
    [SerializeField] private GameObject groundChecker;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private float jumpWindow = 0.1f;
    private float jumpTimming = 0;

    [Header("Fusion")]
    [SerializeField] private float fusionRadius = 2.5f;

    [Header("Other")]
    private GameController gc;
    private Rigidbody rbody;
    private Animator animator;
    [SerializeField] private StudioGlobalParameterTrigger  emitterGameEvent;
    public List<StudioEventEmitter> fmodEmitters;
    /* 0 = switch , */
    

    private bool switchPressed = false;
    public float Speed => moveSpeed;
    public Rigidbody Rigidbody => rbody;
    public bool pushingMovement;
    public bool movementLock;
    
    // Start is called before the first frame update
    void Start()
    {
        gc = FindFirstObjectByType<GameController>();
        rbody = GetComponentInParent<Rigidbody>();
        animator = transform.parent.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SpeedLimitation();

        GroundCheck();
        
        if(!pushingMovement && !movementLock)
            MoveBehaviourGround();
        else
            MoveBehaviourPushing();
    }
    
    private void FixedUpdate()
    {
        if (movementLock)
            return;
        
        /* combine the speed and set the velocity to the new vector without touching to the y velocity so jump and aerial movement aren't affected */
        float mult = 10f;

        Vector3 movementForce = mult * (pushingMovement ? moveSpeed/2 : moveSpeed) * (isGrounded ? 1 : airMultiplier) * direction;
        
        rbody.AddForce(movementForce, ForceMode.Force);
        if(direction != Vector3.zero)
            gc.playerEvent.NotifyAllMovement(rbody.velocity);
    }

    private void MoveBehaviourGround()
    {
        /* Get the values we want to avoid multiple get */
        /* We make the player move according to the camera so we use the camera forward and right vectors */
        Vector3 camForward = new Vector3(gc.cam.forward.x, 0, gc.cam.forward.z).normalized;
        Vector3 camRight = new Vector3(gc.cam.right.x, 0, gc.cam.right.z).normalized;

        /* normalize the direction to avoid diagonal overspeed */
        if (move.magnitude > 1)
            move.Normalize();

        if(animator != null)
            animator.SetFloat("Speed", move.magnitude);

        if (move.magnitude < 0.1f)
            move = Vector2.zero;
        direction = (camForward * move.y + camRight * move.x).normalized;

        // Smooth turn arround
        if (move.magnitude >= 0.1f)
        {
            Vector3 normVel = direction;
            float targetAngle = Mathf.Atan2(normVel.x, normVel.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.parent.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void MoveBehaviourPushing()
    {
        /* Get the values we want to avoid multiple get */
        /* We make the player move according to the camera so we use the camera forward and right vectors */
        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        direction = (forward * move.y).normalized;

    }
    
    private void SpeedLimitation()
    {
        Vector3 flatVel = new Vector3(rbody.velocity.x, 0f, rbody.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rbody.velocity = new Vector3(limitedVel.x, rbody.velocity.y, limitedVel.z);
        }
    }
    
    private void GroundCheck()
    {
        isGrounded = Physics.OverlapSphere(groundChecker.transform.position, groundCheckRadius, 3).Length != 0;
        if (isGrounded)
        {
            jumpTimming = 0;
            canJump = true;
            rbody.drag = groundDrag;
        }
        else
        {
            if (canJump)
            {
                jumpTimming += Time.deltaTime;
                if (jumpTimming > jumpWindow)
                    canJump = false;
            }
            rbody.drag = 0;
        }
        gc.playerEvent.NotifyAllGround(isGrounded);
    }

    private void ResetAnimator()
    {
        animator.SetFloat("Speed", 0);
    }

    #region InputEvents

    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        // Debug.Log("MOVE");
    }

    public void Switch(InputAction.CallbackContext context)
    {
        if (gc.entityControlled == EntityControlled.FUSED)
            return;
        
        if (context.performed && gc.canSwitch)
        {
            gc.transition.SetTrigger("Transition");
            gc.EnableSwitch(false);
            ResetAnimator();
            gameObject.SetActive(false);
            //switchPressed = true;
        }
        
        // if (context.canceled)
        //     switchPressed = false;
        //
        // fmodEmitters[0].SendMessage("Play");
        // emitterGameEvent.Value = gc.entityControlled == EntityControlled.LITE ? 1 : 0 ;
        // emitterGameEvent.TriggerParameters();
    }

    public void Fusion(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        
        if (gc.entityControlled == EntityControlled.FUSED)
        {
            /* Split players */
            // fmodEmitters[0].SendMessage("Play");
            // emitterGameEvent.Value = gc.entityControlled == EntityControlled.LITE ? 1 : 0 ;
            // emitterGameEvent.TriggerParameters();
            //
            
            gc.playerEvent.NotifyAllSwitch(EntityControlled.FUSED);
        }
        else
        {
            float distance = (gc.LiteBehaviour.transform.position - gc.HeavyBehaviour.transform.position).magnitude;
            if (distance <= fusionRadius)
            {
                /* Fusion players */
                // fmodEmitters[0].SendMessage("Play");
                // emitterGameEvent.Value = gc.entityControlled == EntityControlled.LITE ? 1 : 0 ;
                // emitterGameEvent.TriggerParameters();

                gc.playerEvent.NotifyAllSwitch(EntityControlled.FUSED);
            }
        }
    }
    
    #endregion

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(groundChecker.transform.position, groundCheckRadius);
    // }
}
