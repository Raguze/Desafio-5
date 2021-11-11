using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndPoint : PlayerTriggerEnter
{
    protected override void OnTrigger()
    {
        Debug.Log("End Level");
        GameEvents.OnEndLevel.Invoke();
    }
}
