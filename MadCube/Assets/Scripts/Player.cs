using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    Direction Direction;

    [Tooltip("Ne kadar süre içerisinde Dönüþünü tamamlasýn.")]
    public float RollingSpeed;
    public Transform PivotTransform;
    public LayerMask DetectionWallsLayerMask;

    public LayerMask GroundLayerMask;
    [SerializeField] private GameObject[] sensors;
    [SerializeField] private List<(GameObject obj, bool isTouchingGround)> EdgeSensors = new List<(GameObject obj, bool isTouchingGround)>();
    bool isRolling = false;

    Events myEvents;
    GameManager gameManager;
    private void Awake()
    {
        myEvents = Events.instance;
        gameManager = GameManager.Instance;
        Direction = Direction.None;
        foreach (GameObject obj in sensors)
        {
            EdgeSensors.Add((obj, true));
        }
    }
    private void OnEnable()
    {
        myEvents.OnPlayerFall += PlayerFalling;
    }
    private void OnDisable()
    {
        myEvents.OnPlayerFall -= PlayerFalling;
    }

    private void PlayerFalling()
    {
        myEvents.OnPlayerFall -= PlayerFalling;
        Rigidbody rb = transform.AddComponent<Rigidbody>();
        for (int i = 0; i < EdgeSensors.Count; i++)
        {
            if (EdgeSensors[i].isTouchingGround == false)
            {
                rb.AddForceAtPosition(Vector3.down * 250f, EdgeSensors[i].obj.transform.position, ForceMode.Force);
                break;
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var sensor in EdgeSensors)
        {
            GameObject obj = sensor.obj;
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }

    private void Update()
    {
        if (gameManager.GameState == GameState.Over) return;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Direction = GetInput();

            if (!isRolling)
            {
                StartCoroutine(Roll(Direction));
            }

        }
    }
    Direction GetInput()
    {
        float X = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (X != 0)
        {
            return X < 0 ? Direction.Down : Direction.Up;
        }
        else if (y != 0)
        {
            return y < 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            return Direction.None;
        }



    }
    IEnumerator Roll(Direction direction)
    {
        isRolling = true;
        float degree = 90f; // Rotate Angle
        Vector3 Axis = GetRotationAxis(direction);
        PivotTransform.position = GetPivotOffsetPosition(direction);

        float elapsedTime = 0f;
        // Karakterin Dönme Hareketi
        while (elapsedTime < RollingSpeed)
        {
            elapsedTime += Time.deltaTime;
            transform.RotateAround(PivotTransform.position, Axis, degree * (Time.deltaTime / RollingSpeed));
            yield return null;
        }
        CheckFloor();

        isRolling = false;

    }
    // Döndürme Noktasý Bulma
    private Vector3 GetPivotOffsetPosition(Direction direction)
    {
        Vector3 pivotOffset = Vector3.zero;
        Vector3 ColliderSize = transform.GetComponent<BoxCollider>().size / 2f;
        switch (GetCharacterOrientation())
        {
            case "X":
                if (direction == Direction.Left || direction == Direction.Right)
                {
                    pivotOffset = transform.position + (GetInputDirection() * ColliderSize.y) + (Vector3.down * ColliderSize.x);
                }
                else
                {
                    pivotOffset = transform.position + (GetInputDirection() * ColliderSize.x) + (Vector3.down * ColliderSize.x);
                }
                break;

            case "Y":
                pivotOffset = transform.position + (GetInputDirection() * ColliderSize.x) + Vector3.down * ColliderSize.y;
                break;
            case "Z":
                if (direction == Direction.Left || direction == Direction.Right)
                {
                    pivotOffset = transform.position + (GetInputDirection() * ColliderSize.x) + (Vector3.down * ColliderSize.z);
                }
                else
                {
                    pivotOffset = transform.position + (GetInputDirection() * ColliderSize.y) + (Vector3.down * ColliderSize.x);
                }
                break;
        }
        return pivotOffset;

    }

    // Enumdan Vector Elde Etme
    private Vector3 GetInputDirection()
    {
        switch (Direction)
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
    Vector3 GetRotationAxis(Direction direction)
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

    // Interpolasyon dan sonra oluþan küsüratý yuvarlama iþlemi !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    void RoundTransformValues()
    {
        // Rolll Position
        transform.position = new Vector3(
           Mathf.Round(transform.position.x),
           Mathf.Round(transform.position.y),
           Mathf.Round(transform.position.z)

       );
        //Roll Rotation
        Vector3 roundedEulerAngles = new Vector3(
     Mathf.Round(transform.eulerAngles.x / 90) * 90,
     Mathf.Round(transform.eulerAngles.y / 90) * 90,
     Mathf.Round(transform.eulerAngles.z / 90) * 90
       );

        transform.rotation = Quaternion.Euler(roundedEulerAngles);
    }
    // Eger ki objenin bir parçasý boþluktaysa düþme iþlemi gerçekleþecek
    void CheckFloor()
    {
        RaycastHit hit;
        int nonGroundDetected = 0;

        // Check Ground
        for (int i = 0; i < EdgeSensors.Count; i++)
        {
            GameObject sensor = EdgeSensors[i].obj;
            if (!Physics.Raycast(sensor.transform.position, Vector3.down, out hit, 5f, GroundLayerMask))
            {
                nonGroundDetected++;
                EdgeSensors[i] = (sensor, false);
            }

            else EdgeSensors[i] = (sensor, true);
        }
        // Accept and Run Event
        if (nonGroundDetected > 0)
        {
            Debug.Log("0");
            for (int i = 0; i < EdgeSensors.Count; i++)
            {
                if (Physics.Raycast(EdgeSensors[i].obj.transform.position, Vector3.down, out hit, 10f))
                {
                    Debug.Log("1");
                    // Etkileþime geçebileceði bir þeyin üstünde 
                    if (hit.collider.TryGetComponent<IInteractablePoints>(out var interact))
                    {
                        Debug.Log("2");
                        if (interact.RequiredSensorDetection <= nonGroundDetected)
                        {
                            Debug.Log("3");
                            interact.Interact(this.gameObject);
                        }
                    }

                    // Boslukta
                    else if (hit.collider.GetComponent<IInteractablePoints>() == null)
                    {
                        myEvents.OnPlayerFall?.Invoke();
                    }



                }
            }
        }


    }
    string GetCharacterOrientation()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 500f, DetectionWallsLayerMask))
        {
            return hit.collider.name;
        }
        return null;
    }
}
