using UnityEngine;

public class RotateSlowly : MonoBehaviour
{
    public float AngleRotateBy = 0.01f;
    
    void Update()
    {
        this.transform.Rotate(Vector3.up, AngleRotateBy * Time.deltaTime);
    }
}
