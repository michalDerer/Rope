using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen : MonoBehaviour
{
    public Transform[] transforms;

    [Min(3)]
    public int noSides = 3;
    [Min(1)]
    public int noSegments = 1;
    [Min(0)]
    public float radius = 1;


    [ContextMenu("Generate1")]
    public void Generate1()
    {

    }

    [ContextMenu("Generate2")]
    public void Generate2()
    {
        List<Vector3> points = new List<Vector3>();
        List<int> triangles = new List<int>();

        float angle = 360f / noSides;


        OuterLoopTrianglesAClc(
            points.Count,
            noSides,
            triangles);

        OuterLoopVertices(
            transforms[0].position - transforms[0].position,
            transforms[1].position - transforms[0].position,
            angle,
            radius,
            noSides,
            points);


        if (noSegments == 1)
        {
            InnerLoopTriangles(noSides < 5 ? points.Count - noSides : points.Count - noSides - 1, noSides, triangles);
        }
        else
        {
            for (float i = 1; i < noSegments; i++)
            {
                InnerLoopTriangles(points.Count - noSides, noSides, triangles);

                float t = i / noSegments;
                Vector3 p = Vector3.Lerp(transforms[0].position, transforms[1].position, t);
                InnerLoopVertices(
                    p - transforms[0].position,
                    transforms[1].position - p,
                    angle,
                    radius,
                    points);
            }

            InnerLoopTriangles(noSides < 5 ? points.Count - noSides : points.Count - noSides - 1, noSides, triangles);
        }


        OuterLoopTrianglesClc(
            points.Count,
            noSides,
            triangles);

        OuterLoopVertices(
           transforms[1].position - transforms[0].position,
           transforms[1].position - transforms[0].position,
           angle,
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

        mesh.uv = CalculateUVs(noSides, points.Count);
    }

    public void InnerLoopVertices(Vector3 p, Vector3 d, float a, float r, List<Vector3> points)
    {
        Vector3 dp = Vector3.Cross(d, Vector3.up).normalized;

        for (int i = 0; i < noSides; i++)
        {
            points.Add(p + (Quaternion.AngleAxis(i * a, d) * dp) * r);
        }
    }

    public void OuterLoopVertices(Vector3 p, Vector3 d, float a, float r, int noSides, List<Vector3> points)
    {
        Vector3 dp = Vector3.Cross(d, Vector3.up).normalized;

        if (noSides > 4)
            points.Add(p);

        for (int i = 0; i < noSides; i++)
        {
            points.Add(p + (Quaternion.AngleAxis(i * a, d) * dp) * r);
        }
    }

    public void InnerLoopTriangles(int startIdx, int noSides, List<int> triangles)
    {
        for (int i = 0; i < noSides - 1; i++)
        {
            triangles.Add(startIdx + i);
            triangles.Add(startIdx + i + noSides + 1);
            triangles.Add(startIdx + i + noSides);

            triangles.Add(startIdx + i);
            triangles.Add(startIdx + i + 1);
            triangles.Add(startIdx + i + noSides + 1);
        }

        int hlpr = startIdx + noSides - 1;

        triangles.Add(hlpr);
        triangles.Add(startIdx + noSides);
        triangles.Add(hlpr + noSides);

        triangles.Add(hlpr);
        triangles.Add(startIdx);
        triangles.Add(startIdx + noSides);
    }

    public void OuterLoopTrianglesClc(int startIdx, int noSides, List<int> triangles)
    {
        if (noSides == 3)
        {
            triangles.Add(startIdx);
            triangles.Add(startIdx + 1);
            triangles.Add(startIdx + 2);

            return;
        }

        if (noSides == 4)
        {
            triangles.Add(startIdx);
            triangles.Add(startIdx + 1);
            triangles.Add(startIdx + 2);

            triangles.Add(startIdx);
            triangles.Add(startIdx + 2);
            triangles.Add(startIdx + 3);

            return;
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

            return;
        }
    }

    public void OuterLoopTrianglesAClc(int startIdx, int noSides, List<int> triangles)
    {
        if (noSides == 3)
        {
            triangles.Add(startIdx);
            triangles.Add(startIdx + 2);
            triangles.Add(startIdx + 1);

            return;
        }

        if (noSides == 4)
        {
            triangles.Add(startIdx);
            triangles.Add(startIdx + 2);
            triangles.Add(startIdx + 1);

            triangles.Add(startIdx);
            triangles.Add(startIdx + 3);
            triangles.Add(startIdx + 2);

            return;
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

            return;
        }
    }

    public Vector2[] CalculateUVs(int noSides, int vertexCount)
    {
        Vector2[] uvs = new Vector2[vertexCount];

        int i = noSides < 5 ? noSides : noSides + 1;
        int iEnd = noSides < 5 ? vertexCount - noSides : vertexCount - noSides - 1;

        for (; i < iEnd; i++)
        {
            //uvs[i].x = i / noSides;
            uvs[i].x = 1;
            uvs[i].y = (float)(i % noSides + 1) / (noSides + 0);
            Debug.Log(uvs[i].y);
        }

        return uvs;
    }

    [ContextMenu("Generate3")]
    public void Generate3()
    {
        List<Vector3> points = new List<Vector3>();
        List<int> triangles = new List<int>();

        float angle = 360f / noSides;


        CreateSegmentVertices(
            transforms[0].position - transforms[0].position,
            transforms[1].position - transforms[0].position,
            angle,
            radius,
            noSides,
            points);

        for (float i = 1; i <= noSegments; i++)
        {
            CreateSegmentTriangles(points.Count - noSides - 1, noSides, triangles);

            Vector3 p = Vector3.Lerp(transforms[0].position, transforms[1].position, i / noSegments);

            CreateSegmentVertices(
                p - transforms[0].position,
                //transforms[1].position - p, //sposobi zaspicatenie
                Vector3.forward, //takto funguje spravne
                angle,
                radius,
                noSides,
                points);
        }


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

        mesh.uv = CreateUVs(noSides, noSegments, points.Count);
    }

    public void CreateSegmentVertices(Vector3 p, Vector3 d, float a, float r, int sides, List<Vector3> points)
    {
        int startIdx = points.Count;

        Vector3 dp = Vector3.Cross(d, Vector3.up).normalized;

        for (int i = 0; i < sides; i++)
        {
            points.Add(p + (Quaternion.AngleAxis(i * a, d) * dp) * r);
        }

        points.Add(points[startIdx]);
    }

    public void CreateSegmentTriangles(int startIdx, int sides, List<int> triangles)
    {
        for (int i = 0; i < sides; i++)
        {
            triangles.Add(startIdx + i);
            triangles.Add(startIdx + i + sides + 1 + 1);
            triangles.Add(startIdx + i + sides + 1);

            triangles.Add(startIdx + i);
            triangles.Add(startIdx + i + 1);
            triangles.Add(startIdx + i + sides + 1 + 1);
        }
    }

    public Vector2[] CreateUVs(int sides, int segments, int vertexCount)
    {
        Vector2[] uvs = new Vector2[vertexCount];

        //trochu optimalizace s tvoriaci garbage
        float[] uvYs = new float[sides + 1];
        for (int i = 0; i <= sides; i++)
        {
            uvYs[i] = (float)i / sides;
        }

        for (int edge = 0; edge <= segments; edge++)
        {
            int edgeVertRoot = edge * (sides + 1);
            float uvX = (float)edge / segments;

            for (int edgeVert = 0; edgeVert <= sides; edgeVert++)
            {
                uvs[edgeVertRoot + edgeVert].x = uvX;

                //uvs[edgeVertRoot + edgeVert].y = (float)edgeVert / sides;
                //trochu optimalizace s tvoriaci garbage
                uvs[edgeVertRoot + edgeVert].y = uvYs[edgeVert];
            }
        }

        return uvs;
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

    [ContextMenu("VertColorsSetRGBRepeat")]
    public void VertColorsSetFor3SidesRGB()
    {
        var mesh = GetComponent<MeshFilter>().sharedMesh;
        Color[] colors = new Color[mesh.vertexCount];

        int i = 0;
        int j = 0;

        Color[] pallete = new Color[] { Color.red, Color.green, Color.blue };

        if (noSides < 5)
        {
            while (i < colors.Length)
            {
                colors[i] = pallete[j];
                i++;
                j++;

                if (j == pallete.Length)
                {
                    j = 0;
                }
            }
        }
        else
        {
            colors[i] = Color.red;
            i++;

            while (i + 1 < colors.Length)
            {
                colors[i] = pallete[j];
                i++;
                j++;

                if (j == pallete.Length)
                {
                    j = 0;
                }
            }

            colors[i] = Color.red;
        }


        mesh.colors = colors;
    }

    [ContextMenu("VertColorsFlushToGray")]
    public void VertColorsFlushToGray()
    {
        var mesh = GetComponent<MeshFilter>().sharedMesh;

        Color[] colors = new Color[mesh.vertexCount];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.gray;
        }

        mesh.colors = colors;
    }
}