using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager Instance;
    [SerializeField] private Dictionary<int, GameObject> platforms = new Dictionary<int, GameObject>();
    int CurrentPlatformIndex = 0;
    const float SPAWN_TIME = 1f;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
            Destroy(Instance);
    }
    private void Start()
    {
        if (MainEvents.Instance != null)
        {
            MainEvents.Instance.OnPlatformSpawning += SpawnPlatform;
        }
        else
        {
            Debug.Log("Event instance null!");
        }
    }
    private void OnDisable()
    {
        if (MainEvents.Instance != null)
        {
            MainEvents.Instance.OnPlatformSpawning -= SpawnPlatform;
        }
    }
    public void SpawnPlatform(int index)
    {
        if (index > CurrentPlatformIndex)
        {
            CurrentPlatformIndex = index - 1; 
        }

        if (platforms.ContainsKey(index))
        {
          
            GameObject Platform = platforms[index];
            GameObject PlatformChildObj = Platform.transform.GetChild(0).gameObject;
            if(PlatformChildObj != null && !PlatformChildObj.activeInHierarchy)  PlatformChildObj.SetActive(true); 
            Transform[] PlatformTransforms = Platform.GetComponentsInChildren<Transform>();
            Vector3[] firstPositions = new Vector3[PlatformTransforms.Length];
            for (int i = 0; i < PlatformTransforms.Length; i++)
            {
                firstPositions[i] = PlatformTransforms[i].position;
            }

            foreach (var item in PlatformTransforms)
            {
                item.transform.position += new Vector3(Random.Range(-10f, +10f), Random.Range(-10f, +10f), Random.Range(-4f, +4f));
            }
            for (int i = 0; i < PlatformTransforms.Length; i++)
            {
                PlatformTransforms[i].DOMove(firstPositions[i], SPAWN_TIME);
            }
            StartCoroutine(WaitForInvokeEvent(SPAWN_TIME + 0.5f));
            IEnumerator WaitForInvokeEvent(float seconds)
            {
                yield return new WaitForSeconds(seconds);
                MainEvents.Instance.OnPlatformSpawned?.Invoke();
            }
        }
    }
    public void RegisterPlatformToDýctýonary(int platformIndex, GameObject Platform)
    {
        if (platforms.ContainsKey(platformIndex)) return;
        platforms.Add(platformIndex, Platform);
    }
    public void UnregisterPlatformToDýctýonary(int platformIndex)
    {
        platforms.Remove(platformIndex);
    }
    
    public Platform GetCurrentPlatform()
    {
        if (platforms.TryGetValue(CurrentPlatformIndex, out GameObject platform))
        {
            return platform.GetComponent<Platform>();
        }
        else return null;

    }
}
