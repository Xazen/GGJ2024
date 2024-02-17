using UnityEngine;
using System.Collections;

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
        public float timer;
#endif

        private void Start()
        {
            AkSoundEngine.SetState("Screaming", "Idle");
            playMusic.Post(gameObject);
            playAmbience.Post(gameObject);
        }

        private void OnDestroy()
        {
            // stopMusic.Post(gameObject);
            // stopAmbience.Post(gameObject);
        }


        /// <summary>
        /// Call this on the player script, that wants to scream
        /// </summary>
        public void Scream()
        {
            AkSoundEngine.SetState("Screaming", "Scream");
            StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSecondsRealtime(timer);
            StopScream();
        }

        public void StopScream()
        {
            AkSoundEngine.SetState("Screaming", "Idle");
            Screaming = false;
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