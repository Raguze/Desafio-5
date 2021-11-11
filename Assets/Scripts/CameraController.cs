using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target { get; protected set; }

    private float positionZ = -10f;

    public float Smooth = 0.03f;

    private Transform tf;
    private void Awake()
    {
        tf = GetComponent<Transform>();
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }

    private void LateUpdate()
    {
        Vector3 destination = new Vector3(Target.position.x, Target.position.y, positionZ);
        tf.position = Vector3.Lerp(tf.position, destination, Smooth);
    }
}
