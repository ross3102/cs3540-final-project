using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScript : MonoBehaviour
{
    public float speed = 5;
    public float height = 1;

    Vector3 initialPos;

    void Start()
    {
        initialPos = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(initialPos, initialPos + Vector3.up * height, Mathf.PingPong(Time.time * speed, 1));
    }
}
