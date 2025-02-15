using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;

public class DoScale : MonoBehaviour
{
	[SerializeField] private Vector3 addValueScale = new Vector3(1.5f, 1.5f,0);
	[SerializeField] private float durationAnim = 0.2f;
	[SerializeField] private Ease curveAnimScale = Ease.OutBounce;
	[SerializeField] private Ease curveResetScale = Ease.OutBounce;
	[SerializeField] private Transform transformToScale;
	[SerializeField] private bool keepScaleTransform = false;
	[SerializeField] private UnityEvent onEndAnim;
	[SerializeField] private UnityEvent onEndAnimStart;

	private Vector3 startScale;

	private void Start()
	{
		startScale = transformToScale.localScale;
	}
	
	public void Init()
	{
		startScale = transformToScale.localScale;
	}

	public void StartAnim()
	{
		Action callBack = keepScaleTransform ? callBack = null : callBack = ResetToStartScale;

		transformToScale.DOScale(startScale + addValueScale, durationAnim).SetEase(curveAnimScale).OnComplete(() => { EndStartAnim(); callBack(); });
	}

	private void ResetToStartScale()
	{
		transformToScale.DOScale(startScale, durationAnim).SetEase(curveResetScale).OnComplete(EndAnim);
	}

	private void EndAnim()
	{
		onEndAnim?.Invoke();
	}
	
	private void EndStartAnim()
	{
		onEndAnimStart?.Invoke();
	}
}
