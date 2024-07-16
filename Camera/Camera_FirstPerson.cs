using System;
using UnityEngine;

public class Camera_FirstPerson : MonoBehaviour
{
    public Transform origin;

    [SerializeField] private float sensitivity = 1f;

    [SerializeField] private float tiltMinimum = -89f;
    [SerializeField] private float tiltMaximum = 89f;

    [SerializeField] private bool xInverted = false;
    [SerializeField] private bool yInverted = false;

    [SerializeField] private float lerpValue = 10f;

    private float x, y;

    private Quaternion rotation;

    void Update()
    {
        Camera.main.transform.position = origin.position;

        x += Input.GetAxis("Mouse X") * sensitivity * BoolToSign(xInverted);
        y += Input.GetAxis("Mouse Y") * sensitivity * BoolToSign(yInverted);
        y = Mathf.Clamp(y, tiltMinimum, tiltMaximum);

        rotation = Quaternion.Euler(new Vector3(-y, x, 0f));

        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, rotation, lerpValue * Time.deltaTime);
    }

    private int BoolToSign(bool value)
    {
        return 1 - 2 * Convert.ToInt32(value);
    }

    public void ForceRotate(Vector3 newRotation)
    {
        x += newRotation.y;
        y += -newRotation.x;

        rotation = Quaternion.Euler(new Vector3(-y, x, 0f));
        Camera.main.transform.rotation = rotation;
    }

}
