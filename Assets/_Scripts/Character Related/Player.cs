using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8) // If the layer is Hazard
        {
            GameManager.Instance.Restart();
        }
    }
}
