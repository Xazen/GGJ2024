using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManagerController : MonoBehaviour
{
    [UsedImplicitly]
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player Joined");
    }

    [UsedImplicitly]
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player Left");
    }
}
