using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
	[SerializeField] private int multiplier = 1;
	[SerializeField] private GameObject objectToSpawn;
	[SerializeField] private TextMeshPro textMultiplier; 
	[SerializeField] private float forceStrength = 3f;
	[SerializeField] private float angleSpread = 30f;

	private HashSet<GameObject> alreadyTriggered = new HashSet<GameObject>();

	virtual protected void Start()
	{
		textMultiplier.text = "x" + multiplier.ToString();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (alreadyTriggered.Contains(other.gameObject)) return;

		alreadyTriggered.Add(other.gameObject);

		for (int i = 1; i < multiplier; i++)
		{
			Transform otherTransform = other.gameObject.transform;

			GameObject multipliedUnit = Instantiate(objectToSpawn, otherTransform.position, otherTransform.rotation);
			alreadyTriggered.Add(multipliedUnit);

			PushUnit(multipliedUnit);
		}
	}

	private void PushUnit(GameObject unit)
	{
		Rigidbody rb = unit.GetComponent<Rigidbody>();

		if (rb != null)
		{
			Vector3 forceDirection = GetRandomDirection();
			rb.AddForce(forceDirection * forceStrength, ForceMode.Impulse);
		}
	}

	private Vector3 GetRandomDirection()
	{
		float angle = Random.Range(-angleSpread, angleSpread);
		Quaternion rotation = Quaternion.Euler(0, angle, 0);
		return rotation * transform.forward;
	}
}
