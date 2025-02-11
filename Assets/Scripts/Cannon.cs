using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : SpawnerUnit
{
    [SerializeField] private float speed = 500;
    [SerializeField] private float maxLength = 100;
	[SerializeField] private float minX = -20f;
	[SerializeField] private float maxX = 20f;

	private Vector3 lastMousePosition;
	private Camera _camera;

	private void Start()
	{
		_camera = Camera.main;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			lastMousePosition = GetMouseWorldMousePosition();
			counterFireRate = 0;
		}

		if (Input.GetMouseButton(0))
		{
			Move();
			SpawnUnit();
		}
	}

	private void Move()
	{
		Vector3 directionMouse = GetMouseWorldMousePosition() - lastMousePosition;
		directionMouse = Vector3.ClampMagnitude(directionMouse, maxLength);
		float directionMove = Vector3.Dot(Vector3.right, directionMouse.normalized);

		float newX = transform.position.x + (Vector3.left * directionMove * speed * Time.deltaTime * directionMouse.magnitude).x;
		newX = Mathf.Clamp(newX, minX, maxX);

		Vector3 velocity = new Vector3(newX, transform.position.y, transform.position.z);

		transform.position = velocity;

		lastMousePosition = GetMouseWorldMousePosition();
	}

	private Vector3 GetMouseWorldMousePosition()
	{
		Vector3 currentScreenMousePosition = Input.mousePosition;
		currentScreenMousePosition.z = _camera.transform.position.z - transform.position.z;
		Vector3 currentWorldMousePosition = _camera.ScreenToWorldPoint(currentScreenMousePosition);

		return currentWorldMousePosition;
	}

	protected override void SpawnUnit()
	{
		counterFireRate += Time.deltaTime;

		if (counterFireRate >= spawnRate)
		{
			base.SpawnUnit();
			counterFireRate = 0;
		}
	}
}
