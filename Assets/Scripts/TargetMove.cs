using UnityEngine;

public class TargetMove : MonoBehaviour
{
    float timer, t;

    public float xRange, yRange, zRange;

    public float minTimer, maxTimer;

    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(minTimer, maxTimer);
        t = timer;
        targetPos = new Vector3(Random.Range(-xRange, xRange), Random.Range(0, yRange), Random.Range(-zRange, zRange));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, 1 - t/timer);
        t -= Time.deltaTime;
        if(t <= 0)
        {
            timer = Random.Range(minTimer, maxTimer);
            t = timer;
            targetPos = new Vector3(Random.Range(-xRange, xRange), Random.Range(0, yRange), Random.Range(-zRange, zRange));
        }
    }
}
