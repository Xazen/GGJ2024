using UnityEngine;

[CreateAssetMenu(fileName = "Balancing", menuName = "Configs/Balancing")]
public class BalancingConfig : ScriptableObject
{
    [Header("Player")]
    public float PlayerMovementSpeed = 5;
}
