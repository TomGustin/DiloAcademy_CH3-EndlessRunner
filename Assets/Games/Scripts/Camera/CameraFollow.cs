using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float horizontalOffset;

    private void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = target.position.x + horizontalOffset;
        transform.position = newPosition;
    }
}
