using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionWall : MonoBehaviour
{
    [SerializeField] private GameObject Target;


    private void Update()
    {
        transform.position = Target.transform.position;
    }
}
