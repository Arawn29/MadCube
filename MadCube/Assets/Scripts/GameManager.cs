using Cinemachine;
using System;
using UnityEngine;

public enum GameState
{
    Playing,
    Over,
    stopped,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    Events myEvents; 
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        myEvents = Events.instance;
        GameState = GameState.Playing;
    }
    private void OnEnable()
    {
        myEvents.OnPlayerFall += GameOver;
    }
    private void OnDisable()
    {
        myEvents.OnPlayerFall -= GameOver;
    }
    private void GameOver()
    {
        GameState = GameState.Over;
        virtualCamera.Follow = null;
    }

    public void ChangeGameState(GameState state)
    {
        GameState = state;
    }

}
