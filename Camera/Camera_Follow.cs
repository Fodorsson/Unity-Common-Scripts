using UnityEngine;

public class Camera_Follow : MonoBehaviour 
{
	public Transform target;

	[SerializeField] private float elevation = 1f;
	[SerializeField] private float distance = 5f;
	[SerializeField] private float lerpValue = 10f;

	private Vector3 offset;

	void Update()
	{
		offset = new Vector3(0f, elevation, -distance);

		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, target.position + target.rotation * offset, lerpValue * Time.deltaTime);
		Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, target.rotation, lerpValue);
	}
}
