using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerManagerController : MonoBehaviour
{
    private DiContainer container;

    [Inject]
    [UsedImplicitly]
    public void Inject(DiContainer container)
    {
        this.container = container;
    }

    [UsedImplicitly]
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.gameObject.name = "Player " + (playerInput.user.index + 1);
        container.InjectGameObject(playerInput.gameObject);
        Debug.Log("Player Joined");
    }

    [UsedImplicitly]
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player Left");
    }
}
