using UnityEngine;

public class MoveObject : MonoBehaviour
{
	[Header("Speed Settings")]
	[Space(10)]

	[SerializeField] private float speed = 500;
	[SerializeField] private float maxLength = 100;
	[SerializeField] public float minX = -20f;
	[SerializeField] public float maxX = 20f;

	private Vector3 lastMousePosition;
	private Camera _camera;
	private bool init = false;

	public void Init()
	{
		init = true;
		_camera = Camera.main;
	}

	void Update()
    {
		if (!init)
			return;

		if (Input.GetMouseButtonDown(0))
			lastMousePosition = Utils.GetWorldMousePosition(_camera, transform);

		if (Input.GetMouseButton(0))
		{
			Move();
		}
	}

	private void Move()
	{
		Vector3 directionMouse = Utils.GetWorldMousePosition(_camera, transform) - lastMousePosition;
		directionMouse = Vector3.ClampMagnitude(directionMouse, maxLength);

		float directionMove = Mathf.Sign(Vector3.Dot(Vector3.right, directionMouse.normalized));
		float ratioMouseMovement = directionMouse.magnitude / maxLength;

		float dynamicSpeed = speed * ratioMouseMovement;

		float newX = transform.position.x + (Vector3.left * directionMove * dynamicSpeed * Time.deltaTime).x;
		newX = Mathf.Clamp(newX, minX, maxX);

		transform.position = new Vector3(newX, transform.position.y, transform.position.z);

		lastMousePosition = Utils.GetWorldMousePosition(_camera, transform);
	}
}
