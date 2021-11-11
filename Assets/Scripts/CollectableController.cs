using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : PlayerTriggerEnter
{
    public int Points = 1;

    protected override void OnTrigger()
    {
        GameEvents.OnCollectable.Invoke(Points);
        Destroy(gameObject);
    }
}
