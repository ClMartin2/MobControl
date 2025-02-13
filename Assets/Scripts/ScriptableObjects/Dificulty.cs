using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dificulty", order = 1)]

public class Dificulty : ScriptableObject
{
    public Level[] allLevels;
}
