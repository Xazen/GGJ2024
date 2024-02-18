using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace DefaultNamespace
{
    public class AudioService : MonoBehaviour
    {
        enum AudioState
        {
            Off,
            Record,
            Play
        }

        [Header("Event References")]
        [SerializeField] private AK.Wwise.Event playMusic;
        [SerializeField] private AK.Wwise.Event stopMusic;
        [SerializeField] private AK.Wwise.Event playAmbience;
        [SerializeField] private AK.Wwise.Event stopAmbience;

        private float ScreamTimer = 1.2f;
        private Coroutine TIMER;

        private void Start()
        {
            AkSoundEngine.SetState("Screaming", "Idle");
            playMusic.Post(gameObject);
            playAmbience.Post(gameObject);
        }

        /// <summary>
        /// Call this on the player script, that wants to scream
        /// </summary>
        public void Scream()
        {
            AkSoundEngine.SetState("Screaming", "Scream");
            if (TIMER != null) StopCoroutine(TIMER);
            TIMER = StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSecondsRealtime(ScreamTimer);
            AkSoundEngine.SetState("Screaming", "Idle");
        }
    }
}