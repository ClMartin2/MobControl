using Unity.VisualScripting;
using UnityEngine;

public delegate void EventHandlerAllyBase(AllyBase sender);

public class AllyBase : MonoBehaviour
{
	public event EventHandlerAllyBase onLose;

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<EnemyUnit>(out EnemyUnit unit))
			unit.Death();

		onLose?.Invoke(this);
	}
}
