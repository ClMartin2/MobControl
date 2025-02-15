
public class CardFirePower : Card
{
	override public void AddValue()
    {
        _GameManager.Instance.AddFirePower(addValue, cost);
    }
}
