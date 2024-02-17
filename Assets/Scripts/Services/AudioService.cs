using UnityEngine;

namespace DefaultNamespace
{
    public class AudioService : MonoBehaviour
    {
        [Header("Event References")]
        [SerializeField] private AK.Wwise.Event playMusic;
        [SerializeField] private AK.Wwise.Event stopMusic;
        [SerializeField] private AK.Wwise.Event playAmbience;
        [SerializeField] private AK.Wwise.Event stopAmbience;

        private void Start()
        {
            playMusic.Post(gameObject);
            playAmbience.Post(gameObject);
        }

        private void OnDestroy()
        {
            // stopMusic.Post(gameObject);
            // stopAmbience.Post(gameObject);
        }
    }
}