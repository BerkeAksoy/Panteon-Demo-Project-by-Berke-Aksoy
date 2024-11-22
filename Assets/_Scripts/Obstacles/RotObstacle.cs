using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObstacle : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    enum RotAxis
    {
        X,
        Y,
        Z
    }

    [SerializeField] [Range(-200f, 200f)] private float maxRotationSpeed = 25f;
    [SerializeField] private Difficulty difficulty = Difficulty.Easy;
    [SerializeField] [Range(0f, 3f)] [Tooltip("The time that the platform will rotate at its max speed")] private float stableTime = 1f;
    [SerializeField] [Range(2f, 10f)] [Tooltip("Time interval to change direction")] private float interval = 3f; // Default time to change direction for medium difficulty
    [SerializeField] RotAxis rotAxis = RotAxis.Z;
    private Rigidbody rb;
    private float currentSpeed = 0f, rotTimer = 0f, hardRandomInterval = 2f, minInterval = 1f, maxInterval = 4f;
    private float direction = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //if (rb == null)
        //Debug.LogError("Rigidbody is missing on the rotating platform!");

        if (difficulty == Difficulty.Easy)
            currentSpeed = maxRotationSpeed;
        else if (difficulty == Difficulty.Hard) // Randomize the first interval for Hard mode
            hardRandomInterval = Random.Range(minInterval, maxInterval);
    }

    private void FixedUpdate()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:       // No rotation change
                RotateConstantSpeed();
                break;

            case Difficulty.Medium:
                RotateVaryingSpeed(maxRotationSpeed, interval); // Medium uses non changing interval
                break;

            case Difficulty.Hard:
                RotateVaryingSpeed(maxRotationSpeed, hardRandomInterval);   // Rotation changes at random intervals
                break;
        }
    }

    private void RotateConstantSpeed()
    {
        if (!rb)
        {
            transform.Rotate(CalcDeltaRotation(currentSpeed * Time.fixedDeltaTime).eulerAngles);
        }
        else
        {
            rb.MoveRotation(rb.rotation * CalcDeltaRotation(currentSpeed * Time.fixedDeltaTime));
        }
    }

    void RotateVaryingSpeed(float maxSpeed, float interval) // Accelerate or decelerate between 0 and maxSpeed
    {
        rotTimer += Time.fixedDeltaTime;
        float halfInterval = interval / 2f;

        if (rotTimer <= halfInterval) // Accelerating
        {
            currentSpeed = Mathf.Lerp(0f, maxSpeed, rotTimer / halfInterval);
        }
        else if (rotTimer <= halfInterval + stableTime) // Keep the rotation as long as the stable time
        {
            currentSpeed = maxSpeed;
        }
        else if (rotTimer <= interval + stableTime) // Decelerating
        {
            currentSpeed = Mathf.Lerp(maxSpeed, 0f, (rotTimer - halfInterval - stableTime) / halfInterval);
        }
        else // Change direction and reset rotTimer
        {
            rotTimer = 0f;
            direction *= -1f;

            if (difficulty == Difficulty.Hard) // For Hard mode, randomize the next interval
                hardRandomInterval = Random.Range(1f, 4f);
        }

        if (!rb)
        {
            transform.Rotate(CalcDeltaRotation(direction * currentSpeed * Time.fixedDeltaTime).eulerAngles);
        }
        else
        {
            rb.MoveRotation(rb.rotation * CalcDeltaRotation(direction * currentSpeed * Time.fixedDeltaTime));
        }
    }

    private Quaternion CalcDeltaRotation(float value)
    {
        Quaternion deltaRotation;

        if (rotAxis == RotAxis.X)
        {
            deltaRotation = Quaternion.Euler(value, 0f, 0f);
        }
        else if (rotAxis == RotAxis.Y)
        {
            deltaRotation = Quaternion.Euler(0f, value, 0f);
        }
        else
        {
            deltaRotation = Quaternion.Euler(0f, 0f, value);
        }

        return deltaRotation;
    }

    public Difficulty GetDifficulty()
    {
        return difficulty;
    }

    public float GetCurrentRotDir()
    {
        return direction*Mathf.Sign(currentSpeed);
    }

}
