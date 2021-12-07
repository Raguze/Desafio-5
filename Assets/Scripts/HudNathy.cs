using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudNathy : MonoBehaviour
{
    // Start is called before the first frame update
    float tempo;
    public GameObject tempoHud;
 
    void Start()
    {
        tempo = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        tempo += Time.deltaTime;
        tempoHud.GetComponent<Text>().text = Mathf.Floor(tempo).ToString();
       
    }
}
