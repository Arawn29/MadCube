using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private Platform TargetPlatform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("There");
            int SpawnedPlatformIndex = TargetPlatform.PlatformIndex;
            MainEvents.Instance.OnPlatformSpawning?.Invoke(SpawnedPlatformIndex);
            Destroy(gameObject,0.05f);
        }
    }
}
