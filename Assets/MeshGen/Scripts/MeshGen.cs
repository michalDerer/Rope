using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen : MonoBehaviour
{
    public Transform[] transforms;

    [Min(3)]
    public int noSides = 3;
    [Min(0)]
    public int noSegments = 0;
    [Min(0)]
    public float radius = 1;


    [ContextMenu("Generate1")]
    public void Generate1()
    {

        //float a = 2 * Mathf.PI / noSides;
        float a = 360f / noSides;

        Vector3 pv = transforms[0].position;
        Vector3 p = transforms[0].position;
        Vector3 d = transforms[1].position - transforms[0].position;
        Vector3 d_perpendicular = Vector3.Cross(d, Vector3.one).normalized;


        var meshFilter = GetComponent<MeshFilter>();
        Mesh mesh;
        if (meshFilter.sharedMesh == null)
        {
            mesh = new Mesh();
            meshFilter.sharedMesh = mesh;
        }
        else
        {
            mesh = meshFilter.sharedMesh;
            mesh.Clear();
        }


        List<Vector3> pointss = new List<Vector3>();

        Vector3 pl = p - pv;
        if (noSides > 4)
            pointss.Add(pl);

        for (int i = 0; i < noSides; i++)
        {
            Vector3 pp = pl + (Quaternion.AngleAxis(i * a, d) * d_perpendicular) * radius;
            pointss.Add(pp);
        }


        int[] tria = null;

        if (noSides == 3)
        {
            tria = new int[3];

            tria[0] = 2;
            tria[1] = 1;
            tria[2] = 0;
        }

        if (noSides == 4)
        {
            tria = new int[6];

            tria[0] = 2;
            tria[1] = 1;
            tria[2] = 0;

            tria[3] = 3;
            tria[4] = 2;
            tria[5] = 0;
        }

        if (noSides > 4)
        {
            tria = new int[noSides * 3];

            for (int i = 0; i < noSides - 1; i++)
            {
                tria[i * 3] = 0;
                tria[i * 3 + 1] = i + 2;
                tria[i * 3 + 2] = i + 1;
            }
            tria[(noSides - 1) * 3] = 0;
            tria[(noSides - 1) * 3 + 1] = 1;
            tria[(noSides - 1) * 3 + 2] = (noSides - 1) + 1;
        }

        mesh.SetVertices(pointss);
        mesh.triangles = tria;
        mesh.RecalculateNormals();
    }

    [ContextMenu("Generate2")]
    public void Generate2()
    {
        List<Vector3> points = new List<Vector3>();
        List<int> triangles = new List<int>();


        OuterLoopTrianglesAClc(
            points.Count,
            noSides,
            triangles);

        OuterLoop(
            transforms[0].position - transforms[0].position,
            transforms[1].position - transforms[0].position,
            360f / noSides,
            radius,
            noSides,
            points);


        OuterLoopTrianglesAClc(
            points.Count,
            noSides,
            triangles);

        OuterLoop(
           transforms[1].position - transforms[0].position,
           transforms[0].position - transforms[1].position,
           360f / noSides,
           radius,
           noSides,
           points);


        var meshFilter = GetComponent<MeshFilter>();
        Mesh mesh;
        if (meshFilter.sharedMesh == null)
        {
            mesh = new Mesh();
            meshFilter.sharedMesh = mesh;
        }
        else
        {
            mesh = meshFilter.sharedMesh;
            mesh.Clear();
        }

        mesh.SetVertices(points);
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    public void OuterLoop(Vector3 p, Vector3 d, float a, float r, int noSides, List<Vector3> points)
    {
        Vector3 dp = Vector3.Cross(d, Vector3.up).normalized;

        if (noSides > 4)
            points.Add(p);

        for (int i = 0; i < noSides; i++)
        {
            points.Add(p + (Quaternion.AngleAxis(i * a, d) * dp) * radius);
        }
    }

    public void InnerLoop(Vector3 p, Vector3 d, float a, float r, List<Vector3> points)
    {
        Vector3 dp = Vector3.Cross(d, Vector3.up).normalized;

        for (int i = 0; i < noSides; i++)
        {
            points.Add(p + (Quaternion.AngleAxis(i * a, d) * dp) * radius);
        }
    }

    public void OuterLoopTrianglesClc(int startIdx, int noSides, List<int> triangles)
    {
        if (noSides == 3)
        {
            triangles.Add(startIdx);
            triangles.Add(startIdx + 1);
            triangles.Add(startIdx + 2);
        }

        if (noSides == 4)
        {
            triangles.Add(startIdx);
            triangles.Add(startIdx + 1);
            triangles.Add(startIdx + 2);

            triangles.Add(startIdx);
            triangles.Add(startIdx + 2);
            triangles.Add(startIdx + 3);
        }

        if (noSides > 4)
        {
            for (int i = 0; i < noSides - 1; i++)
            {
                triangles.Add(startIdx);
                triangles.Add(startIdx + i + 1);
                triangles.Add(startIdx + i + 2);
            }

            triangles.Add(startIdx);
            triangles.Add(startIdx + noSides);
            triangles.Add(startIdx + 1);
        }
    }

    public void OuterLoopTrianglesAClc(int startIdx, int noSides, List<int> triangles)
    {
        if (noSides == 3)
        {
            triangles.Add(startIdx);
            triangles.Add(startIdx + 2);
            triangles.Add(startIdx + 1);
        }

        if (noSides == 4)
        {
            triangles.Add(startIdx);
            triangles.Add(startIdx + 2);
            triangles.Add(startIdx + 1);

            triangles.Add(startIdx);
            triangles.Add(startIdx + 3);
            triangles.Add(startIdx + 2);
        }

        if (noSides > 4)
        {
            for (int i = 0; i < noSides - 1; i++)
            {
                triangles.Add(startIdx);
                triangles.Add(startIdx + i + 2);
                triangles.Add(startIdx + i + 1);
            }

            triangles.Add(startIdx);
            triangles.Add(startIdx + 1);
            triangles.Add(startIdx + noSides);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transforms[0].position, 0.05f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transforms[1].position, 0.05f);

        Gizmos.DrawLine(transforms[0].position, transforms[1].position);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            transforms[0].position,
            transforms[0].position + 
            Quaternion.AngleAxis(10, transforms[1].position - transforms[0].position) * 
            (0.2f * Vector3.Cross(transforms[1].position - transforms[0].position, Vector3.up).normalized));

        //Gizmos.DrawLine(
        //   transforms[1].position,
        //   transforms[1].position +
        //   Quaternion.AngleAxis(10, transforms[0].position - transforms[1].position) *
        //   (0.2f * Vector3.Cross(transforms[0].position - transforms[1].position, Vector3.up).normalized));

        Gizmos.DrawLine(
           transforms[0].position,
           transforms[0].position +
          
           (Vector3.Cross(transforms[0].position - transforms[1].position, Vector3.up)));

    }

    [ContextMenu("VertColorsSetFor3SidesRGB")]
    public void VertColorsSetFor3SidesRGB()
    {
        if (noSides == 3)
        {
            Color[] colors = new Color[6];

            colors[0] = UnityEngine.Color.red;
            colors[1] = UnityEngine.Color.green;
            colors[2] = UnityEngine.Color.blue;

            colors[3] = UnityEngine.Color.red;
            colors[4] = UnityEngine.Color.green;
            colors[5] = UnityEngine.Color.blue;

            GetComponent<MeshFilter>().sharedMesh.colors = colors;
        }
    }

    [ContextMenu("VertColorsFlushToGray")]
    public void VertColorsFlushToGray()
    {
        var mesh = GetComponent<MeshFilter>().sharedMesh;

        Color[] colors = new Color[mesh.vertexCount];

        for(int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.gray;
        }

        mesh.colors = colors;
    }
}