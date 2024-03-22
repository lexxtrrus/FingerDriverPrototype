using UnityEngine;

public class TrackSegment
{
    public int TriangleCounter;
    public Vector3[] Points;
    public TrackSegment[] Neibrhoods;

    public bool IsPointInSegment(Vector3 point)
    {
        return MathfTriangles.IsPointInTriangleXY(point, Points[0], Points[1], Points[2]);
    }
}