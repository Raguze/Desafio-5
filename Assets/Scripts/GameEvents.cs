using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntEvent : UnityEvent<int> { }
public class FloatEvent : UnityEvent<float> { }

public class GameEvents
{
    static public UnityEvent OnEndLevel = new UnityEvent();
    static public UnityEvent<int> OnCollectable = new IntEvent();

    static public UnityEvent<int> OnChangePlayerPoints = new IntEvent();
    static public UnityEvent<float> OnChangeLevelTime = new FloatEvent();

}
