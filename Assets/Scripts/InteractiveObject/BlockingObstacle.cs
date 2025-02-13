using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BlockingObstacle : MonoBehaviour
{
	[SerializeField] private int life = 50;
	[SerializeField] private TextMeshPro lifeTxt;
	[SerializeField] private UnityEvent onDeath;
	[SerializeField] private UnityEvent onCollision;

	private void Start()
	{
		lifeTxt.text = life.ToString();
	}

	private void OnCollisionEnter(Collision collision)
	{
		life--;
		lifeTxt.text = life.ToString();
		onCollision?.Invoke();

		GameObject objectCollision = collision.gameObject;

		if (objectCollision.TryGetComponent<BasicUnit>(out BasicUnit unit))
		{
			unit.Death();
		}

		if (life <= 0)
		{
			Death();
		}
	}

	private void Death()
	{
		onDeath?.Invoke();
	}
}
