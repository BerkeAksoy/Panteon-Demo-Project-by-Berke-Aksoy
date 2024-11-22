using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character Data/Create New Character Data")]

public class CharAttrSO : ScriptableObject
{
    public float runSpeed = 1f, horSpeed = 0.8f, charsize = 0.1f;
    public string charName;
    public int priority;
    public Enemy.IntelligenceLevel intelligenceLevel;
}
