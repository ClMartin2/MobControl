using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Cannon : SpawnerUnit
{
	[SerializeField] private Animator animator;
	[SerializeField] private string nameParameterShootAnimation;
	[SerializeField] private Transform[] wheels;

	[Header("Speed Settings")]
	[Space(10)]

	[SerializeField] private float speed = 500;
    [SerializeField] private float wheelRotationSpeed = 500;
    [SerializeField] private float maxLength = 100;
	[SerializeField] private float minX = -20f;
	[SerializeField] private float maxX = 20f;

	[Header("Muzzle Flash Settings")]
	[Space(10)]
	[SerializeField] private GameObject muzzleFlash;
	[SerializeField] private float startAlphaMuzzleFlash = 0;
	[SerializeField] private float endAlphaMuzzleFlash = 1;
	[SerializeField] private float durationApparition = 0.1f;
	[SerializeField] private float durationDisparition = 0.05f;
	[SerializeField] private float stayAtScreen = 0.3f;

	[Space(10)]
	[SerializeField] private UnityEvent onShoot;
	[SerializeField] private UnityEvent onStopShoot;

	private SpriteRenderer muzzleRenderer;
	private Vector3 lastMousePosition;
	private Camera _camera;
	private bool muzzleFlashActivate = false;

	private void Start()
	{
		_camera = Camera.main;

		muzzleFlash.TryGetComponent<SpriteRenderer>(out muzzleRenderer);
		Color newColor = muzzleRenderer.color;
		newColor.a = startAlphaMuzzleFlash;
		muzzleRenderer.color = newColor;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			lastMousePosition = GetMouseWorldMousePosition();
			counterFireRate = 0;
		}else if (Input.GetMouseButtonUp(0))
		{
			onStopShoot?.Invoke();
		}

		if (Input.GetMouseButton(0))
		{
			Move();
			Shoot();
		}
	}

	private void Shoot()
	{
		counterFireRate += Time.deltaTime;

		if (counterFireRate >= spawnRate)
		{
			List<BasicUnit> units = new List<BasicUnit>();
			units = SpawnUnit();

			animator.SetBool(nameParameterShootAnimation,true);
			counterFireRate = 0;
			StartCoroutine(DelayResetAnimation());

			if (muzzleFlashActivate)
				return;

			muzzleFlashActivate = true;
			muzzleFlash.SetActive(true);
			Color newColor = muzzleRenderer.color;
			newColor.a = endAlphaMuzzleFlash;

			muzzleRenderer.DOColor(newColor, durationApparition).OnComplete(DisparittionMuzzleFlash);

			onShoot?.Invoke();
		}
	}

	private void DisparittionMuzzleFlash()
	{
		StartCoroutine(WaitToDeactivateMuzzleFLash());
	}

	private IEnumerator WaitToDeactivateMuzzleFLash()
	{
		yield return new WaitForSeconds(stayAtScreen);
		Color newColor = muzzleRenderer.color;
		newColor.a = startAlphaMuzzleFlash;

		muzzleRenderer.DOColor(newColor, durationDisparition).OnComplete(DeactivateMuzzleFlash);
	}

	private void DeactivateMuzzleFlash()
	{
		muzzleFlash.SetActive(false);
		muzzleFlashActivate = false;
	}

	private IEnumerator DelayResetAnimation()
	{
		yield return new WaitForSeconds(0.05f);
		animator.SetBool(nameParameterShootAnimation, false);
	}

	private void Move()
	{
		Vector3 directionMouse = GetMouseWorldMousePosition() - lastMousePosition;
		directionMouse = Vector3.ClampMagnitude(directionMouse, maxLength);

		float directionMove = Mathf.Sign(Vector3.Dot(Vector3.right, directionMouse.normalized));
		float ratioMouseMovement = directionMouse.magnitude / maxLength;

		float dynamicSpeed = speed * ratioMouseMovement;

		float newX = transform.position.x + (Vector3.left * directionMove * dynamicSpeed * Time.deltaTime).x;
		newX = Mathf.Clamp(newX, minX, maxX);

		transform.position = new Vector3(newX, transform.position.y, transform.position.z);

		foreach (var wheel in wheels)
		{
			float dynamicSpeedRotation = wheelRotationSpeed * ratioMouseMovement;
			float newAngle = directionMove * dynamicSpeedRotation * Time.deltaTime;

			wheel.rotation = wheel.rotation * Quaternion.AngleAxis(newAngle, Vector3.forward);
		}

		lastMousePosition = GetMouseWorldMousePosition();
	}

	private Vector3 GetMouseWorldMousePosition()
	{
		Vector3 currentScreenMousePosition = Input.mousePosition;
		currentScreenMousePosition.z = _camera.transform.position.z - transform.position.z;
		Vector3 currentWorldMousePosition = _camera.ScreenToWorldPoint(currentScreenMousePosition);

		return currentWorldMousePosition;
	}
}
