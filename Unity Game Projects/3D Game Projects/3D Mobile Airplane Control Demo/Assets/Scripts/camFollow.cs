using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour
{
    public Transform plane;
    public Vector3 camOffset;

    // Update is called once per frame
    void Update()
    {
        transform.position = plane.position + camOffset;
    }
}
