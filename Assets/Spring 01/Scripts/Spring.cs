using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private Transform head;
    public Transform tail;


    public float stifness;
    public float naturalLenght;
    public float mass;


    public Vector3 gravity;


    private Vector3 th_force;
    private Vector3 th_vel;


    // Start is called before the first frame update
    void Start()
    {
        head = transform;
        naturalLenght = Vector3.Distance(head.position, tail.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Code();
    }

    private void FixedUpdate()
    {
        Code();
    }

    void Code()
    {
        Vector3 th = tail.position - head.position;
        Vector3 th_norm = Vector3.Normalize(th);
        float th_stretch = Vector3.Magnitude(th) - naturalLenght;

        th_force = -stifness * th_stretch * th_norm;
        Vector3 gr_force = gravity * mass;

        th_vel += ((th_force + gr_force) / mass) * Time.deltaTime;

        tail.position = tail.position + th_vel * Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(tail.position, tail.position + th_force);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + (tail.position - transform.position).normalized * naturalLenght, 0.06f);
    }

}
