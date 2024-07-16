using System.Collections.Generic;
using UnityEngine;

public class Camera_Rail : MonoBehaviour
{
	public Transform target;

	public Transform[] railControlPoints;

	[SerializeField] private float lerpValue = 10f;
    [SerializeField] private int segments = 10;

    private Vector3 direction;
    private List<Vector3> interpolatedPoints;

    private void Start()
    {
        interpolatedPoints = new List<Vector3>();

        if (railControlPoints.Length >= 4)
        {
            for (int i = 0; i < railControlPoints.Length - 3; i++)
            {
                Vector3 p0 = railControlPoints[i].position;
                Vector3 p1 = railControlPoints[i + 1].position;
                Vector3 p2 = railControlPoints[i + 2].position;
                Vector3 p3 = railControlPoints[i + 3].position;

                Vector3[] newPoints = Interpolate.GetCatmullRomPoints(p0, p1, p2, p3, segments);

                for (int j = 0; j < newPoints.Length; j++)
                    interpolatedPoints.Add(newPoints[j]);
            }
        }
        else
        {
            Debug.LogError("You need at least 4 control points for interpolation!");

            for (int i = 0; i < railControlPoints.Length; i++)
                interpolatedPoints.Add(railControlPoints[i].position);
        }

    }

    void Update()
	{
		direction = target.position - Camera.main.transform.position;

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, GetNearestRailPoint(), lerpValue * Time.deltaTime);
        Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(direction), lerpValue * Time.deltaTime);
    }

	private Vector3 GetNearestRailPoint()
    {
        float smallestDistance = Mathf.Infinity;
        Vector3 nearestPoint = Vector3.zero;

        foreach (Vector3 point in interpolatedPoints)
        {
            float distance = (target.position - point).magnitude;
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                nearestPoint = point;
            }

        }

        return nearestPoint;
    }

    public class Interpolate : MonoBehaviour
    {
        private static float alpha = 0.5f;

        public static Vector3[] GetCatmullRomPoints(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float segments)
        {
            List<Vector3> catmullRomPoints = new List<Vector3>();

            float t0 = 0.0f;
            float t1 = GetT(t0, p0, p1);
            float t2 = GetT(t1, p1, p2);
            float t3 = GetT(t2, p2, p3);

            for (float t = t1; t < t2; t += (t2 - t1) / segments)
            {
                Vector3 A1 = (t1 - t) / (t1 - t0) * p0 + (t - t0) / (t1 - t0) * p1;
                Vector3 A2 = (t2 - t) / (t2 - t1) * p1 + (t - t1) / (t2 - t1) * p2;
                Vector3 A3 = (t3 - t) / (t3 - t2) * p2 + (t - t2) / (t3 - t2) * p3;

                Vector3 B1 = (t2 - t) / (t2 - t0) * A1 + (t - t0) / (t2 - t0) * A2;
                Vector3 B2 = (t3 - t) / (t3 - t1) * A2 + (t - t1) / (t3 - t1) * A3;

                Vector3 C = (t2 - t) / (t2 - t1) * B1 + (t - t1) / (t2 - t1) * B2;

                catmullRomPoints.Add(C);
            }

            return catmullRomPoints.ToArray();
        }

        private static float GetT(float t, Vector3 p0, Vector3 p1)
        {
            float a = Mathf.Pow((p1.x - p0.x), 2.0f) + Mathf.Pow((p1.y - p0.y), 2.0f) + Mathf.Pow((p1.z - p0.z), 2.0f);
            float b = Mathf.Pow(a, 0.5f);
            float c = Mathf.Pow(b, alpha);

            return (c + t);
        }

    }

}


