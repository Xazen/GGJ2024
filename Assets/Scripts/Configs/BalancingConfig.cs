using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Balancing", menuName = "Configs/Balancing")]
public class BalancingConfig : ScriptableObject
{
    [Header("Game")]
    public float GameDuration = 120;
    public int MinPlayerCount = 2;
    public float HideScoreAtTotalSeconds = 30;
    public float FadeScoreTime = 5;

    [Header("Player")]
    public float PlayerMovementSpeed = 5;

    [Header("Attack")]
    public float HitboxDuration = 0.2f;
    public float HitCooldown = 0.5f;
    public float ScreamboxDuration = 0.2f;
    public float ScreamDuration = 1.2f;
    public float ScreamCooldown = 1.2f;
    public float ScreamFlyMin = 1.2f;
    public float ScreamFlyMax = 1.2f;
    public float ScreamDistanceMultiplier = 1f;

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

    [Header("Flow")]
    public float MidGameThreshold = 80;
    public float MidGameMoveSpeedMultiplier = 1.2f;
    public float MidGameAnimationMultiplier = .9f;
    public float MidGameStuffSpawnMultiplier = 2f;

    public float LateGameThreshold = 30;
    public float LateGameMoveSpeedMultiplier = 1.5f;
    public float LateGameAnimationMultiplier = .75f;
    public float LateGameStuffMultiplier = 3f;
}
