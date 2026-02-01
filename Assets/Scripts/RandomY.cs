using UnityEngine;

public class RandomY : MonoBehaviour
{
    public float Min;
    public float Max;
    public float UpdatePeriod;
    private float _nextUpdateTime = 0f;
    
    void Update()
    {
        if (Time.time > _nextUpdateTime)
        {
            transform.localPosition = new Vector3(transform.localPosition.x,
                Random.Range(Min, Max),
                transform.localPosition.z);

            _nextUpdateTime = Time.time + UpdatePeriod;
        }
    }
}
