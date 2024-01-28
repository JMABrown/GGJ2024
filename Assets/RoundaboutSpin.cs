using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundaboutSpin : MonoBehaviour
{
    public Rigidbody rb;

    public float angle = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb == null)
        {
            return;
        }
        
        //this.transform.Rotate(0f, 7.0f, 0f, Space.Self);
        rb.MoveRotation(Quaternion.Euler(0f, angle * Time.timeSinceLevelLoad, 0f));
    }
}
