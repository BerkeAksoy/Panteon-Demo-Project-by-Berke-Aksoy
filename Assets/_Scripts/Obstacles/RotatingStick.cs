using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingStick : MonoBehaviour
{
    private float pushForce = -4f;
    private void OnCollisionEnter(Collision collision) // Our rotator object currently uses this function
    {
        GameObject collidedObj = collision.gameObject;

        Debug.Log("Collision happened");

        if (collidedObj.GetComponent<Character>())
        {
            Debug.Log("Collision happened via char");
            Vector3 forceVector = new Vector3(0f, 0f, pushForce);
            Rigidbody colObjRb = collidedObj.GetComponent<Rigidbody>();

            colObjRb.AddForce(forceVector, ForceMode.Impulse);

            Debug.Log("We need to see force");
        }
    }

    private void OnTriggerEnter(Collider other) // But in case of thought change, we can use this
    {
        GameObject collidedObj = other.gameObject;

        Debug.Log("Collision happened T");

        if (collidedObj.GetComponent<Character>())
        {
            Debug.Log("Collision happened via char T");
            Vector3 forceVector = new Vector3(0f, 0f, pushForce);
            Rigidbody colObjRb = collidedObj.GetComponent<Rigidbody>();

            colObjRb.AddForce(forceVector, ForceMode.Impulse);

            Debug.Log("We need to see force T");
        }
    }
}
