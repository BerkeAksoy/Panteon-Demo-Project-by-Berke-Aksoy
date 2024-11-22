using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(NavMeshAgent))]

public class Enemy : Character
{
    public enum IntelligenceLevel
    {
        Newbee,
        Amateur,
        SemiPro,
        Pro,
        President,
        Player
    }

    private IntelligenceLevel intLevel;
    private int priority;
    private NavMeshAgent meshAgent;
    private RaycastHit meshHitInfo = new RaycastHit();
    private bool setBefore;

    protected override void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if(Input.GetMouseButtonDown(0) && !setBefore){
            meshAgent.destination = finishPos;
            animator.SetBool("Running", true);
            setBefore = true;
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.y < 0.3f)
        {
            transform.position = startPos;
        }
    }

    protected override void UnpackCharAttributes()
    {
        base.UnpackCharAttributes();
        priority = charAttributes.priority;
        intLevel = charAttributes.intelligenceLevel;
    }

    protected override void ApplyUnpackedData()
    {
        base.ApplyUnpackedData();
        meshAgent.speed = runSpeed;
        meshAgent.avoidancePriority = priority;
        meshAgent.obstacleAvoidanceType = DefineAvoidanceType();
        meshAgent.acceleration = UnityEngine.Random.Range(0.5f, 4f);    // Maybe she is good but tired
    }

    private ObstacleAvoidanceType DefineAvoidanceType()
    {
        switch (intLevel)
        {
            case IntelligenceLevel.Newbee:
                return ObstacleAvoidanceType.NoObstacleAvoidance;
            case IntelligenceLevel.Amateur:
                return ObstacleAvoidanceType.LowQualityObstacleAvoidance;
            case IntelligenceLevel.SemiPro:
                return ObstacleAvoidanceType.MedQualityObstacleAvoidance;
            case IntelligenceLevel.Pro:
                return ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
            case IntelligenceLevel.President:
                return ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            default:
                return ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8) // If the layer is Hazard
        {
            transform.position = startPos;
        }
    }

    public void StopRunning()
    {
        animator.SetBool("Running", false);
        meshAgent.speed = 0;
    }

    private void MoveToClick() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out meshHitInfo))
        {
            meshAgent.destination = meshHitInfo.transform.position;
        }
    }
}
