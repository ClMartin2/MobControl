using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
	public static Vector3 GetWorldMousePosition(Camera camera, Transform transform)
	{
		Vector3 currentScreenMousePosition = Input.mousePosition;
		currentScreenMousePosition.z = camera.transform.position.z - transform.position.z;
		Vector3 currentWorldMousePosition = camera.ScreenToWorldPoint(currentScreenMousePosition);

		return currentWorldMousePosition;
	}
}
