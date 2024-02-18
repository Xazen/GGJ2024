using System;
using Constants;
using UnityEngine;

namespace Player
{
    public class ScreamHitbox : MonoBehaviour
    {
        public event Action<Stuffing> OnStuffHit;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Stuffing))
            {
                var stuffing = other.GetComponent<Stuffing>();
                OnStuffHit?.Invoke(stuffing);
            }
        }
    }
}