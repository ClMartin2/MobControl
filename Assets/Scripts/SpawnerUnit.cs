using System.Collections.Generic;
using UnityEngine;

public class SpawnerUnit : MonoBehaviour
{
	[SerializeField] protected float spawnRate = 0.5f;
	[SerializeField] private BasicUnit unit;
	[SerializeField] protected Transform spawnPosition;
	[SerializeField] protected int numberTospawn = 1;
	[SerializeField] private float forceStrength = 3f;
	[SerializeField] private float angleSpread = 30f;

	protected float counterFireRate = 0;

	virtual protected List<BasicUnit> SpawnUnit()
	{
		List<BasicUnit> spawnsUnit = new List<BasicUnit>();

		for (int i = 0; i < numberTospawn; i++)
		{
			BasicUnit _unit = Instantiate(unit, spawnPosition.position, spawnPosition.rotation);
			_unit.Init();
			spawnsUnit.Add(_unit);
			PushUnit(_unit);
		}

		return spawnsUnit;
	}

	private void PushUnit(BasicUnit unit)
	{
		Rigidbody rb = unit.rb;

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
