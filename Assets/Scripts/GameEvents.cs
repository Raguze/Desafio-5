using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntEvent : UnityEvent<int> { }

public class GameEvents
{
    static public UnityEvent OnEndLevel = new UnityEvent();
    static public UnityEvent<int> OnCollectable = new IntEvent();
}
