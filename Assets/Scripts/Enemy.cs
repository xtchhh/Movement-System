using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float walkSpeed = 1f;
    private float distanceToPlayer = 15f;
    private Rigidbody rb;
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Exodus");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void distance()
    {
        
    }

}
