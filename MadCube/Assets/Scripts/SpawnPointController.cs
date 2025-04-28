using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    SpawnPoint spawnPoint;
    [SerializeField] private GameObject TargetObject;
    private void Start()
    {
        spawnPoint = new SpawnPoint(TargetObject.transform.position);
        MainEvents.Instance.OnPlayerTeleported += ChangeSpawnPoint;
    }
    private void OnDisable()
    {
        MainEvents.Instance.OnPlayerTeleported -= ChangeSpawnPoint;
    }
    private void ChangeSpawnPoint(Vector3 newSpawnPoint)
    {
        Debug.Log("Yeni Spawn point" + newSpawnPoint);
        spawnPoint.SetSpawnPoint(newSpawnPoint);
    }

    public Vector3 GetSpawnPoint()
    {
        Debug.Log("Dönen Teleport position" + spawnPoint.GetSpawnPoint());
        return spawnPoint.GetSpawnPoint();
    }
}

public class SpawnPoint
{
    public Vector3 spawnPosition;

    public SpawnPoint(Vector3 spawnPosition)
    {
        this.spawnPosition = spawnPosition;
    }

    public void SetSpawnPoint(Vector3 newSpawnPosition)
    {
        spawnPosition = newSpawnPosition;
    }
    public Vector3 GetSpawnPoint()
    {
        return spawnPosition;
    }
}
