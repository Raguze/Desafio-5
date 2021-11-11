using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerEnter : MonoBehaviour
{
    protected virtual void OnTrigger() { }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            OnTrigger();
        }
    }
}
