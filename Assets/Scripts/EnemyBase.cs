using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : SpawnerUnit
{
	[SerializeField] private float spawnInterval = 2f;
	[SerializeField] private float spawnDuration = 1f;

	private float counterSpawnInterval;

	private void Update()
	{
		counterSpawnInterval += Time.deltaTime;

		if(counterSpawnInterval >= spawnInterval)
		{
			StartCoroutine(SpawnWave());
			counterSpawnInterval = 0;
		}
	}

	private IEnumerator SpawnWave()
	{
		float elapsedTime = 0f;

		while (elapsedTime < spawnDuration)
		{
			SpawnUnit();
			yield return new WaitForSeconds(spawnRate);
			elapsedTime += spawnRate;
		}
	}
}
