using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudNathy : MonoBehaviour
{
    float tempo;
    public Text timeUI;
 
    void Start()
    {
        timeUI.text = "TEMPO: 0";
    }

    void Update()
    {
        tempo += Time.deltaTime;

        timeUI.text = $"TEMPO: {Mathf.Floor(tempo).ToString()}";
    }
}
