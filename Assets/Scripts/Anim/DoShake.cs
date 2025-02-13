using DG.Tweening;
using UnityEngine;

public class DoShake : MonoBehaviour
{
	[SerializeField] private Transform transformToShake;
	[SerializeField] private float duration = 0.2f;
	[SerializeField] private float strength = 2f;
	[SerializeField] private int vibrato = 10;
	[SerializeField] private float randomness = 90;
	[SerializeField] private ShakeRandomnessMode shakeRandomnessMode = ShakeRandomnessMode.Full;
	[SerializeField] private Ease curve = Ease.Linear;

	private Vector3 startPosition;

	private void Start()
	{
		startPosition = transformToShake.position;
	}

	public void Shake()
	{
		transformToShake.DOShakePosition(duration,strength,vibrato,randomness,false,true,shakeRandomnessMode).SetEase(curve).OnComplete(ResetPosition);
	}

	private void ResetPosition()
	{
		transformToShake.position = startPosition;
	}
}
