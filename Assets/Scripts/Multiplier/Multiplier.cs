using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Multiplier : SpawnerUnit
{
	[SerializeField] private TextMeshPro textMultiplier; 
	[SerializeField] private UnityEvent onMultiply; 

	private HashSet<GameObject> alreadyTriggered = new HashSet<GameObject>();

	virtual protected void Start()
	{
		textMultiplier.text = "X" + numberTospawn.ToString();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (alreadyTriggered.Contains(other.gameObject)) return;

		spawnPosition = other.transform;

		List<BasicUnit> allSpawnUnits = SpawnUnit();
		allSpawnUnits.Add(other.GetComponent<BasicUnit>());

		foreach (BasicUnit unit in allSpawnUnits) {
			alreadyTriggered.Add(unit.gameObject);
		}

		foreach (var unit in allSpawnUnits)
		{
			unit.onDeath += Unit_onDeath;
		}

		onMultiply?.Invoke();
	}

	private void Unit_onDeath(BasicUnit sender)
	{
		RemoveObjectAlreadyTriggered(sender.gameObject);
	}

	public void RemoveObjectAlreadyTriggered(GameObject objectToRemove)
	{
		alreadyTriggered.Remove(objectToRemove);
	}
}
