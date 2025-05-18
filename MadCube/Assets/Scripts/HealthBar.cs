using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Tween shakeTween;
    public Action KillHealthBar;
    void Start()
    {
        shakeTween = StartShakeTween();
        KillHealthBar += StopShake;
        KillHealthBar += ChangeColor;
    }
    public void OnDisable()
    {
        KillHealthBar -= StopShake;
        KillHealthBar -= ChangeColor;
    }
    Tween StartShakeTween()
    {
        Debug.Log("StartShakeTween");
        return transform.DOShakeRotation(1f, strength: new Vector3(0, 0, 2.5f), vibrato: 10, randomness: 90f)
                 .SetEase(Ease.OutBack)
                 .SetDelay(UnityEngine.Random.Range(0f, 0.25f))
                 .SetLoops(-1, LoopType.Yoyo);
    }
    void StopShake()
    {
        if (shakeTween != null && shakeTween.IsActive())
        {
            shakeTween.Kill();
            shakeTween = null;
        }
    }
    void ChangeColor()
    {
        var image = transform.GetComponent<Image>();
        Color targetColor = new Color(63f / 255f, 63f / 255f, 63f / 255f, 1f);
        image.DOColor(targetColor, 0.5f);
    }

}
