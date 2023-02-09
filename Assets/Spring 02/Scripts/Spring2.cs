using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring2 : MonoBehaviour
{
    public bool[] kinematic;
    public Transform[] transforms;
    public Vector3[] velocities;
    public float[] masses;

    public float stiffness;
    public float length;

    public Vector3 gravity;
    public float energyState;


    private void Update()
    {
        //UpdateSystem();
    }

    private void FixedUpdate()
    {
        UpdateSystem();
    }

    private void UpdateSystem()
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            EvaluateForce(i);
        }

        for (int i = 0; i < transforms.Length; i++)
        {
            UpdatePosition(i);
        }

        energyState = 0f;
        for (int i = 0; i < transforms.Length; i++)
        {
            energyState += 0.5f * masses[i] * velocities[i].magnitude * velocities[i].magnitude;
            energyState += masses[i] * gravity.magnitude * transforms[i].position.y;

            if (i > 0)
            {
                energyState +=
                    0.5f * 
                    stiffness * 
                    ((transforms[i].position - transforms[i - 1].position).magnitude - length) *
                    ((transforms[i].position - transforms[i - 1].position).magnitude - length);
            }
        }
    }

    private void EvaluateForce(int idx)
    {
        if (kinematic[idx])
        {
            return;
        }

        Vector3 forceL = Vector3.zero;
        Vector3 forceR = Vector3.zero;

        int idxL = idx - 1;
        int idxR = idx + 1;

        if (idxL >= 0)
        {
            forceL = CalculateForce(idxL, idx);
        }

        if (idxR < transforms.Length)
        {
            forceR = CalculateForce(idxR, idx);
        }

        Vector3 forceNet = forceL + forceR + gravity * masses[idx];

        velocities[idx] += Time.deltaTime * forceNet / masses[idx];
    }

    private void UpdatePosition(int idx)
    {
        transforms[idx].position += Time.deltaTime * velocities[idx];
    }

    private Vector3 CalculateForce(int idx1, int idx2)
    {
        Vector3 vecIdx1Idx2 = transforms[idx2].position - transforms[idx1].position;
        return -stiffness * (Vector3.Magnitude(vecIdx1Idx2) - length) * Vector3.Normalize(vecIdx1Idx2);
    }
}
