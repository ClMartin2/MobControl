using UnityEngine;

public class SpawnerUnit : MonoBehaviour
{
	[SerializeField] protected float spawnRate = 0.5f;
	[SerializeField] private BasicUnit unit;
	[SerializeField] private Transform spawnPosition;
	[SerializeField] private int numberTospawn = 1;

	protected float counterFireRate = 0;

	virtual protected void SpawnUnit()
	{
		for (int i = 0; i < numberTospawn; i++)
		{
			Instantiate(unit, spawnPosition.position, spawnPosition.rotation);
		}
	}
}
