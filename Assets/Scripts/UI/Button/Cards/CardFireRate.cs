
public class CardFireRate : Card
{
	override public void AddValue()
    {
        _GameManager.Instance.AddFireRate(addValue, cost);
    }
}
