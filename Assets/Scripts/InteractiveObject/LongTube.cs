using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class LongTube : MonoBehaviour
{
    [SerializeField] private float timeToTravelTube = 1f;
    [SerializeField] private Transform end;
    [SerializeField] private Transform exit;
    [SerializeField] private UnityEvent enterTube;
    [SerializeField] private UnityEvent exitTube;

	private void OnCollisionEnter(Collision collision)
	{
		GameObject objectToSpawn = collision.gameObject;
		objectToSpawn.SetActive(false);
		StartCoroutine(SpawnAtEnd(objectToSpawn));
		enterTube?.Invoke();
	}

	private IEnumerator SpawnAtEnd(GameObject objectToSpawn)
	{
		yield return new WaitForSeconds(timeToTravelTube);

		if (objectToSpawn == null)
			yield break;

		Vector3 endPosition = new Vector3(end.position.x, objectToSpawn.transform.position.y, end.position.z);

		objectToSpawn.transform.position = endPosition;
		objectToSpawn.transform.rotation = end.rotation;

		objectToSpawn.SetActive(true);
		exitTube?.Invoke();

		yield return null;
	}
}
