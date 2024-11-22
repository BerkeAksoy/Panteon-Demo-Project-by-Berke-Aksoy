using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public abstract class Collectable : MonoBehaviour, ICollectable
{
    public abstract void Collect(Collider collector);

    private void OnTriggerEnter(Collider other)
    {
        Collect(other);
        Destroy(gameObject);
    }
}
