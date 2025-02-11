using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
	[SerializeField] private int multiplier = 1;
	[SerializeField] private GameObject objectToSpawn;

	private HashSet<GameObject> alreadyTriggered = new HashSet<GameObject>();

	private void OnTriggerEnter(Collider other)
	{
		if (alreadyTriggered.Contains(other.gameObject)) return;

		alreadyTriggered.Add(other.gameObject);

		for (int i = 1; i < multiplier; i++)
		{
			Transform otherTransform = other.gameObject.transform;

			GameObject multipliedUnit = Instantiate(objectToSpawn, otherTransform.position, otherTransform.rotation);
			alreadyTriggered.Add(multipliedUnit);
		}
	}
}
