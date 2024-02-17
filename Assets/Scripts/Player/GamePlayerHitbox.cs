using System;
using Constants;
using UnityEngine;

public class GamePlayerHitbox : MonoBehaviour
{
    public Action<GamePlayerController> OnAttackHit;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            var playerController = other.GetComponent<GamePlayerController>();
            OnAttackHit?.Invoke(playerController);
        }
    }
}
