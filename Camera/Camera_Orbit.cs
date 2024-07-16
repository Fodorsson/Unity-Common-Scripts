using UnityEngine;

public class Camera_Orbit : MonoBehaviour
{
	private float x;
	private float y;

	[SerializeField] private float sensitivity = 1f;

	[SerializeField] private float distance = 5f;
	[SerializeField] private float elevation = 0f;

	[SerializeField] private float tiltMinimum = 0f;
	[SerializeField] private float tiltMaximum = 89f;

	[SerializeField] private float distanceMinimum = 1f;
	[SerializeField] private float distanceMaximum = 20f;

	[SerializeField] private float lerpValue = 10f;

	[SerializeField] private KeyCode key_ResetView = KeyCode.X;

	public Transform target;

	void Start()
	{
		x = target.eulerAngles.y;
		y = target.eulerAngles.x;
	}

	void Update()
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			SetZoom(distance - 1f);
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			SetZoom(distance + 1f);
		}

		if (Input.GetKeyDown(key_ResetView))
		{
			ResetView();
		}

		x += Input.GetAxis("Mouse X") * sensitivity;
		y -= Input.GetAxis("Mouse Y") * sensitivity;
		y = Mathf.Clamp(y, tiltMinimum, tiltMaximum);

		Quaternion rotation = Quaternion.Euler(y, x, 0);

		Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
		Vector3 position = rotation * negDistance + target.position;

		position.y += elevation;

		Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, rotation, lerpValue * Time.deltaTime);
		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position, lerpValue * Time.deltaTime);
	}

	public void SetAngles(float newX, float newY)
	{
		x = newX;
		y = newY;
	}

	public void SetZoom(float newDistance)
    {
		distance = Mathf.Clamp(newDistance, distanceMinimum, distanceMaximum);
	}

	private void ResetView()
    {
		SetAngles(0f, 0f);
		SetZoom(5f);
	}

}