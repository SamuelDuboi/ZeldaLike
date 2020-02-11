using UnityEngine;


public static class Vector3Extensions
{

  

    public static Vector3 ReplaceX(this Vector3 vector, float x)
    {
        return new Vector3(x, vector.y, vector.z);
    }

    public static Vector3 ReplaceY(this Vector3 vector, float y)
    {
        return new Vector3(vector.x, y, vector.z);
    }

    public static Vector3 ReplaceZ(this Vector3 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }


    

    public static Vector3 addVector(this Vector3 vector, Vector3 value)
    {
        return new Vector3(vector.x + value.x, vector.y + value.y, vector.z + value.z);
    }


 


    public static Vector3 mulVector(this Vector3 a, Vector3 d)
    {
        return new Vector3(a.x * d.x, a.y * d.y, a.z * d.z);
    }

    public static Vector3 AddValueToAllAxis(this Vector3 a, float d)
    {
        return new Vector3(a.x + d, a.y + d, a.z + d);
    }


    public static Vector3 subValueToAllAxis(this Vector3 a, float d)
    {
        return new Vector3(a.x - d, a.y - d, a.z - d);
    }
    


    public static Vector3 divVector(this Vector3 a, Vector3 d)
    {
        return new Vector3(a.x / d.x, a.y / d.y, a.z / d.z);
    }



    public static Vector3 ToGuiCoordinateSystem(this Vector3 a)
    {
        var copy = a;
        copy.y = Screen.height - copy.y;
        return copy;
    }




    public static Vector3 Inverse(this Vector3 a)
    {
        return new Vector3(1 / a.x, 1 / a.y, 1 / a.z);
    }




    public static Vector2 xz(this Vector3 aVector)
    {
        return new Vector2(aVector.x, aVector.z);
    }


    public static Vector2 yz(this Vector3 aVector)
    {
        return new Vector2(aVector.y, aVector.z);
    }



    public static Vector2 zx(this Vector3 aVector)
    {
        return new Vector2(aVector.z, aVector.x);
    }


    public static Vector2 zy(this Vector3 aVector)
    {
        return new Vector2(aVector.z, aVector.y);
    }



    /// <summary>
    /// gets the square distance between two vector3 positions. this is much faster that Vector3.distance.
    /// </summary>
    /// <param name="first">first point</param>
    /// <param name="second">second point</param>
    /// <returns>squared distance</returns>
    public static float SqrDistance(this Vector3 first, Vector3 second)
    {
        return (first.x - second.x) * (first.x - second.x) +
        (first.y - second.y) * (first.y - second.y) +
        (first.z - second.z) * (first.z - second.z);
    }

    /// <summary>
    /// Le point entre les deux
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Vector3 MidPoint(this Vector3 first, Vector3 second)
    {
        return new Vector3((first.x + second.x) * 0.5f, (first.y + second.y) * 0.5f, (first.z + second.z) * 0.5f);
    }

    /// <summary>
    /// get the square distance from a point to a line segment.
    /// </summary>
    /// <param name="point">point to get distance to</param>
    /// <param name="lineP1">line segment start point</param>
    /// <param name="lineP2">line segment end point</param>
    /// <param name="closestPoint">set to either 1, 2, or 4, determining which end the point is closest to (p1, p2, or the middle)</param>
    /// <returns></returns>
    public static float SqrLineDistance(this Vector3 point, Vector3 lineP1, Vector3 lineP2, out int closestPoint)
    {

        Vector3 v = lineP2 - lineP1;
        Vector3 w = point - lineP1;

        float c1 = Vector3.Dot(w, v);

        if (c1 <= 0) //closest point is p1
        {
            closestPoint = 1;
            return SqrDistance(point, lineP1);
        }

        float c2 = Vector3.Dot(v, v);
        if (c2 <= c1) //closest point is p2
        {
            closestPoint = 2;
            return SqrDistance(point, lineP2);
        }


        float b = c1 / c2;

        Vector3 pb = lineP1 + b * v;
        {
            closestPoint = 4;
            return SqrDistance(point, pb);
        }
    }

    /// <summary>
    /// Absolute value of the vector
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector3 Abs(this Vector3 v)
    {
        return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }


    /// <summary>
    /// Gets the normal of the triangle formed by the 3 vectors
    /// </summary>
    /// <param name="vec1"></param>
    /// <param name="vec2"></param>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public static Vector3 Vector3Normal(Vector3 vec1, Vector3 vec2, Vector3 vec3)
    {
        return Vector3.Cross((vec3 - vec1), (vec2 - vec1));
    }


    /// <summary>
    /// centre de plein de vector
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    public static Vector3 Center(this Vector3[] points)
    {
        Vector3 ret = Vector3.zero;
        foreach (var p in points)
        {
            ret += p;
        }
        ret /= points.Length;
        return ret;
    }

    /// <summary>
	/// Calculates the intersection line segment between 2 lines (not segments).
	/// Returns false if no solution can be found.
	/// </summary>
	/// <returns></returns>
	public static bool CalculateLineLineIntersection(Vector3 line1Point1, Vector3 line1Point2,
                                                     Vector3 line2Point1, Vector3 line2Point2, out Vector3 resultSegmentPoint1, out Vector3 resultSegmentPoint2)
    {
        // Algorithm is ported from the C algorithm of 
        // Paul Bourke at http://local.wasp.uwa.edu.au/~pbourke/geometry/lineline3d/
        resultSegmentPoint1 = new Vector3(0, 0, 0);
        resultSegmentPoint2 = new Vector3(0, 0, 0);

        var p1 = line1Point1;
        var p2 = line1Point2;
        var p3 = line2Point1;
        var p4 = line2Point2;
        var p13 = p1 - p3;
        var p43 = p4 - p3;

        if (p4.sqrMagnitude < float.Epsilon)
        {
            return false;
        }
        var p21 = p2 - p1;
        if (p21.sqrMagnitude < float.Epsilon)
        {
            return false;
        }

        var d1343 = p13.x * p43.x + p13.y * p43.y + p13.z * p43.z;
        var d4321 = p43.x * p21.x + p43.y * p21.y + p43.z * p21.z;
        var d1321 = p13.x * p21.x + p13.y * p21.y + p13.z * p21.z;
        var d4343 = p43.x * p43.x + p43.y * p43.y + p43.z * p43.z;
        var d2121 = p21.x * p21.x + p21.y * p21.y + p21.z * p21.z;

        var denom = d2121 * d4343 - d4321 * d4321;
        if (Mathf.Abs(denom) < float.Epsilon)
        {
            return false;
        }
        var numer = d1343 * d4321 - d1321 * d4343;

        var mua = numer / denom;
        var mub = (d1343 + d4321 * (mua)) / d4343;

        resultSegmentPoint1.x = p1.x + mua * p21.x;
        resultSegmentPoint1.y = p1.y + mua * p21.y;
        resultSegmentPoint1.z = p1.z + mua * p21.z;
        resultSegmentPoint2.x = p3.x + mub * p43.x;
        resultSegmentPoint2.y = p3.y + mub * p43.y;
        resultSegmentPoint2.z = p3.z + mub * p43.z;

        return true;
    }


}
