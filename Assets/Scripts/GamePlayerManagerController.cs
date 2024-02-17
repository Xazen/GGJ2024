using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GamePlayerManagerController : MonoBehaviour
{
    [SerializeField]
    private PlayerInputManager playerInputManager;

    public Action<PlayerInput> PlayerJoined;

    private DiContainer container;

    [Inject]
    [UsedImplicitly]
    public void Inject(DiContainer container)
    {
        this.container = container;
    }

    private void Start()
    {
        playerInputManager.DisableJoining();
        playerInputManager.onPlayerJoined += OnPlayerJoined;
        playerInputManager.onPlayerLeft += OnPlayerLeft;
    }

    public int GetPlayerCount()
    {
        return playerInputManager.playerCount;
    }

    private void OnDestroy()
    {
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
        playerInputManager.onPlayerLeft -= OnPlayerLeft;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player Joined");
        playerInput.gameObject.name = "Player " + (playerInput.user.index + 1);
        playerInput.gameObject.GetComponent<GamePlayerController>().SetInputUser(playerInput.user);
        PlayerJoined?.Invoke(playerInput);
        container.InjectGameObject(playerInput.gameObject);
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player Left");
    }

    public void AllowJoining()
    {
        playerInputManager.EnableJoining();
    }
}
