using UnityEngine;

public class CrazyWiggle : MonoBehaviour
{
    public float Min;
    public float Max;
    public float UpdatePeriod;
    private float _nextUpdateTime = 0f;
    
    void Update()
    {
        if (Time.time > _nextUpdateTime)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(Min, Max));

            _nextUpdateTime = Time.time + UpdatePeriod;
        }
    }
}
