using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    /// <summary>
    ///     Player
    ///         Jump
    ///         Double Jump
    ///         Wall Jump
    ///         Flutuar
    ///         Walk
    ///         Run
    ///         Dash
    ///         Escalar parede
    ///         Subir Escada (Megaman)
    ///         Puxar/Empurrar
    ///         Pendurar "cipó"
    ///     Sistema de Ranking (pontos, tempo)
    ///     Hud
    /// </summary>


    private Transform playerStartTransform;

    private PlayerController player;
    private HudController hud;

    private int _playerPoints;
    public int PlayerPoints 
    {
        get { return _playerPoints; }
        protected set 
        {
            _playerPoints = value;
            GameEvents.OnChangePlayerPoints.Invoke(_playerPoints);
        }
    }

    private float _levelTime;
    public float LevelTime
    {
        get { return _levelTime; }
        protected set
        {
            _levelTime = value;
            GameEvents.OnChangeLevelTime.Invoke(_levelTime);
        }
    }


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

        HudController hudPrefab = Resources.Load<HudController>("Prefabs/Canvas");
        hud = Instantiate<HudController>(hudPrefab);

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
        PlayerPoints = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
