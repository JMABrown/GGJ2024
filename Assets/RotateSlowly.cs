using UnityEngine;

public class RotateSlowly : MonoBehaviour
{
    public float AngleRotateBy = 0.01f;
    public Vector3 VectorToRotateIn;
    
    void Update()
    {
        this.transform.Rotate(VectorToRotateIn, AngleRotateBy * Time.deltaTime);
    }
}
