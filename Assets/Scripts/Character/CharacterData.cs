using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    public int MaxHealth;
    public int Damage;
    public float MoveSpeed;
    public float AttackRange;
    public float AttackRate;
}