using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
	[SerializeField] public float cost;
	[SerializeField] protected float addValue;
	[SerializeField] private TextMeshProUGUI costText;
	[SerializeField] private Button btn;
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private float alphaDeactivate = 0.75f;

	private void Start()
	{
		costText.text = "$" + cost.ToString();
		btn.onClick.AddListener(AddValue);
	}

	virtual public void AddValue()
	{
	}

	public void Deactivate()
	{
		btn.enabled = false;
		canvasGroup.alpha = alphaDeactivate;
	}
}
