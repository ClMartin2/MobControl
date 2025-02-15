using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoColor : MonoBehaviour
{
	[SerializeField] private MeshRenderer meshRenderer;
	[SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
	[SerializeField] private Color newColor;
	[SerializeField] private string nameColor = "_EmissionColor";
	[SerializeField] private float durationLerpNewColor;
	[SerializeField] private float durationLerpStartColor;
	[SerializeField] private float intensityHDRColor;
	[SerializeField] private Ease curveLerpNewColor = Ease.Linear;
	[SerializeField] private Ease curveLerpStartColor = Ease.Linear;
	[SerializeField] private bool resetAfterLerp;

	private Color startColor;
	private Renderer _renderer;

	private void Start()
	{
		_renderer = meshRenderer ? meshRenderer : skinnedMeshRenderer;
		startColor = _renderer.material.GetColor(nameColor);
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
		_renderer.material.DOColor(color, nameColor, duration).SetEase(curve).OnComplete(() => callBack());
	}
}
