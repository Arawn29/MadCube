using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [Header("HealthBarParameters")]
    HealthBarGenerator healthBarGenerator;
    [SerializeField] private int lifeCount = 3;
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private float xOffset = 0f;
    [SerializeField] private float yOffset = 0f;
    [SerializeField] private float xSpaceBetweenObjects = 0.5f;

    private void Start()
    {
        healthBarGenerator = new HealthBarGenerator(lifeCount, healthBarPrefab, xOffset, yOffset, xSpaceBetweenObjects, transform);
        MainEvents.Instance.OnPlayerFalled += OnPlayerFalled;
    }
    private void OnDisable()
    {
        if (MainEvents.Instance != null)
        {
            MainEvents.Instance.OnPlayerFalled -= OnPlayerFalled;
        }
    }

    private void OnPlayerFalled()
    {
        if (lifeCount <= 1)
        {
            MainEvents.Instance.OnGameOver?.Invoke();
        }
        healthBarGenerator.GetHealthBar(lifeCount).GetComponent<HealthBar>().KillHealthBar?.Invoke();
        --lifeCount;
    }
}
public class HealthBarGenerator
{
    
    int lifeCount;
    GameObject healthBarPrefab;
    float xOffset;
    float xSpaceBetweenObjects;
    float yOffset;
    Transform parent;
    Dictionary<int, GameObject> healthBars = new Dictionary<int, GameObject>(); 
    public HealthBarGenerator(int lifeCount, GameObject healthBarPrefab, float xOffset, float yOffset, float xSpaceBetweenObjects, Transform parent)
    {
        this.lifeCount = lifeCount;
        this.healthBarPrefab = healthBarPrefab;
        this.xOffset = xOffset;
        this.yOffset = yOffset;
        this.xSpaceBetweenObjects = xSpaceBetweenObjects;
        this.parent = parent;

        GenerateHealthBar();
    }

    private void GenerateHealthBar()
    {
        for (int i = 1; i <= lifeCount; i++)
        {
            Vector2 offsetPosition = GetOffset(i);
            GameObject created = Object.Instantiate(healthBarPrefab, offsetPosition, Quaternion.identity, parent);
            created.name = "HealthBar"+ i;
            healthBars.Add(i, created);
        }
    }

    public GameObject GetHealthBar(int index)
    {
        if (healthBars.TryGetValue(index, out GameObject healthBar))
        {
            return healthBar;
        }
        return null;
    }
    Vector2 GetOffset(int index)
    {
        return new Vector2(xOffset + (index-1) * xSpaceBetweenObjects, yOffset);
    }
}

