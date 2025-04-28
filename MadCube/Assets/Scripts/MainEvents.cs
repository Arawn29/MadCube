using System;
using UnityEngine;

public class MainEvents : MonoBehaviour
{

    [Header("Player Events")]

    public static MainEvents Instance;
    public Action OnPlayerFall; // Karakter d��t���nde.
    public Action OnPlayerRolled; // Karakter roll bitti�inde.
    public Action<Vector3> OnPlayerTeleported; // Karakter teleport oldu�unda.

    [Header("Platform Events")]
    public Action<int> OnPlatformSpawning; // int Platform indexi belli ediyor.
    public Action OnPlatformSpawned; // Spawn olduktan sonra
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
