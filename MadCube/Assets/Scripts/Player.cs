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
    [SerializeField] private List<EdgeSensor> EdgeSensors = new();
    public bool IsRolling = false;
    MainEvents myEvents;
    GameManager gameManager;
    Coroutine rollCoroutine;
    private void Awake()
    {
        myEvents = MainEvents.Instance;
        gameManager = GameManager.Instance;
        Direction = Direction.None;
    }
    private void Start()
    {
        myEvents.OnPlayerFall += PlayerFalling;
        myEvents.OnPlayerRolled += CheckFloor;
        myEvents.OnPlayerRolled += RoundTransformValues;
    }
    private void OnDisable()
    {
        myEvents.OnPlayerFall -= PlayerFalling;
        myEvents.OnPlayerRolled -= CheckFloor;
        myEvents.OnPlayerRolled -= RoundTransformValues;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var sensor in EdgeSensors)
        {

            Gizmos.DrawSphere(sensor.obj.transform.position, 0.2f);
        }
    }

    private void Update()
    {
        if (gameManager.GameState != GameState.Playable) return;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Direction = GetInput();

            if (!IsRolling)
            {
                rollCoroutine = StartCoroutine(Roll(Direction));
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
        IsRolling = true;
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
        // Dönme iþlemi tamamlandýktan sonra
        myEvents.OnPlayerRolled?.Invoke();
        yield return null;
        IsRolling = false;



    }
    void StopRoll()
    {
        IsRolling = false;
        if (rollCoroutine != null)
        {
            StopCoroutine(rollCoroutine);
        }
    }
    private void PlayerFalling()
    {
        StopRoll();
        Rigidbody rb = transform.GetComponent<Rigidbody>();
        if (rb == null) { rb = transform.AddComponent<Rigidbody>(); }
        for (int i = 0; i < EdgeSensors.Count; i++)
        {
            if (!EdgeSensors[i].GetSensor())
            {
                rb.AddForceAtPosition(Vector3.down * 250f, EdgeSensors[i].obj.transform.position, ForceMode.Force);
                break;
            }
        }
    }

    // Döndürme Noktasý Bulma
    private Vector3 GetPivotOffsetPosition(Direction direction)
    {
        Vector3 pivotOffset = Vector3.zero;
        Vector3 ColliderSize = transform.GetComponent<BoxCollider>().size / 2f;
        switch (GetPlayerOrientation())
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

    // TODO:  !!!!!Interpolasyon dan sonra oluþan küsüratý yuvarlama iþlemi !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  
    void RoundTransformValues()
    {

        // Rotasyonu 90 derece katlarýna yuvarla    
        Vector3 roundedEulerAngles = new Vector3(
            Mathf.Round(transform.eulerAngles.x / 90f) * 90f,
            Mathf.Round(transform.eulerAngles.y / 90f) * 90f,
            Mathf.Round(transform.eulerAngles.z / 90f) * 90f
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
            if (!Physics.Raycast(EdgeSensors[i].obj.transform.position, Vector3.down, 20f, gameManager.GroundLayerMask))
            {
                nonGroundDetected++;
                EdgeSensors[i].ResetSensor();
                Debug.Log(EdgeSensors[i].obj.name);
            }

            else
            {
                EdgeSensors[i].SetSensor();
            }
        }
        // Accept and Run Event
        if (nonGroundDetected > 0)
        {
            foreach (var sensor in EdgeSensors)
            {
                if (!sensor.GetSensor())
                {
                    if (Physics.Raycast(sensor.obj.transform.position, Vector3.down, out hit,20f))
                    {
                        // Etkileþime geçebileceði bir þeyin üstünde 
                        if (hit.collider.TryGetComponent<IPlayerInteractablePoints>(out var interact))
                        {
                            if (interact.RequiredSensorDetection <= nonGroundDetected)
                            {
                                interact.Interact(gameObject);
                            }
                        }
                        // Boslukta
                        else if (hit.collider.GetComponent<IPlayerInteractablePoints>() == null)
                        {
                            Debug.Log($"{sensor.obj.name} göremedi ve düþüyor");
                            myEvents.OnPlayerFall?.Invoke();
                        }
                    }
                }
            }
        }


    }
    public string GetPlayerOrientation()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 500f, gameManager.DetectionWallsLayerMask))
        {
            return hit.collider.name;
        }
        return null;
    }
}
[System.Serializable]
public class EdgeSensor
{
    public GameObject obj;
    [HideInInspector]
    public bool isTouchingGround = false;

    public void SetSensor()
    {
        isTouchingGround = true;
    }
    public void ResetSensor()
    {
        isTouchingGround = false;
    }
    public bool GetSensor()
    {
        return isTouchingGround;
    }
}

