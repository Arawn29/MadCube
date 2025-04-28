using Cinemachine;
using UnityEngine;

public enum GameState
{
    Playable,
    Over,
    stopped,
    Transporting,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject Player;
    public LayerMask GroundLayerMask;
    public LayerMask DetectionWallsLayerMask;
    MainEvents myEvents;
    [HideInInspector]
    public XRayManager xRayManager;
    SpawnPointController spawnPointController;

    bool isGameOver = false;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        xRayManager = GetComponent<XRayManager>();
        GameState = GameState.Playable;
        spawnPointController = GetComponent<SpawnPointController>();
    }
    private void Start()
    {
        myEvents = MainEvents.Instance;
        myEvents.OnPlayerFall += GameOver;
        myEvents.OnPlayerRolled += DetermineXRayFeasibility;

    }
    private void OnDisable()
    {
        myEvents.OnPlayerFall -= GameOver;
        myEvents.OnPlayerRolled -= DetermineXRayFeasibility;
    }
    private void GameOver()
    {
        GameState = GameState.Over;
        isGameOver = true;
        virtualCamera.Follow = null;
    }
    private void Update()
    {
        if (!isGameOver) return;
        #region isGameOver
        if (GameState == GameState.Over &&
           (Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetKeyDown(KeyCode.R) ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetMouseButtonDown(0)))
        {
            #endregion
            Vector3 playerTransform = spawnPointController.GetSpawnPoint();
            if (playerTransform != null)
            {
                if (Player.TryGetComponent(out Rigidbody rb))
                {
                    Destroy(rb);
                }
                Player.transform.position = playerTransform;
                Player.transform.rotation = Quaternion.Euler(0, 0, 0);
                GameState = GameState.Playable;
                isGameOver = false;
                virtualCamera.Follow = Player.transform;
            }
            else
            {
                Debug.LogWarning("Spawn point not set!");
            }

        }
    }
    public void ChangeGameState(GameState state)
    {
        GameState = state;
    }

    public void DetermineXRayFeasibility()
    {
        xRayManager.DetermineXRayFeasibility(Player, GroundLayerMask);
    }


    private void OnDrawGizmos()
    {
        if (Player != null && Camera.main != null)
        {
            Vector3 direction = (Player.transform.position - Camera.main.transform.position);
            float distance = Vector3.Distance(Camera.main.transform.position, Player.transform.position) * 0.75f;

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
