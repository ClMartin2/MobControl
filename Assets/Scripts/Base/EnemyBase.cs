using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemyBase : SpawnerUnit
{
	[SerializeField] private float spawnInterval = 2f;
	[SerializeField] private float spawnDuration = 1f;
	[SerializeField] private Dificulty dificulty;
	[SerializeField] private ParticleSystem fxTouch;

	private float counterSpawnInterval;
	private int currentIndexDificulty;

	private void Start()
	{
		SetDificulty();
		StartCoroutine(ChangeLevel());
		counterSpawnInterval = spawnInterval;
	}

	private void Update()
	{
		counterSpawnInterval += Time.deltaTime;

		if(counterSpawnInterval >= spawnInterval)
		{
			StartCoroutine(SpawnWave());
			counterSpawnInterval = 0;
		}
	}

	private IEnumerator ChangeLevel()
	{
		yield return new WaitForSeconds(dificulty.allLevels[currentIndexDificulty].timeToChangeLevel);

		if (currentIndexDificulty < dificulty.allLevels.Length -1)
		{
			currentIndexDificulty++;
			SetDificulty();
			StartCoroutine(ChangeLevel());
			counterSpawnInterval = 0;
			Debug.Log("Level : " + (currentIndexDificulty + 1));
		}
		else
			yield return null;
	}

	private void SetDificulty()
	{
		Level actualLevel = dificulty.allLevels[currentIndexDificulty];

		spawnInterval = actualLevel.spawnInterval;
		spawnDuration = actualLevel.spwanDuration;
		spawnRate = actualLevel.spawnRate;
		numberTospawn = actualLevel.numberToSpawn;
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

	public void Death()
	{
		Debug.Log("Win");
		Destroy(gameObject);
	}

	public void BaseTouch()
	{
		fxTouch.Play();
	}
}
