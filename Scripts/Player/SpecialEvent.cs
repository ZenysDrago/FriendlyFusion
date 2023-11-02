using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SpecialEvent : MonoBehaviour, IPlayerListener
{
    private GameController gc;
    private CinemachineFreeLook freeLookCam;

    [Header("Fusion")]
    [SerializeField] private float fusionTime = 0.3f;
    [SerializeField] private GameObject PurpleSmoke;

    [Header("Split")]
    [SerializeField] private float explusionForce = 15f;
    [SerializeField] private GameObject BlueSmoke;
    [SerializeField] private GameObject RedSmoke;

    // Start is called before the first frame update

    void Start()
    {
        freeLookCam = GetComponent<CinemachineFreeLook>();

        gc = FindFirstObjectByType<GameController>();
        if(gc.entityControlled == EntityControlled.LITE)
            UpdateComponents(gc.HeavyBehaviour, gc.LiteBehaviour);
            
        else 
            UpdateComponents(gc.LiteBehaviour, gc.HeavyBehaviour);

        gc.playerEvent.Subscribe(this);
    }

    private bool UpdateComponents(GameObject oldTarget, GameObject newTarget)
    {
        if (newTarget == null)
            return false;

        if (oldTarget != null)
            oldTarget.SetActive(false);

        freeLookCam.Follow = newTarget.transform;
        freeLookCam.LookAt = newTarget.transform;
        freeLookCam.m_YAxis.Value = 0.5f;
        newTarget.SetActive(true);
        return true;
    }

    #region InterfaceFunction

    public void NotifyMovement(Vector3 current)
    {
    }

    public void NotifyGround(bool onGround)
    {
    }

    public void NotifySwitch(EntityControlled controlled)
    {
        // LITE to HEAVY
        if (controlled == EntityControlled.LITE)
        {
            if (UpdateComponents(gc.LiteBehaviour, gc.HeavyBehaviour))
                gc.entityControlled = EntityControlled.HEAVY;
        }
        // HEAVY to LITE
        else if (controlled == EntityControlled.HEAVY)
        {
            if (UpdateComponents(gc.HeavyBehaviour, gc.LiteBehaviour))
                gc.entityControlled = EntityControlled.LITE;
        }
        else
        {
            if(gc.entityControlled != EntityControlled.FUSED)
            {
                StartCoroutine(FusionTransition());
                return;
            }

            GameObject fused = freeLookCam.Follow.gameObject;
            gc.LiteBehaviour.transform.parent.transform.position = fused.transform.position;
            gc.LiteBehaviour.transform.parent.transform.rotation = fused.transform.rotation;
            gc.HeavyBehaviour.transform.parent.transform.position = fused.transform.position;
            gc.HeavyBehaviour.transform.parent.transform.rotation = fused.transform.rotation;
            Instantiate(BlueSmoke, fused.transform.position + Vector3.up * 1.5f, Quaternion.identity);
            Instantiate(RedSmoke, fused.transform.position, Quaternion.identity);
            Destroy(freeLookCam.Follow.gameObject);
            gc.LiteBehaviour.transform.parent.gameObject.SetActive(true);
            gc.HeavyBehaviour.transform.parent.gameObject.SetActive(true);
            freeLookCam.Follow = gc.LiteBehaviour.transform;
            freeLookCam.LookAt = gc.LiteBehaviour.transform;
            gc.LiteBehaviour.SetActive(true);

            Rigidbody liteRb = gc.LiteBehaviour.transform.parent.GetComponent<Rigidbody>();
            liteRb.velocity = new Vector3(0, explusionForce, 0);
            gc.entityControlled = EntityControlled.LITE;
        }
            
    }

    private IEnumerator FusionTransition()
    {
        Transform targetTransform;
        Transform entityTransform;
        if (gc.entityControlled == EntityControlled.LITE)
        {
            targetTransform = gc.HeavyBehaviour.transform.parent.transform;
            entityTransform = gc.LiteBehaviour.transform.parent.transform;
        }
        else
        {
            targetTransform = gc.LiteBehaviour.transform.parent.transform;
            entityTransform = gc.HeavyBehaviour.transform.parent.transform;
        }

        // Disable controls
        gc.LiteBehaviour.SetActive(false);
        gc.HeavyBehaviour.SetActive(false);

        // Transition
        Vector3 initialPos = entityTransform.position;
        Vector3 targetPos = targetTransform.position + Vector3.up;
        Instantiate(PurpleSmoke, targetPos, Quaternion.identity);
        float initialCamRot = freeLookCam.m_YAxis.Value;
        float timer = 0;
        while(timer < fusionTime)
        {
            timer += Time.deltaTime;
            entityTransform.position = Vector3.Slerp(initialPos, targetPos, timer / fusionTime);
            freeLookCam.m_YAxis.Value = Mathf.SmoothStep(initialCamRot, 0.5f, timer / fusionTime);
            yield return null;
        }

        // Spawn Fused Entity
        gc.LiteBehaviour.transform.parent.gameObject.SetActive(false);
        gc.HeavyBehaviour.transform.parent.gameObject.SetActive(false);
        GameObject fusedEntity = Instantiate(gc.Fused, targetPos, targetTransform.rotation);
        freeLookCam.Follow = fusedEntity.transform;
        freeLookCam.LookAt = fusedEntity.transform;
        gc.entityControlled = EntityControlled.FUSED;
    }
    #endregion
}


