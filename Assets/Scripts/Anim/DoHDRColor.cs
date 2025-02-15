using System.Collections;
using UnityEngine;

public class DoHDRColor : MonoBehaviour
{
	[SerializeField] private Renderer _renderer;
	[ColorUsage(true, true)] [SerializeField] private Color endColor;
	[ColorUsage(true, true)] [SerializeField] private Color startColor;
	[SerializeField] private string nameColorToChange;
	[SerializeField] private float lerpDuration;

	private MaterialPropertyBlock _propBlock;

	private void Start()
	{
		_propBlock = new MaterialPropertyBlock();

		_renderer.GetPropertyBlock(_propBlock);
		SetColorRenderer(startColor);
	}

	public void StartLerp()
	{
		StartCoroutine(LerpColor(GetActualColor(), endColor));
	}

	public void EndLerp()
	{
		StartCoroutine(LerpColor(GetActualColor(), startColor));
	}

	private Color GetActualColor()
	{
		return _propBlock.GetColor(nameColorToChange);
	}

	private void SetColorRenderer(Color color)
	{
		_propBlock.SetVector(nameColorToChange, (Vector4)color);
		_renderer.SetPropertyBlock(_propBlock);
	}

	private IEnumerator LerpColor(Color startColor, Color endColor)
	{
		float time = 0;

		while (time < lerpDuration)
		{
			float delta = Time.deltaTime;
			float t = (time + delta > lerpDuration) ? 1 : (time / lerpDuration);
			time += delta;

			Color newColor = Color.Lerp(startColor,endColor,t);
			SetColorRenderer(newColor);

			yield return null;
		}

		yield return null;
	}
}
