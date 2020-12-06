using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleView
{
    public List<Vector3> eyePoints;

    public CircleView(Vector3 eye, float radius, float angle, int pointNum)
    {
        eyePoints = SetViewPoint(eye, radius, angle, pointNum);
    }

    public List<Vector3> SetViewPoint(Vector3 eye, float radius, float angle, int pointNum)
    {
        List<Vector3> points = new List<Vector3>();

        int fullNum = (int)(pointNum / (angle / 360f));
        float phi = 2 * Mathf.PI * 0.618f;
        float x, y, z;
        for(int i = 0; i < fullNum; i++)
        {
            y = (2 * i + 1f) / fullNum - 1;
            x = Mathf.Sqrt(1 - y * y) * Mathf.Cos(phi * i);
            z = Mathf.Sqrt(1 - y * y) * Mathf.Sin(phi * i);
            if(Vector3.Dot(Vector3.forward, new Vector3(x,y,z)) >= Mathf.Cos(angle / 2))
            {
                points.Add(new Vector3(x, y, z));
            }
        }

        if (points.Count != pointNum)
        {
            if (points.Count > pointNum)
            {
                do
                {
                    int rand = Random.Range(0, points.Count);
                    points.RemoveAt(rand);
                } while (points.Count > pointNum);
            }
            else
            {
                float u, v, r2;
                while (points.Count != pointNum)
                {
                    do
                    {
                        u = Random.Range(-1, 1f);
                        v = Random.Range(-1, 1f);
                        r2 = u * u + v * v;
                        x = 2 * u * Mathf.Sqrt(1 - r2);
                        y = 2 * v * Mathf.Sqrt(1 - r2);
                        z = 1 - 2 * r2;
                    } while (u * u + v * v > 1 || Vector3.Dot(Vector3.forward, new Vector3(x, y, z)) < Mathf.Cos(angle / 2));
                    points.Add(new Vector3(x, y, z));
                }
            }
        }

        for (int i = 0; i < points.Count; i++)
        {
            
            x = points[i].x * radius + eye.x;
            y = points[i].y * radius + eye.y;
            z = points[i].z * radius + eye.z;
            points[i] = new Vector3(x, y, z);
        }

        return points;
    }

}
