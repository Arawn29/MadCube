using System;
using UnityEngine;

public class MainEvents : MonoBehaviour
{

    [Header("Player Events")]

    public static MainEvents Instance;
    public Action OnPlayerFall; // Karakter düþtüðünde.
    public Action OnPlayerRolled; // Karakter roll bittiðinde.
    public Action<Vector3> OnPlayerTeleported; // Karakter teleport olduðunda.

    [Header("Platform Events")]
    public Action<int> OnPlatformSpawning; // int Platform indexi belli ediyor.
    public Action OnPlatformSpawned; // Spawn olduktan sonra
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
