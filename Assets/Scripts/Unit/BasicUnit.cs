using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine.VFX;
using System.Collections;
using UnityEngine.Events;

public delegate void OnDeath(BasicUnit sender);

[RequireComponent(typeof(Rigidbody))]
public class BasicUnit : MonoBehaviour
{
	[SerializeField] private float speed = 100f;
	[SerializeField] private LayerMask layerMaskEnemy;
	[SerializeField] public Rigidbody rb { get; private set; }
	[SerializeField] private Collider _collider;
	[SerializeField] public UnityEvent _callBackOnDeathToEnemy;

	[Header("Spawn FX Settings")]
	[Space(10)]

	[SerializeField] private GameObject vfxSpawn;
	[SerializeField] private Transform positionVfxSpawn;
	[SerializeField] private float delaySmokeFX = 0;

	[Header("Death Settings")]
	[Space(10)]

	[SerializeField] private float deathYPositionOffset = -5f;
	[SerializeField] private Vector2 minRotationOffset;
	[SerializeField] private Vector2 maxRotationOffset;
	[SerializeField] private float durationAnimDeath = 0.5f;
	[SerializeField] private Ease curveAnimDeath = Ease.Linear;
	[SerializeField] private Animator animator;
	[SerializeField] private Material deathMaterial;
	[SerializeField] private string nameParameterDeath = "death";

	public event OnDeath onDeath;

	public void Init()
	{
		rb = GetComponent<Rigidbody>();
	}

	virtual protected void Start()
	{
		Init();
		StartCoroutine(InstantiateVFXSPawn());
	}

	private IEnumerator InstantiateVFXSPawn()
	{
		yield return new WaitForSeconds(delaySmokeFX);
		Instantiate(vfxSpawn, positionVfxSpawn.position, Quaternion.identity);
		//StartCoroutine(CheckDeactivateVFX());
	}

	//private IEnumerator CheckDeactivateVFX()
	//{
	//	vfxSpawn.TryGetComponent<VisualEffect>(out VisualEffect vfx);

	//	while (vfx.aliveParticleCount > 0) {
	//		Debug.Log("Wait");
	//		yield return new WaitForSeconds(0.1f);
	//	};
	//	vfxSpawn.SetActive(false);
	//	yield return null;
	//}

	private void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		rb.velocity = transform.forward * speed * Time.fixedDeltaTime;
	}

	protected void OnCollisionEnter(Collision collision)
	{
		GameObject objectCollision = collision.gameObject;

		if ((layerMaskEnemy.value & (1 << objectCollision.layer)) != 0)
		{
			Death();

			if (objectCollision.TryGetComponent<BasicUnit>(out BasicUnit unit))
			{
				unit.Death();
			}

			_callBackOnDeathToEnemy?.Invoke();
		}
	}

	virtual public void Death()
	{
		SkinnedMeshRenderer skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
		Material[] materials = skinnedMeshRenderer.materials;
		materials[0] = deathMaterial;
		skinnedMeshRenderer.materials = materials;
		_collider.enabled = false;
		rb.isKinematic = false;

		Vector3 endPosition = transform.position + Vector3.up * deathYPositionOffset;
		Quaternion endRotation = transform.rotation * Quaternion.AngleAxis(GenerateRandoAngle(), Vector3.forward);

		transform.DOMove(endPosition, durationAnimDeath).SetEase(curveAnimDeath).OnComplete(OnCompletAnimDeath);
		transform.DORotate(endRotation.eulerAngles, durationAnimDeath).SetEase(curveAnimDeath);

		animator.SetBool(nameParameterDeath, true);

		onDeath?.Invoke(this);
	}

	private float GenerateRandoAngle()
	{
		if (Random.value < 0.5f)
			return Random.Range(minRotationOffset.x, minRotationOffset.x);
		else
			return Random.Range(minRotationOffset.y,maxRotationOffset.y);
	}

	private void OnCompletAnimDeath()
	{
		Destroy(gameObject);
	}
}
