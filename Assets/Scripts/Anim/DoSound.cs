using DG.Tweening;
using UnityEngine;

public class DoSound : MonoBehaviour
{
	[SerializeField] private AudioClip[] audioClips;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private bool checkIsPlaying = false;

	public void StartAudio()
	{
		if (checkIsPlaying && audioSource.isPlaying)
			return;

		int currentIndex = Random.Range(0, audioClips.Length);

		audioSource.clip = audioClips[currentIndex];
		audioSource.Play();
	}
}
