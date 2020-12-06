using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public FishSwarm fishSwarm;
    private Rigidbody rb;
    private float timer;
    private float t;
    private Vector3 curVelocity;
    private Vector3 randomVelocity;

    private List<GameObject> foods;
    private List<GameObject> predators;
    public List<Fish> friends;

    // Start is called before the first frame update
    void Start()
    {
        friends = new List<Fish>();


        if (string.IsNullOrEmpty(fishSwarm.foodTag))
            foods = new List<GameObject>();
        else
            foods = new List<GameObject>(GameObject.FindGameObjectsWithTag(fishSwarm.foodTag));

        if (string.IsNullOrEmpty(fishSwarm.predatorTag))
            predators = new List<GameObject>();
        else
            predators = new List<GameObject>(GameObject.FindGameObjectsWithTag(fishSwarm.predatorTag));


        rb = gameObject.GetComponent<Rigidbody>();
        timer = 0;
        t = timer;

        //var c = gameObject.AddComponent<SphereCollider>();
        //c.isTrigger = true;
        //c.radius = fishSwarm.findFriendDistance;
        //c.center = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        FindFriends();
        ComputeVelocity();
        rb.velocity = Vector3.RotateTowards(rb.velocity, curVelocity, fishSwarm.maxTurnSpeed * Time.deltaTime, fishSwarm.maxAcceleration * Time.deltaTime);
        DrawLine(rb.velocity, Color.green);
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    void FindFriends()
    {
        friends.Clear();
        for (int i = 0; i < fishSwarm.fishNum; i++)
        {
            if (fishSwarm.fishes[i] != this && Vector3.SqrMagnitude(fishSwarm.fishes[i].transform.position - transform.position) < fishSwarm.findFriendDistance * fishSwarm.findFriendDistance)
            {
                Vector3 dir = fishSwarm.fishes[i].transform.position - transform.position;
                if (Vector3.Angle(dir, transform.forward) < fishSwarm.viewingAngle / 2)
                {
                    friends.Add(fishSwarm.fishes[i]);
                }

            }
        }
    }

    void ComputeVelocity()
    {
        RandomVelocity();
        Vector3 weightedVelocity = SeparationVelocity(friends) + AlignmentVelocity(friends) + CohesionVelocity(friends)
                                  + PreyVelocity() + EscapeVelocity() + CohesionWithSwarmVelocity() + randomVelocity;

        weightedVelocity /= (7 * fishSwarm.maxSpeed);
        if (weightedVelocity.magnitude > fishSwarm.maxSpeed)
            weightedVelocity = Vector3.ClampMagnitude(weightedVelocity, fishSwarm.maxSpeed);
        curVelocity = weightedVelocity;
    }

    /// <summary>
    /// stay far away from nearest fish
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    Vector3 SeparationVelocity(List<Fish> f)
    {
        Vector3 tempVelocity = Vector3.zero;
        if (f.Count == 0)
            return tempVelocity;
        float dis = float.MaxValue;
        Fish tF = null;
        for (int i = 0; i < f.Count; i++)
        {
            float d = Vector3.Distance(transform.position, f[i].transform.position);
            if (d < dis)
            {
                dis = d;
                tF = f[i];
            }
        }
        if (tF != null)
        {
            tempVelocity = (tF.transform.position - transform.position).normalized * Mathf.Clamp((dis - fishSwarm.separationDistance) / fishSwarm.separationDistance, -1f, 1f) * fishSwarm.separationWeight;
        }
        return tempVelocity;
    }

    /// <summary>
    /// let fish swim towards almost same
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    Vector3 AlignmentVelocity(List<Fish> f)
    {
        Vector3 tempVelocity = Vector3.zero;
        if (f.Count == 0)
            return tempVelocity;
        for (int i = 0; i < f.Count; i++)
        {
            tempVelocity += f[i].rb.velocity;
        }
        tempVelocity /= f.Count;
        return tempVelocity.normalized * fishSwarm.alignmentWeight;
    }

    /// <summary>
    /// bond with surrounding fish
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    Vector3 CohesionVelocity(List<Fish> f)
    {
        Vector3 center = Vector3.zero;
        if (f.Count == 0)
            return center;
        for (int i = 0; i < f.Count; i++)
        {
            center += f[i].transform.position;
        }
        center /= f.Count;
        var temp = (center - transform.position).normalized * fishSwarm.cohesionWeight;
        return temp;
    }

    /// <summary>
    /// each fish swim randomly
    /// </summary>
    void RandomVelocity()
    {
        t -= Time.deltaTime;
        if (t <= 0)
        {
            t = timer;
            timer = Random.Range(fishSwarm.minTimer, fishSwarm.maxTimer);
            randomVelocity = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), Random.Range(-1, 1f)).normalized * fishSwarm.randomWeight;
        }
    }

    /// <summary>
    /// bond with whole fish swarm
    /// </summary>
    /// <returns></returns>
    Vector3 CohesionWithSwarmVelocity()
    {
        var temp = (fishSwarm.target.position - transform.position).normalized * fishSwarm.cohesionWithSwarmWeight;
        return temp;
    }

    Vector3 EscapeVelocity()
    {
        Vector3 tempVelocity = Vector3.zero;
        if (predators.Count == 0)
            return tempVelocity;
        GameObject p = null;
        float dis = float.MaxValue;
        for (int i = 0; i < predators.Count; i++)
        {
            float d = Vector3.Distance(predators[i].transform.position, transform.position);
            if (d < dis)
            {
                dis = d;
                p = predators[i];
            }
        }
        if (p != null)
        {
            float nearDis = Mathf.Clamp((fishSwarm.detectPredatorDistance - dis) / fishSwarm.detectPredatorDistance, 0f, 1f) * 2;
            tempVelocity = nearDis * nearDis * (transform.position - p.transform.position).normalized * fishSwarm.escapeWeight;
        }
        DrawLine(tempVelocity / fishSwarm.escapeWeight, Color.yellow);
        return tempVelocity;
    }

    Vector3 PreyVelocity()
    {
        Vector3 tempVelocity = Vector3.zero;
        if (foods.Count == 0)
            return Vector3.zero;
        GameObject f = null;
        float dis = float.MaxValue;
        for (int i = 0; i < foods.Count; i++)
        {
            float d = Vector3.Distance(foods[i].transform.position, transform.position);
            if (d < dis)
            {
                dis = d;
                f = foods[i];
            }
        }
        if (f != null)
        {
            float nearDis = Mathf.Clamp((fishSwarm.detectFoodDistance - dis) / fishSwarm.detectFoodDistance, 0f, 1f) * 2;
            tempVelocity = nearDis * nearDis * (f.transform.position - transform.position).normalized * fishSwarm.preyWeight;
        }
        DrawLine(tempVelocity / fishSwarm.preyWeight, Color.red);
        return tempVelocity;
    }

    void ResetPos()
    {
        transform.position = new Vector3(Random.Range(-fishSwarm.initialXRange, fishSwarm.initialXRange),
                                         Random.Range(-fishSwarm.initialYRange, fishSwarm.initialYRange),
                                         Random.Range(-fishSwarm.initialZRange, fishSwarm.initialZRange));
        rb.velocity = Vector3.zero;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(fishSwarm.predatorTag) && other.CompareTag(fishSwarm.predatorTag))
        {
            ResetPos();
            GameManager.Singleton.AddScore(1);
        }
    }

    void DrawLine(Vector3 velocity, Color c)
    {
        Debug.DrawLine(transform.position, transform.position + velocity, c);
    }

}
