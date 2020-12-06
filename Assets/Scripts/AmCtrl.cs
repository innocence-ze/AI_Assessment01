using UnityEngine;

public class AmCtrl : MonoBehaviour
{
    public float maxAmSpeed;
    public Rigidbody rb;
    public Animator am;
    Fish thisFish;
    // Use this for initialization
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        am = GetComponentInChildren<Animator>();
        thisFish = GetComponent<Fish>();
    }

    // Update is called once per frame
    void Update()
    {
        am.speed = rb.velocity.magnitude / thisFish.fishSwarm.maxSpeed * maxAmSpeed;
    }
}
