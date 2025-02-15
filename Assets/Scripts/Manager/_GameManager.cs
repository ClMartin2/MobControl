using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class _GameManager : MonoBehaviour
{
	[SerializeField] private AllyBase allyBase;
	[SerializeField] private Cannon[] cannons;
	[SerializeField] private MoveObject moveObject;
	[SerializeField] private EnemyBase enemyBase;
	[SerializeField] private GameObject containerTxtLoose;
	[SerializeField] private GameObject containerTxtWin;
	[SerializeField] private Transform cardsContainer;
	[SerializeField] private TextMeshProUGUI textCurrency;
	[SerializeField] private float startCurrency = 190521;
	[SerializeField] private Card[] cards;
	[SerializeField] private float minXTwoCannon = -3.5f;
	[SerializeField] private float maxXTwoCannon = 2f;

	[Header("Events")]
	[Space(10)]

	[SerializeField] private UnityEvent onLose;
	[SerializeField] private UnityEvent onWin;
	[SerializeField] private UnityEvent buyUpgrade;

	private bool loose = false;
	private bool win = false;
	private float actualCurrency;

	public static _GameManager Instance { get; private set; }

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);

		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			Instance = this;
		}
	}

	private void Start()
	{
		allyBase.onLose += AllyBase_onLose;
		enemyBase.onWin += EnemyBase_onWin;

		textCurrency.text = startCurrency.ToString();
		actualCurrency = startCurrency;
	}

	private void EnemyBase_onWin(EnemyBase sender)
	{
		Win();
	}

	private void AllyBase_onLose(AllyBase sender)
	{
		Loose();
	}

	public void Loose()
	{
		if (loose || win)
			return;

		loose = true;
		containerTxtLoose.SetActive(true);
		onLose?.Invoke();
	}

	public void Win()
	{
		if (loose || win)
			return;

		win = true;
		containerTxtWin.SetActive(true);
		onWin?.Invoke();
	}

	public void PauseGame()
	{
		Time.timeScale = 0f;
	}

	public void StartGame()
	{
		foreach (var cannon in cannons)
		{
			cannon.Init();
		}

		enemyBase.Init();
		moveObject.Init();
		cardsContainer.gameObject.SetActive(false);
	}

	public void AddFireRate(float valueToAdd, float costCurrency)
	{
		if (actualCurrency >= costCurrency)
		{
			foreach (var cannon in cannons)
			{
				cannon.spawnRate += valueToAdd;
			}

			Buy(costCurrency);
		}
	}
	
	public void AddFirePower(float valueToAdd, float costCurrency)
	{
		if (actualCurrency >= costCurrency)
		{
			cannons[1].doUpgradeAnim = false;
			cannons[1].Spawn();

			Vector3 newCannonPosition = cannons[0].transform.position;
			newCannonPosition.x = Mathf.Abs(cannons[1].transform.position.x);

			cannons[0].transform.position = newCannonPosition;
			Buy(costCurrency);

			moveObject.minX = minXTwoCannon;
			moveObject.maxX = maxXTwoCannon;
		}
	}

	private void RefreshTextCurrency()
	{
		textCurrency.text = actualCurrency.ToString();	
	}

	public void Buy(float cost)
	{
		actualCurrency -= cost;
		RefreshTextCurrency();

		foreach (var card in cards)
		{
			if (card.cost > actualCurrency)
				card.Deactivate();
		}

		buyUpgrade?.Invoke();
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(0);
	}
}
