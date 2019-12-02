using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCreator : MonoBehaviour
{
    private class TrackSegment
    {
        public Vector3[] points;
        public bool IsPointInSegment(Vector3 point)
        {
            return MathfTriangles.IsPointInTriangleXY(point, points[0], points[1], points[2]);
        }
    }

    [SerializeField] LineRenderer _lineRenderer;
    private TrackSegment[] segments;

    private List<List<Vector3>> _roadPoints;
    private float x = 0f;
    private float y = 0f;

    void Start()
    {
        _roadPoints = new List<List<Vector3>>();

        CreateStartSegmentRoad();
        CreateCurveSegmentRoad();
        CreateCurveSegmentRoad();

        SetArrayPositionsOfLineRenderer();

        //bake mesh
        Mesh mesh = new Mesh();
        _lineRenderer.BakeMesh(mesh, true);

        segments = new TrackSegment[mesh.triangles.Length / 3];
        int segmentCounter = 0;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            segments[segmentCounter] = new TrackSegment();
            segments[segmentCounter].points = new Vector3[3];
            segments[segmentCounter].points[0] = mesh.vertices[mesh.triangles[i]];
            segments[segmentCounter].points[1] = mesh.vertices[mesh.triangles[i + 1]];
            segments[segmentCounter].points[2] = mesh.vertices[mesh.triangles[i + 2]];

            segmentCounter++;
        }        
    }

    private void SetArrayPositionsOfLineRenderer()
    {
        List<Vector3> points = new List<Vector3>();

        foreach(List<Vector3> element in _roadPoints)
        {
            foreach(Vector3 point in element)
            {
                points.Add(point);
            }
        }

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
    }

    private void CreateCurveSegmentRoad()
    {
        Vector3 p0, p1, p2, p3;

        int indexOfLastElement = _roadPoints.Count - 1;
        p0 = _roadPoints[indexOfLastElement][_roadPoints[indexOfLastElement].Count - 1];

        p1 = new Vector3(p0.x + x, p0.y + 10f + y, 0f);

        float randomX = 10;
        if (UnityEngine.Random.Range(1, 11) % 2 == 0) randomX *= -1;


        p2 = new Vector3(p0.x + randomX , p0.y + UnityEngine.Random.Range(-2, 3), 0f);

        x = UnityEngine.Random.Range(-3, 4);
        y = UnityEngine.Random.Range(-2, 3);

        p3 = new Vector3(p2.x + x, p2.y + 10f + y, 0f);

        _roadPoints.Add(new List<Vector3>());

        Vector3 p01, p12, p23, p012, p123, p0123;

        for (float u = 0.02f; u <= 1; u+= 0.02f)
        {
            p01 = (1 - u) * p0 + u * p1;
            p12 = (1 - u) * p1 + u * p2;
            p23 = (1 - u) * p2 + u * p3;
            p012 = (1 - u) * p01 + u * p12;
            p123 = (1 - u) * p12 + u * p23;
            p0123 = (1 - u) * p012 + u * p123;

            _roadPoints[_roadPoints.Count - 1].Add(p0123);
        }
    }

    public void CreateStartSegmentRoad()
    {
        _roadPoints.Add(new List<Vector3>());
        for (float u = -4f; u < 2f; u+=2f)
        {
            _roadPoints[0].Add(new Vector3(0f, u, 0f));
        }
    }

    public bool IsPointInTrack(Vector3 point)
    {
        foreach (var segment in segments)
        {
            if (segment.IsPointInSegment(point))
            {
                return true;
            }
        }
        return false;
    }
}