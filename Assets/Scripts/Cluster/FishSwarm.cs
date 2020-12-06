using System.Collections.Generic;
using UnityEngine;

public class FishSwarm : MonoBehaviour
{
    public int fishNum;

    public GameObject fishPrefeb;
    public float initialXRange;
    public float initialYRange;
    public float initialZRange;

    public string curTag;
    public string foodTag;
    public string predatorTag;

    public float detectPredatorDistance;
    public float escapeWeight;

    public float detectFoodDistance;
    public float preyWeight;

    public float cohesionWithSwarmWeight;

    public float findFriendDistance;
    public float viewingAngle;

    public float cohesionWeight;

    public float separationDistance;
    public float separationWeight;

    public float alignmentWeight;

    public float randomWeight;

    public float maxSpeed;
    public float maxTurnSpeed;
    public float maxAcceleration;

    public float minTimer;
    public float maxTimer;

    public Transform target;

    public List<Fish> fishes;




    // Start is called before the first frame update
    void Start()
    {
        fishes = new List<Fish>();

        InitialSwarm(true);

        if (target == null)
            target = transform;
    }

    public void InitialSwarm(bool bFirst = false)
    {
        for (int i = 0; i < fishNum; i++)
        {
            if (bFirst)
            {
                Fish f = Instantiate(fishPrefeb, transform.position + new Vector3(Random.Range(-initialXRange, initialXRange),
                                                                     Random.Range(-initialYRange, initialYRange),
                                                                     Random.Range(-initialZRange, initialZRange)),
                        Random.rotation).AddComponent<Fish>();
                f.fishSwarm = this;
                fishes.Add(f);

                f.transform.SetParent(transform);
                f.gameObject.tag = curTag;
            }
            else
            {
                fishes[i].transform.position = transform.position + new Vector3(Random.Range(-initialXRange, initialXRange),
                                                                     Random.Range(-initialYRange, initialYRange),
                                                                     Random.Range(-initialZRange, initialZRange));
                fishes[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

}
