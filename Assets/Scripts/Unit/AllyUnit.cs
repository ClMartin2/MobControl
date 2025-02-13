using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyUnit : BasicUnit
{
	[SerializeField] private ParticleSystem particleSystemSpawn;


	protected override void Start()
	{
		base.Start();
		particleSystemSpawn.Play();
		particleSystemSpawn.transform.parent = null;
	}
}
