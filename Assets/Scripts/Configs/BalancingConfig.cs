using UnityEngine;

[CreateAssetMenu(fileName = "Balancing", menuName = "Configs/Balancing")]
public class BalancingConfig : ScriptableObject
{
    [Header("Game")]
    public float GameDuration = 120;
    public int MinPlayerCount = 2;

    [Header("Player")]
    public float PlayerMovementSpeed = 5;

    [Header("Attack")]
    public float HitboxDuration = 0.2f;
    public float HitCooldown = 0.5f;

    [Header("Knockback")]
    public float StaggeredDuration = 0.3f;
    public float KnockbackStrength = 0.2f;
    public float KnockbackMoveSpeed = 0.1f;
}
