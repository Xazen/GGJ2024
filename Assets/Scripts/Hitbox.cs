using System;
using Constants;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Action<PlayerController> OnAttackHit;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            var playerController = other.GetComponent<PlayerController>();
            OnAttackHit?.Invoke(playerController);
        }
    }
}
