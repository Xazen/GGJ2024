using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerManagerController : MonoBehaviour
{
    private DiContainer container;
    private ScoreService scoreService;

    [Inject]
    [UsedImplicitly]
    public void Inject(DiContainer container, ScoreService scoreService)
    {
        this.scoreService = scoreService;
        this.container = container;
    }

    [UsedImplicitly]
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.gameObject.name = "Player " + (playerInput.user.index + 1);
        container.InjectGameObject(playerInput.gameObject);
        playerInput.gameObject.GetComponent<PlayerController>().SetInputUser(playerInput.user);
        scoreService.RegisterPlayer(playerInput.user.index);
        Debug.Log("Player Joined");
    }

    [UsedImplicitly]
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player Left");
    }
}
