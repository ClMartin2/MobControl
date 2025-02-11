using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ToonyColorsPro.ShaderGenerator.Enums;

[RequireComponent(typeof (Rigidbody))]
public class BasicUnit : MonoBehaviour
{
	[SerializeField] private float speed = 100f;
	[SerializeField] private LayerMask layerMaskEnemy;

	private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		rb.velocity = transform.forward * speed * Time.fixedDeltaTime;
	}

	protected void OnCollisionEnter(Collision collision)
	{
		if ((layerMaskEnemy.value & (1 << collision.gameObject.layer)) != 0)
		{
			Destroy(gameObject);
			Destroy(collision.gameObject);
		}
	}
}
