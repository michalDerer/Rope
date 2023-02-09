using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneString : MonoBehaviour
{

    public Transform tr1;
    public Transform tr2;

    public float mas1;
    public float mas2;

    public Vector3 vel1;
    public Vector3 vel2;

    public float l;
    public float stiffness;
    public Vector3 grav;

    public float eng;



    void Start()
    {
    }


    void Update()
    {
        //Step();
        //CalcEnergy();
    }

    private void FixedUpdate()
    {
        Step();
        CalcEnergy();
    }

    void CalcEnergy()
    {
        eng = 0;

        eng += 0.5f * mas1 * vel1.magnitude * vel1.magnitude;
        eng += 0.5f * mas2 * vel2.magnitude * vel2.magnitude;

        eng += mas1 * tr1.position.y * grav.magnitude;
        eng += mas2 * tr2.position.y * grav.magnitude;

        Vector3 dir = tr2.position - tr1.position;
        eng += 0.5f * stiffness * (dir.magnitude - l) * (dir.magnitude - l);
    }

    void Step()
    {
        Vector3 dir = tr2.position - tr1.position;
        Vector3 springF = -stiffness * (dir.magnitude - l) * dir.normalized;

        Vector3 gravF = mas2 * grav;

        Vector3 netF = springF + gravF;

        vel2 += Time.deltaTime * (netF / mas2);

        tr2.position += vel2 * Time.deltaTime;
    }
}
