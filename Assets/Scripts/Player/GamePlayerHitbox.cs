using System;
using Constants;
using UnityEngine;

public class GamePlayerHitbox : MonoBehaviour
{
    public event Action<GamePlayerActorController> OnAttackHit;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            var playerController = other.GetComponent<GamePlayerActorController>();
            OnAttackHit?.Invoke(playerController);
        }
    }
}
