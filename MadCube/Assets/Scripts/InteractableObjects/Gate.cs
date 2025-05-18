using DG.Tweening;
using UnityEngine;

public class Gate : MonoBehaviour, IButtonListener
{
    public enum GateStrechtDirection
    {
        forward, backward, left, right
    }

    public int requiredButton;
    [SerializeField] private GateStrechtDirection direction;
    [SerializeField] private int strechtAmount;
    const float STRECH_TIME = 1.5f;

    bool isActivated = false;
    int currentPressedButton;
    public void ButtonPressed()
    {
        ++currentPressedButton;
        if (currentPressedButton >= requiredButton && !isActivated)
        {

            // Yön vektörünü al (local space)
            Vector3 localDir = DirectionVector(direction);

            // Ekseni ve scale farkýný bul
            Vector3 oldScale = transform.localScale;
            Vector3 newScale = oldScale;

            // Hangi eksende büyüyeceðimizi belirle
            if (Mathf.Abs(localDir.x) > 0f)
            {
                newScale.x = strechtAmount;
            }
            else if (Mathf.Abs(localDir.y) > 0f)
            {
                newScale.y = strechtAmount;
            }
            else if (Mathf.Abs(localDir.z) > 0f)
            {
                newScale.z = strechtAmount;
            }

            // Offset = scale farkýnýn yarýsý kadar, yön vektörüne göre
            Vector3 scaleDelta = newScale - oldScale;
            Vector3 offset = new Vector3(
                localDir.x * scaleDelta.x * 0.5f,
                localDir.y * scaleDelta.y * 0.5f,
                localDir.z * scaleDelta.z * 0.5f
            );

            // Local yönü world space'e çevirerek pozisyonu kaydýr
            Vector3 worldOffset = transform.rotation * offset;

            // Uygula
            transform.DOScale(newScale, STRECH_TIME).SetEase(Ease.Linear);
            transform.DOMove(transform.position + worldOffset, STRECH_TIME).SetEase(Ease.Linear);
        }
    }
    
    Vector3 DirectionVector(GateStrechtDirection direction)
    {
        switch (direction)
        {
            case GateStrechtDirection.forward:
                return Vector3.forward;

            case GateStrechtDirection.backward:
                return Vector3.back;
            case GateStrechtDirection.left:
                return Vector3.left;
            case GateStrechtDirection.right:
                return Vector3.right;
            default: return Vector3.zero;
        }
    }

}
