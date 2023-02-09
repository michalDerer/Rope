using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring03 : MonoBehaviour
{
    public Transform t0, t1, t2;
    public Vector3 v1, v2;
    public float m1, m2;
    public float l01, l12;

    public float tension;
    public Vector3 gravity;

    public float energy;

    // Start is called before the first frame update
    void Start()
    {
        l01 = Vector3.Distance(t0.position, t1.position);
        l12 = Vector3.Distance(t1.position, t2.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Step();
    }

    private void FixedUpdate()
    {
        Step();
    }

    void Step()
    {
        energy = 0;
        energy += m1 * t1.position.y * gravity.magnitude;
        energy += m2 * t2.position.y * gravity.magnitude;

        energy += 0.5f * m1 * Mathf.Pow(v1.magnitude, 2);
        energy += 0.5f * m2 * Mathf.Pow(v2.magnitude, 2);

        energy += 0.5f * tension * Mathf.Pow(Vector3.Distance(t0.position, t1.position) - l01, 2);
        energy += 0.5f * tension * Mathf.Pow(Vector3.Distance(t1.position, t2.position) - l12, 2);


        Vector3 vec01 = t1.position - t0.position;
        Vector3 springForce1 = - tension * (vec01.magnitude - l01) * vec01.normalized;


        Vector3 vec12 = t2.position - t1.position;
        Vector3 springForce2 = -tension * (vec12.magnitude - l12) * vec12.normalized;

        Vector3 netForce1 = springForce1 - springForce2 + gravity * m1;
        Vector3 netForce2 = springForce2 + gravity * m2;

        v1 += (netForce1 / m1) * Time.deltaTime;
        v2 += (netForce2 / m2) * Time.deltaTime;

        t1.position += v1 * Time.deltaTime;
        t2.position += v2 * Time.deltaTime;

    }
}
