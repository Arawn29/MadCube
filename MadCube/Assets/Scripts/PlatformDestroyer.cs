using System.Collections;
using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    // Kendinden Onceki Platformu yok eden basit script

    [SerializeField] private Platform TargetPlatform;

    const float DESTROYTIME = 0.05f;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            int DestroyedPlatformIndex = TargetPlatform.PlatformIndex;
            StartCoroutine(DestroyProcces(DestroyedPlatformIndex));

        }
    }
    IEnumerator DestroyProcces(int DestroyedPlatformIndex)
    {
        yield return new WaitForSeconds(DESTROYTIME);
        TargetPlatform.DestoryPlatform();
        Destroy(gameObject, DESTROYTIME);
    }
}
