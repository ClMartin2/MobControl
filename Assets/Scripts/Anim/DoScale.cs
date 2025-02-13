using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class DoScale : MonoBehaviour
{
	[SerializeField] private Vector3 addValueScale = new Vector3(1.5f, 1.5f,0);
	[SerializeField] private float durationAnim = 0.2f;
	[SerializeField] private Ease curveAnimScale = Ease.OutBounce;
	[SerializeField] private Ease curveResetScale = Ease.OutBounce;
	[SerializeField] private Transform transformToScale;
	[SerializeField] private UnityEvent onEndAnim;

	private Vector3 startScale;

	private void Start()
	{
		startScale = transformToScale.localScale;
	}

	public void StartAnim()
	{
		transformToScale.DOScale(startScale + addValueScale, durationAnim).SetEase(curveAnimScale).OnComplete(ResetToStartScale);
	}

	private void ResetToStartScale()
	{
		transformToScale.DOScale(startScale, durationAnim).SetEase(curveResetScale).OnComplete(EndAnim);
	}

	public void EndAnim()
	{
		onEndAnim?.Invoke();
	}
}
