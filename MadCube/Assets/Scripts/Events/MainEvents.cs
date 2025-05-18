using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MainEvents : MonoBehaviour
{
    [Header("Player Events")]

    public static MainEvents Instance;
    public Action OnPlayerRolled; // Karakter roll bittiðinde.
    public Action<Vector3> OnPlayerTeleported; // Karakter teleport olduðunda.
    public Action OnPlayerFalled; // Player düþtüðünde.

    [Header("Platform Events")]
    public Action<int> OnPlatformSpawning; // int Platform indexi belli ediyor.
    public Action<int> OnPlatformSpawned; // Spawn olduktan sonra

    [Header("Game Events")]
    public Action OnGameRestarting; // Oyun Tekrar baþladýðýnda
    public Action OnGameRestarted;
    public Action OnGameCompleted; // Oyun bittiðinde.
    public Action OnGameOver; // Baþka can kalmadýðýnda
    public Action<bool> OnGamePaused; // Oyun durduðunda. false ise devam ediyor. true ise duruyor.

    public Action OnNewRecord;
     

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
