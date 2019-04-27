using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector3 Velocity;
    public float RotationSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position += Velocity * Time.deltaTime;
        this.transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);
    }
}
