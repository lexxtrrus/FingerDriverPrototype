using System.Collections.Generic;
using UnityEngine;

public class RoadCreator : MonoBehaviour
{
    [SerializeField] private Transform _carTrackingPoint;

    [SerializeField] GameObject _checkpoitsHolder;
    [SerializeField] LineRenderer _lineRenderer;
    private TrackSegment[] segments;

    private List<List<Vector3>> _roadPoints;
    private List<GameObject> _checkPoints;
    private float x = 0f;
    private float y = 0f;
    private int i = 0;

    public int currentTriangleNumber;

    void Awake()
    {
        _roadPoints = new List<List<Vector3>>();
        _checkPoints = new List<GameObject>();

        CreateStartSegmentRoad();
        CreateCurveSegmentRoad();
        CreateCurveSegmentRoad();

        SetArrayPositionsOfLineRenderer();
    }


    private void SetArrayPositionsOfLineRenderer()
    {
        List<Vector3> points = new List<Vector3>();

        foreach (List<Vector3> element in _roadPoints)
        {
            if (element != null)
            {
                foreach (Vector3 point in element)
                {
                    points.Add(point);
                }
            }
        }

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());

        //bake mesh
        Mesh mesh = new Mesh();
        _lineRenderer.BakeMesh(mesh, true);

        segments = new TrackSegment[mesh.triangles.Length / 3];
        int triangles = mesh.triangles.Length;

        int segmentCounter = 0;

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            segments[segmentCounter] = new TrackSegment();
            segments[segmentCounter].TriangleCounter = segmentCounter;
            segments[segmentCounter].Points = new Vector3[3];
            segments[segmentCounter].Points[0] = mesh.vertices[mesh.triangles[i]];
            segments[segmentCounter].Points[1] = mesh.vertices[mesh.triangles[i + 1]];
            segments[segmentCounter].Points[2] = mesh.vertices[mesh.triangles[i + 2]];   
            segmentCounter++;
        }


        foreach (var segment in segments)
        {
            segment.Neibrhoods = new TrackSegment[5];
            int neigh = -2; // треугольник должен знать СЕБЯ, двух предыдущих и двух следующих соседей
            for (int n = 0; n < 5; n++)
            {
                if(segment.TriangleCounter >= 3 && segment.TriangleCounter <= segments[segments.Length - 3].TriangleCounter) //заполнение работает от 2-го до пред-пред-последнего
                {
                    segment.Neibrhoods[n] = segments[segment.TriangleCounter + neigh];
                    neigh++;
                }                
            }
        }
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

        var p = 1;

        for (var u = 0.025f; u <= 1; u+= 0.025f)
        {
            p01 = (1 - u) * p0 + u * p1;
            p12 = (1 - u) * p1 + u * p2;
            p23 = (1 - u) * p2 + u * p3;
            p012 = (1 - u) * p01 + u * p12;
            p123 = (1 - u) * p12 + u * p23;
            p0123 = (1 - u) * p012 + u * p123;            

            _roadPoints[^1].Add(p0123);

            if (p % 8 == 0)
            {
                CreateCheckPoint(p0123);
            }
            p++;
        }
    }

    public void CreateStartSegmentRoad()
    {
        _roadPoints.Add(new List<Vector3>());
        for (var u = -8f; u <= 2f; u+=2f)
        {
            _roadPoints[0].Add(new Vector3(0f, u, 0f));
        }

        CreateCheckPoint(_roadPoints[0][0]);
        CreateCheckPoint(_roadPoints[0][3]);
    }

    
    public void WhereIsTheCar(Vector3 point)
    {
        foreach (var segment in segments)
        {
            if (segment.IsPointInSegment(point))
            {
                currentTriangleNumber = segment.TriangleCounter;
                break;
            }
        }
    }


    public bool IsPointInTrack(Vector3 point)
    {
        foreach (var item in segments[currentTriangleNumber].Neibrhoods)
        {
            if (!item.IsPointInSegment(point)) continue;
            currentTriangleNumber = item.TriangleCounter;
            return true;
        }
        
        return false;
    }

    private void CreateCheckPoint(Vector3 point)
    {
        GameObject checkPoint = new GameObject();
        checkPoint.transform.position = point;
        checkPoint.AddComponent<BoxCollider>();
        checkPoint.GetComponent<BoxCollider>().isTrigger = true;
        checkPoint.transform.localScale = new Vector3(0.1f, 0.1f, 2f);
        checkPoint.transform.SetParent(_checkpoitsHolder.transform);
        _checkPoints.Add(checkPoint);
    }

    public void CreateNewTrack()
    {
        CreateCurveSegmentRoad();
         _roadPoints.Remove(_roadPoints[i]); 
        SetArrayPositionsOfLineRenderer();
        WhereIsTheCar(_carTrackingPoint.position);
        _checkPoints.RemoveRange(0, 5);
    }
}