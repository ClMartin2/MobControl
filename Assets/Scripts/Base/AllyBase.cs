using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyBase : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Loose");
	}
}
