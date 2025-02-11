using System.Collections.Generic;
using UnityEngine;

public class MovingMultiplier : Multiplier
{
	[SerializeField] private Transform[] targets;
	[SerializeField] private float maxSpeed = 5f; 
	[SerializeField] private AnimationCurve speedCurve;

	private List<Vector3> targetsPosition = new List<Vector3>();

	private float distanceToTarget;
	private float initialDistance;
	private Vector3 currentTargetPosition;

	override protected void Start()
	{
		base.Start();

		foreach (var target in targets)
		{
			targetsPosition.Add(target.position);
		}

		if (targets.Length > 0)
		{
			currentTargetPosition = targetsPosition[0];
			initialDistance = Vector3.Distance(transform.position, currentTargetPosition);
		}
	}

	private void Update()
	{
		distanceToTarget = Vector3.Distance(transform.position, currentTargetPosition);
		float ratioDistance = 1 - (distanceToTarget / initialDistance);
		float currentSpeed = speedCurve.Evaluate(ratioDistance) * maxSpeed;

		transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, currentSpeed * Time.deltaTime);

		if (distanceToTarget <= 0.1f)
		{
			SwitchTarget();
		}
	}

	private void SwitchTarget()
	{
		if (targets.Length == 0) return;

		int currentIndex = targetsPosition.IndexOf(currentTargetPosition);

		int nextIndex = (currentIndex + 1) % targetsPosition.Count; 

		currentTargetPosition = targetsPosition[nextIndex]; 
		initialDistance = Vector3.Distance(transform.position, currentTargetPosition); 
	}
}
