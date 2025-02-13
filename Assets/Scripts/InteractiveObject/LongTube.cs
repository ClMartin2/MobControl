using System.Collections;
using UnityEngine;
using DG.Tweening;

public class LongTube : MonoBehaviour
{
    [SerializeField] private float timeToTravelTube = 1f;
    [SerializeField] private Transform end;
    [SerializeField] private Transform exit;
    [SerializeField] private float durationAnimEnter;
    [SerializeField] private Vector3 animEnterAddScale;
    [SerializeField] private Ease easeAnimEnter = Ease.Linear;
    [SerializeField] private Ease easeAnimReset = Ease.Linear;
    
	private Vector3 startScaleEnter;
	private Vector3 startScaleExit;

	private void Start()
	{
		startScaleEnter = transform.lossyScale;
		startScaleExit = exit.lossyScale;
	}

	private void OnCollisionEnter(Collision collision)
	{
		GameObject objectToSpawn = collision.gameObject;
		objectToSpawn.SetActive(false);
		StartCoroutine(SpawnAtEnd(objectToSpawn));

		Vector3 _endScale = startScaleExit + animEnterAddScale;
		transform.DOScale(_endScale,durationAnimEnter).OnComplete(ResetScaleEnter).SetEase(easeAnimEnter);
	}

	private void ResetScaleEnter()
	{
		transform.DOScale(startScaleEnter, durationAnimEnter).SetEase(easeAnimReset);
	}
	
	private void ResetScaleExit()
	{
		exit.DOScale(startScaleExit, durationAnimEnter).SetEase(easeAnimReset); ;
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

		Vector3 _endScale = startScaleExit + animEnterAddScale;
		exit.transform.DOScale(_endScale, durationAnimEnter).OnComplete(ResetScaleExit).SetEase(easeAnimEnter);


		yield return null;
	}
}
