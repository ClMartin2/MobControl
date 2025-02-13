using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{
	public float timeToChangeLevel;
	public float spawnInterval;
	public float spwanDuration;
	public float spawnRate;
	public int numberToSpawn;
}
