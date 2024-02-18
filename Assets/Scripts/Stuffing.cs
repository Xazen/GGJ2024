using System;
using Constants;
using UnityEngine;

public class Stuffing : MonoBehaviour
{
    public int PlayerIndex;

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            var player = other.gameObject.GetComponent<GamePlayerActorController>();
            PlayerIndex = player.PlayerIndex;
            Debug.Log("Stuffing changed to PlayerIndex: " + PlayerIndex);
        }
    }
}
