using UnityEngine;

public class FinishLine : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MainEvents.Instance.OnGameCompleted?.Invoke();
        }
    }
}
