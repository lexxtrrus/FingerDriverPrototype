using UnityEngine;

public static class MathfTriangles
{
    public static bool IsPointInTriangleXY(Vector3 point, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return IsInside(point, p1, p2, p3);
    }

    private static bool IsInside(Vector3 point, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float l1 = CrossProduct(point - p1, p3 - p1),
              l2 = CrossProduct(point - p2, p1 - p2),
              l3 = CrossProduct(point - p3, p2 - p3);
        return (l1 > 0 && l2 > 0 && l3 > 0) || (l1 < 0 && l2 < 0 && l3 < 0);
    }

    private static float CrossProduct(Vector3 v1, Vector3 v2)
    {
        return v1.x * v2.y - v1.y * v2.x;
    }
}