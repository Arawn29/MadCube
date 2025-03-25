using System.Collections;
using UnityEngine;

enum Direction
{
    Left,
    Right,
    Up,
    Down,
    None
}
public class Player : MonoBehaviour
{
    Direction direction;

    [Tooltip("Ne kadar süre içerisinde Dönüþünü tamamlasýn.")]
    public float RollingSpeed;
    public Transform PivotTransform;
    public LayerMask DetectionWallsLayer;

    bool isRolling = false;

    private void Start()
    {
        direction = Direction.None;
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            direction = DirectionDetection();

            if (!isRolling)
            {
                StartCoroutine(Roll(direction));
            }

        }
    }
    Direction DirectionDetection()
    {
        float X = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (X != 0)
        {
            return X < 0 ? Direction.Left : Direction.Right;
        }
        else if (y != 0)
        {
            return y < 0 ? Direction.Down : Direction.Up;
        }
        else
        {
            return Direction.None;
        }



    }
    private IEnumerator Roll(Direction direction)
    {
        isRolling = true;
        float degree = 90f; // Rotate Angle

        Vector3 Axis = GetRotateAxis(direction);
        PivotTransform.position = GetPivotOffsetPosition();

        float elapsedTime = 0f;
        // Karakterin Dönme Hareketi
        while (elapsedTime < RollingSpeed)
        {
            elapsedTime += Time.deltaTime;
            transform.RotateAround(PivotTransform.position, Axis, degree * (Time.deltaTime / RollingSpeed));
            yield return null;
        }
        /* #region MyRegion
         transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            Mathf.Round(transform.position.z)

        );

         Vector3 roundedEulerAngles = new Vector3(
      Mathf.Round(transform.eulerAngles.x / 90) * 90,
      Mathf.Round(transform.eulerAngles.y / 90) * 90,
      Mathf.Round(transform.eulerAngles.z / 90) * 90
        );

         transform.rotation = Quaternion.Euler(roundedEulerAngles);

         #endregion
        */
        isRolling = false;


    }


    private Vector3 GetPivotOffsetPosition()
    {
        Vector3 pivotOffset = Vector3.zero;
        Vector3 ColliderSize = transform.GetComponent<BoxCollider>().size / 2f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 500f, DetectionWallsLayer))
        {
            Debug.Log(hit.collider.name); Debug.Log(direction); Debug.Log(GetInputDirection());
            switch (hit.collider.name)
            {
                case "X":
                    if (direction == Direction.Left )
                    {
                        pivotOffset = transform.position+(GetInputDirection() * ColliderSize.y *2f) + (Vector3.down * ColliderSize.x);
                    }
                    else if (direction == Direction.Right)
                    {
                        pivotOffset = transform.position + (GetInputDirection() * ColliderSize.y) + (Vector3.down * ColliderSize.x);
                    }
                    else
                    {
                        pivotOffset = transform.position + (GetInputDirection() * ColliderSize.x) + (Vector3.down * ColliderSize.x);
                    }
                        break;
                //case "-X":
                //    if (direction == Direction.Left || direction == Direction.Right)
                //    {
                //        pivotOffset = transform.position + (GetInputDirection()*ColliderSize.x) +(Vector3.down * ColliderSize.x);
                //    }
                //    else
                //    {

                //    }
                //        break;

                case "Y":
                    pivotOffset = transform.position + (GetInputDirection() * ColliderSize.x);
                    break;
                case "-Y":
                    pivotOffset = transform.position + (GetInputDirection() * ColliderSize.z) + (Vector3.down * ColliderSize.y * 2f);
                    break;
                case "Z":
                    if (direction == Direction.Left || direction == Direction.Right)
                    {
                        pivotOffset = transform.position + (GetInputDirection() * ColliderSize.x) + (Vector3.down * ColliderSize.z);
                    }
                    else if (direction == Direction.Up)
                    {
                        pivotOffset = transform.position + (GetInputDirection() * ColliderSize.y *2f) + (Vector3.down* ColliderSize.x);
                    }
                    else if (direction == Direction.Down)
                    {
                        pivotOffset = transform.position + (GetInputDirection() * ColliderSize.y) + (Vector3.down * ColliderSize.x);
                    }
                        break;

                case "-Z":
                    if (direction == Direction.Left || direction == Direction.Right)
                    {
                        pivotOffset = transform.position + (GetInputDirection() * ColliderSize.x) + (Vector3.down * ColliderSize.z);
                    }
                    else if (direction == Direction.Up)
                    {
                        pivotOffset = transform.position + Vector3.down * ColliderSize.x;
                    }
                    else if (direction == Direction.Down)
                    {
                       pivotOffset = transform.position + (GetInputDirection()* ColliderSize.y * 2f)+ Vector3.down * ColliderSize.z;
                    }
                        break;
            }
        }
        return pivotOffset;
    }

    private Vector3 GetInputDirection()
    {
        switch (direction)
        {
            case Direction.Left:
                return Vector3.left;
            case Direction.Right:
                return Vector3.right;
            case Direction.Up:
                return Vector3.forward;
            case Direction.Down:
                return Vector3.back;
            default: return Vector3.zero;
        }

    }

    //Karakterin dönme ekseni
    Vector3 GetRotateAxis(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:

                return Vector3.forward;

            case Direction.Right:

                return Vector3.back;

            case Direction.Up:
                return Vector3.right;

            case Direction.Down:

                return Vector3.left;
            default:

                return Vector3.zero;
        }
    }

    public void CopyTransformData(Transform Source, Transform Target)
    {
        //Target.localPosition = Source.localPosition;
        Target.localEulerAngles = Source.localEulerAngles;
    }

}
