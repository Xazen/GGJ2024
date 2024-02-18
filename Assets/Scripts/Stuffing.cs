using Configs;
using Constants;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

public class Stuffing : MonoBehaviour
{
    [SerializeField] private GameObject modelContainer;

    public int PlayerIndex;
    private PlayerModelConfig _playerModelConfig;
    private DiContainer _diContainer;

    [Inject]
    [UsedImplicitly]
    private void Inject(PlayerModelConfig playerModelConfig, DiContainer diContainer)
    {
        _diContainer = diContainer;
        _playerModelConfig = playerModelConfig;
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            var player = other.gameObject.GetComponent<GamePlayerActorController>();
            PlayerIndex = player.PlayerIndex;
            Debug.Log("Stuffing changed to PlayerIndex: " + PlayerIndex);
        }
    }

    public void InitWithPlayerIndex(int playerIndex)
    {
        _diContainer.InstantiatePrefab(_playerModelConfig.PlayerStuffModelByIndex[playerIndex], modelContainer.transform);
    }
}
