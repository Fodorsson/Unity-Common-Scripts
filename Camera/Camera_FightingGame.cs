using UnityEngine;

public class Camera_FightingGame : MonoBehaviour
{
    public Transform character1, character2;

    [SerializeField] private float elevation = 0.4f;
    [SerializeField] private float lerpSpeed = 5f;

    private Vector3 fightingLane;
    private Vector3 cameraMoveDirection;
    private Vector3 center;
    private float distance;

    private static Vector3 newPosition;
    private static Quaternion newRotation;

    void Update()
    {
        fightingLane = character2.position - character1.position;
        cameraMoveDirection = Vector3.Cross(fightingLane, Vector3.up).normalized;
        
        center = (character1.position + character2.position) / 2f;
        distance = fightingLane.magnitude;

        newPosition = center - distance * cameraMoveDirection;
        newRotation = Quaternion.LookRotation(center - newPosition, Vector3.up);
        newPosition += Vector3.up * elevation * distance;

        Camera.main.transform.position = Vector3.Lerp(transform.position, newPosition, lerpSpeed * Time.deltaTime);
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, newRotation, lerpSpeed * Time.deltaTime);
    }

}
