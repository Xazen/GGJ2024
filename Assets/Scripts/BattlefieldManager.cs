using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

public class BattlefieldManager : MonoBehaviour
{
    [SerializeField]
    public Transform[] spawnPoints;

    private BattlefieldService battlefieldService;

    [Inject]
    [UsedImplicitly]
    public void Inject(BattlefieldService battlefieldService)
    {
        battlefieldService.SetSpawnPoints(spawnPoints);
    }
}
