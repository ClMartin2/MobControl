using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoColor : MonoBehaviour
{
	[SerializeField] private MeshRenderer meshRenderer;
	[SerializeField] private Color newColor;
	[SerializeField] private string nameColor = "_EmissionColor";
	[SerializeField] private float durationLerpNewColor;
	[SerializeField] private float durationLerpStartColor;
	[SerializeField] private Ease curveLerpNewColor = Ease.Linear;
	[SerializeField] private Ease curveLerpStartColor = Ease.Linear;
	[SerializeField] private bool resetAfterLerp;

	private Color startColor;

	private void Start()
	{
		startColor = meshRenderer.material.GetColor(nameColor);
	}

	public void StartToShine()
	{
		Action callBack = resetAfterLerp ? ResetMaterial : null;
		SetEmissionColor(newColor, durationLerpNewColor, curveLerpNewColor, callBack);
	}

	public void ResetMaterial()
	{
		SetEmissionColor(startColor, durationLerpStartColor, curveLerpStartColor);
	}

	private void SetEmissionColor(Color color, float duration, Ease curve, Action callBack = null)
	{
		meshRenderer.material.DOColor(color, nameColor, duration).SetEase(curve).OnComplete(() => callBack());
	}
}
