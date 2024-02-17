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

#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] private bool Screaming = false;

         private bool previousScreaming = false;                         //DEBUG
#endif

        private void Start()
        {
            AkSoundEngine.SetState("Screaming", "Idle");
            playMusic.Post(gameObject);
            playAmbience.Post(gameObject);
        }

        private void OnDestroy()
        {
            stopMusic.Post(gameObject);
            stopAmbience.Post(gameObject);
        }


        /// <summary>
        /// Call this on the player script, that wants to scream
        /// </summary>
        public void Scream()
        {
            AkSoundEngine.SetState("Screaming", "Scream");
        }

        public void StopScream()
        {
            AkSoundEngine.SetState("Screaming", "Idle");
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (previousScreaming != Screaming)
            {
                if (Screaming)
                {
                    Scream();
                }
                else
                {
                    StopScream();
                }
            }
            previousScreaming = Screaming;
        }
#endif

    }
}