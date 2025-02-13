using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTarget : MonoBehaviour
{
    [SerializeField] private Transform target;

	private void OnTriggerEnter(Collider other)
	{
		Vector3 direction = target.position - other.transform.position;
		direction = direction.normalized;

		other.transform.forward = direction;
	}
}
