using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

public enum GameState
{
    Playable,
    Unplayable,
    Paused,
    Over,
    Transporting,
    Completed,
    Reloading,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState CurrentState;
    GameState previousGameState;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject PlayerObj;
    public LayerMask GroundLayerMask;
    public LayerMask DetectionWallsLayerMask;
    MainEvents myEvents;
    [HideInInspector]
    public XRayManager xRayManager;
    SpawnPointController spawnPointController;
    bool isPlayerFalled = false;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        xRayManager = GetComponent<XRayManager>();
        CurrentState = GameState.Playable;
        previousGameState = GameState.Playable;
        spawnPointController = GetComponent<SpawnPointController>();
    }
    private void Start()
    {
        myEvents = MainEvents.Instance;
        myEvents.OnPlayerFalled += PlayerFalling;
        myEvents.OnPlayerRolled += DetermineXRayFeasibility;
        myEvents.OnGameRestarting += RespawnPlayer;
        myEvents.OnGameCompleted += GameCompleted;
        myEvents.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        myEvents.OnPlayerFalled -= PlayerFalling;
        myEvents.OnPlayerRolled -= DetermineXRayFeasibility;
        myEvents.OnGameRestarting -= RespawnPlayer;
        myEvents.OnGameCompleted -= GameCompleted;
        myEvents.OnGameOver -= GameOver;
    }

    float et;
    bool isPaused = false;
    private void Update()
    {
        if (CurrentState == GameState.Over && Input.GetKeyDown(KeyCode.R))
        {
            ResetScene();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PauseScene();
        }
        if (!isPlayerFalled) return;
        if (CurrentState == GameState.Unplayable)
        {
            et += Time.deltaTime;
            if (et >= 1f)
            {
                myEvents.OnGameRestarting?.Invoke();
                et = 0f;
            }

        }
    }
    public void ResetScene()
    {
        MainCanvaScript.Instance.SetSceneInterpolatePan();
        ChangeGameState(GameState.Reloading);
    }
    public void PauseScene()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        myEvents.OnGamePaused?.Invoke(isPaused);
    }
    public void ChangeGameState(GameState state)
    {
        previousGameState = CurrentState;
        CurrentState = state;
    }
    public void DetermineXRayFeasibility()
    {
        xRayManager.DetermineXRayFeasibility(PlayerObj, GroundLayerMask);
    }
    private void PlayerFalling()
    {
        ChangeGameState(GameState.Unplayable);
        isPlayerFalled = true;
        virtualCamera.Follow = null;
    }
    private void GameCompleted()
    {
        ChangeGameState(GameState.Completed);
    }
    void GameOver()
    {
        ChangeGameState(GameState.Over);
    }
    async void RespawnPlayer()
    {

        Vector3 playerTransform = spawnPointController.GetSpawnPoint();
        if (playerTransform != null)
        {
            if (PlayerObj.TryGetComponent(out Rigidbody rb))
            {
                Destroy(rb);
            }
            PlayerObj.transform.position = playerTransform;
            PlayerObj.transform.rotation = Quaternion.Euler(0, 0, 0);
            isPlayerFalled = false;
            virtualCamera.Follow = PlayerObj.transform;
            await Task.Delay(300);
            ChangeGameState(GameState.Playable);
            myEvents.OnGameRestarted?.Invoke();
        }
        else
        {
            Debug.LogWarning("Spawn point not set!");
        }
    }

    private void OnDrawGizmos()
    {
        if (PlayerObj != null && Camera.main != null)
        {
            Vector3 direction = (PlayerObj.transform.position - Camera.main.transform.position);
            float distance = Vector3.Distance(Camera.main.transform.position, PlayerObj.transform.position) * 0.75f;

            // Draw the ray for the BoxCast
            Gizmos.color = Color.red;
            Gizmos.DrawRay(Camera.main.transform.position, direction);

            // Visualize the BoxCast
            Gizmos.color = Color.green;
            Vector3 boxSize = new Vector3(1f, 0.1f, 1f);
            Quaternion orientation = Quaternion.identity;

            // Calculate the center of the box at the end of the cast
            Vector3 boxEndCenter = Camera.main.transform.position + direction.normalized * distance;

            // Draw the initial box
            Gizmos.matrix = Matrix4x4.TRS(Camera.main.transform.position, orientation, boxSize);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

            // Draw the final box
            Gizmos.matrix = Matrix4x4.TRS(boxEndCenter, orientation, boxSize);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

            // Reset Gizmos matrix
            Gizmos.matrix = Matrix4x4.identity;
        }
    }

}
