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

    bool isActivated = false;
    int currentPressedButton;
    public void ButtonPressed()
    {
        ++currentPressedButton;
        if (currentPressedButton >= requiredButton && !isActivated)
        {

            Vector3 directionVector = DirectionVector(direction);
            Vector3 offset = LocaleScaleOffset(direction);

            Vector3 newScale = offset + (directionVector * strechtAmount);


            Vector3 positionOffset = (directionVector * strechtAmount) / 2f;

            transform.DOScale(newScale, 1.5f).SetEase(Ease.Linear);
            transform.DOMove(transform.position + positionOffset, 1.5f).SetEase(Ease.Linear);
            isActivated = true;
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
    Vector3 LocaleScaleOffset(GateStrechtDirection direction)
    {
        switch (direction)
        {
            case GateStrechtDirection.forward:
                return new Vector3(1, 0.25f, 0);
            case GateStrechtDirection.backward:
                return new Vector3(1, 0.25f, 0);
            case GateStrechtDirection.left:
                return new Vector3(0, 0.25f,1);
            case GateStrechtDirection.right:
                return new Vector3(0, 0.25f, 1);
            default: return Vector3.zero;
        }
    }

}
