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
    public float ScreamCooldown = 1.2f;

    [Header("Knockback")]
    public float StaggeredDuration = 0.3f;
    public float KnockbackStrength = 0.2f;
    public float KnockbackMoveSpeed = 0.1f;
    public float CollisionThreshold = 0.8f;

    [Header("Stuffing")]
    public int StuffingCountMin = 1;
    public int StuffingCountMax = 4;
    public float StuffingDirectionVariance = 10f;
    public float StuffingFlyDistanceMin = 0.4f;
    public float StuffingFlyDistanceMax = 0.7f;
    public float StuffingFlyDurationMin = 0.2f;
    public float StuffingFlyDurationMax = 0.5f;
    public float StuffingScaleMin = 0.8f;
    public float StuffingScaleMax = 1.2f;
}
