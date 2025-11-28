using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Game/Character/Character Data")]
public class CharacterData : ScriptableObject
{
    public int MaxHealth;
    public int Damage;
    public float MoveSpeed;
    public float AttackRange;
    public int AttackPerMinute = 60;
}