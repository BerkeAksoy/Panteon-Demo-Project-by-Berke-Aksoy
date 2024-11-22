using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;

    [SerializeField] private Vector3 camOffsetRun, camOffsetPaint, camRotRun, camRotPaint;
    [SerializeField] private Transform target, lookTarget;
    [SerializeField] private float smoothTime, rotationSpeed = 5f;
    Quaternion targetRotation;
    Vector3 camOffset;

    public static CameraManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Camera manager is null.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance == this) { return; }
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetCamForRun();
    }

    private void FixedUpdate() // To eleminate character jitter while moving we must update the cam position in fixedUpdate as well
    {
        followTarget();
    }

    private void followTarget()
    {
        if (!target)
            target = GameObject.FindGameObjectWithTag("Player").transform;
        if (!target)
            return;

        Vector3 desiredPos = target.position + camOffset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothTime * Time.fixedDeltaTime);
        transform.position = smoothedPos;

        if (lookTarget != null)
        {
            Vector3 direction = lookTarget.position - transform.position; // Calculate the direction to the target
            targetRotation = Quaternion.LookRotation(direction); // Calculate the target rotation
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime); // Smoothly interpolate between the current and target rotation
    }

    public void SetCamForRotPlatforms(Transform transform)
    {
        lookTarget = transform;
    }

    public void SetCamForRun()
    {
        lookTarget = null;
        camOffset = camOffsetRun;
        targetRotation = Quaternion.Euler(camRotRun);
    }

    public void SetCamForPainting(Transform transform)
    {
        lookTarget = null;
        target = transform;
        camOffset = camOffsetPaint;
        targetRotation = Quaternion.Euler(camRotPaint);
    }

}
