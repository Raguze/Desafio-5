using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    /// <summary>
    ///     Double Jump
    ///     Dash
    ///     Flutuar
    ///     Wall Jump
    ///     Sistema de Ranking (pontos, tempo)
    ///     Run
    ///     Puxar/Empurrar
    ///     Pendurar "cipó"
    ///     Escalar parede
    ///     Subir Escada (Megaman)
    ///     Hud
    /// </summary>


    private Transform playerStartTransform;

    private PlayerController player;

    private int PlayerPoints = 0;

    private void Awake()
    {
        LevelStartPoint startPoint = GameObject.FindObjectOfType<LevelStartPoint>();
        playerStartTransform = startPoint.transform;

        PlayerController playerPrefab = Resources.Load<PlayerController>("Prefabs/Player");
        player = Instantiate<PlayerController>(playerPrefab, playerStartTransform.position, playerStartTransform.rotation);

        CameraController cameraController = Camera.main.gameObject.AddComponent<CameraController>();
        cameraController.SetTarget(player.gameObject.transform);

        GameEvents.OnEndLevel.AddListener(LevelEnd);
        GameEvents.OnCollectable.AddListener(OnCollect);

    }

    private void LevelEnd()
    {
        Debug.Log("GameController LevelEnd");
        SceneManager.LoadScene("Level1");
    }

    private void OnCollect(int points)
    {
        PlayerPoints += points;
        Debug.Log(PlayerPoints);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
