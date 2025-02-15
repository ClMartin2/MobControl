using System.Collections;
using UnityEngine;
using DG.Tweening;

public delegate void EventHandlerEnemyBase (EnemyBase sender);

public class EnemyBase : SpawnerUnit
{
	[SerializeField] private float spawnInterval = 2f;
	[SerializeField] private float spawnDuration = 1f;
	[SerializeField] private Dificulty dificulty;
	[SerializeField] private ParticleSystem fxTouch;

	public event EventHandlerEnemyBase onWin;

	private float counterSpawnInterval;
	private int currentIndexDificulty;

	private void Start()
	{
		SetDificulty();
		counterSpawnInterval = spawnInterval;
	}

	public override void Init()
	{
		base.Init();
		StartCoroutine(ChangeLevel());
	}

	private void Update()
	{
		if (!init)
			return;

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
		onWin?.Invoke(this);
		Destroy(gameObject);
	}

	public void BaseTouch()
	{
		fxTouch.Play();
	}
}
