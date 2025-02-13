using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
	[SerializeField] private float updateInterval = 0.5f;
	[SerializeField] private TextMeshProUGUI txtFps;

	private float accum = 0;
	private int frames = 0;
	private float timeleft;
	private float fps;

	void Start()
	{
		timeleft = updateInterval;
	}

	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;

		if (timeleft <= 0.0)
		{
			fps = accum / frames;
			timeleft = updateInterval;
			accum = 0.0f;
			frames = 0;
		}

		txtFps.text = fps.ToString();
	}
}
