using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.VFX;
using System;

public class Cannon : SpawnerUnit
{
	[SerializeField] private Animator animator;
	[SerializeField] private string nameParameterShootAnimation;
	[SerializeField] private Transform[] wheels;
	[SerializeField] private DoHDRColor[] doHDRColors;
	[SerializeField] private DoScale doScale;
	[SerializeField] private DoScale doScaleApparition;
	[SerializeField] private VisualEffect vfxUpgrade;
	[SerializeField] private bool spawnAtStart = false;

	[Header("Speed Settings")]
	[Space(10)]
	[SerializeField] private float wheelRotationSpeed = 500;
	[SerializeField] private float maxLength = 100;

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

	[NonSerialized]
	public bool doUpgradeAnim = true;

	private SpriteRenderer muzzleRenderer;
	private Vector3 lastMousePosition;
	private Camera _camera;
	private bool muzzleFlashActivate = false;
	private bool spawn = false;

	private void Start()
	{
		_camera = Camera.main;

		muzzleFlash.TryGetComponent<SpriteRenderer>(out muzzleRenderer);
		Color newColor = muzzleRenderer.color;
		newColor.a = startAlphaMuzzleFlash;

		muzzleRenderer.color = newColor;
		doUpgradeAnim = true;

		if (spawnAtStart)
			Spawn();
	}

	private void Update()
	{
		if (!init || !spawn)
			return;

		if (Input.GetMouseButtonDown(0))
		{
			lastMousePosition = Utils.GetWorldMousePosition(_camera, transform);
			Shoot();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			onStopShoot?.Invoke();
		}

		if (Input.GetMouseButton(0))
		{
			RotateWheel();
			counterFireRate += Time.deltaTime;

			if (counterFireRate >= spawnRate)
			{
				Shoot();
			}
		}
	}

	private void Shoot()
	{
		List<BasicUnit> units = new List<BasicUnit>();
		units = SpawnUnit();

		animator.SetBool(nameParameterShootAnimation, true);
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

	private void RotateWheel()
	{
		Vector3 directionMouse = Utils.GetWorldMousePosition(_camera, transform) - lastMousePosition;
		directionMouse = Vector3.ClampMagnitude(directionMouse, maxLength);

		float directionMove = Mathf.Sign(Vector3.Dot(Vector3.right, directionMouse.normalized));
		float ratioMouseMovement = directionMouse.magnitude / maxLength;

		foreach (var wheel in wheels)
		{
			float dynamicSpeedRotation = wheelRotationSpeed * ratioMouseMovement;
			float newAngle = directionMove * dynamicSpeedRotation * Time.deltaTime;

			wheel.rotation = wheel.rotation * Quaternion.AngleAxis(newAngle, Vector3.forward);
		}

		lastMousePosition = Utils.GetWorldMousePosition(_camera, transform);
	}

	public void Upgrade()
	{
		if (!spawn)
			return;

		foreach (var doHDRColor in doHDRColors)
		{
			doHDRColor.StartLerp();
		}

		vfxUpgrade.Play();

		if (doUpgradeAnim)
			doScale.StartAnim();
		else
			doUpgradeAnim = true;
	}

	public void Spawn()
	{
		spawn = true;
		doScaleApparition.StartAnim();
	}
}
