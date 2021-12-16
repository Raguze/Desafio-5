using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public Text TextPlayerPoints;

    private void Awake()
    {
        List<Text> texts = new List<Text>();
        texts.AddRange(GetComponentsInChildren<Text>());

        foreach (var text in texts)
        {
            switch (text.gameObject.name)
            {
                case "PlayerPoints":
                    TextPlayerPoints = text;
                    break;
                default:
                    break;
            }
        }

        // Events
        GameEvents.OnChangePlayerPoints.AddListener(OnChangePlayerPoints);
    }

    void OnChangePlayerPoints(int points)
    {
        TextPlayerPoints.text = $"Player Points {points}";
    }
}
