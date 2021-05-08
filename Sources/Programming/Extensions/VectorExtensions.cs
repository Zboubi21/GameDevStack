using UnityEngine;

public static class VectorExtensions
{
    /**********
    * Vector2 *
    **********/
    public static Vector2 SetX(this Vector2 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector2 SetY(this Vector2 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector2 GetCenterPosition(this Vector2[] positions)
    {
        Vector2 totalPos = Vector2.zero;
        for (int i = 0, l = positions.Length; i < l; i++)
        {
            totalPos += positions[i];
        }
        return totalPos / positions.Length;
    }

    /**********
    * Vector3 *
    **********/
    public static Vector3 SetX(this Vector3 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector3 SetY(this Vector3 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 SetZ(this Vector3 v, float z)
    {
        v.z = z;
        return v;
    }

    public static Vector3 GetCenterPosition(this Vector3[] positions)
    {
        Vector3 totalPos = Vector3.zero;
        for (int i = 0, l = positions.Length; i < l; i++)
        {
            totalPos += positions[i];
        }
        return totalPos / positions.Length;
    }
}