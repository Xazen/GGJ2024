using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace DefaultNamespace
{
    [RequireComponent(typeof(AudioSource))]
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

        [Header("Debug")]
        [SerializeField] private bool RecordScreamButton = false;

        private float ScreamTimer = 1.2f;
        private Coroutine TIMER;

        public AudioMixerGroup mixer;

        public uint SampleRate = 48000;
        public uint BufferLengthInSeconds = 2;
        private AudioSource src;
        // internal buffer of samples produced by microphone in OnAudioFilterRead and consumed by Wwise in AudioSamplesDelegate
        private List<float> buffer = new List<float>();
        // synchronizes access to buffer since OnAudioFilterRead and AudioSamplesDelegate execute in different threads
        private Mutex mutex = new Mutex();
        // can be used to stop recording at runtime
        private bool AudioInputEnabled = true;
        private AudioState audioState = AudioState.Off;

        public AudioClip scream;

        private void Start()
        {
            src = GetComponent<AudioSource>();
            AkSoundEngine.SetState("Screaming", "Idle");
            playMusic.Post(gameObject);
            playAmbience.Post(gameObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerNumber">must be between 1 and 4</param>
        private void RecordScream(/*int playerNumber*/)
        {
            //if (1 > playerNumber || playerNumber > 4)
            //{
            //    Debug.LogError("playernumber MUST be between 1 and 4");
            //}

            audioState = AudioState.Record;

            //start Unity microphone recording
            
            if (Microphone.devices.Length <= 0)
            {
                //Throw a warning message at the console if there isn't    
                Debug.LogWarning("Microphone not connected!");
            }
            else
            {
                foreach (var device in Microphone.devices)
                {
                    Debug.Log("Name: " + device);
                }
            }
            src.clip = Microphone.Start(Microphone.devices[0], true, (int)BufferLengthInSeconds, (int)SampleRate);
            src.loop = false;
            src.mute = false;
            src.outputAudioMixerGroup = mixer;
            while (!(Microphone.GetPosition(null) > 0)) { }
            src.Play();
            src.ignoreListenerVolume = true;

            //StartCoroutine(WaitForRecord());
        }

        private IEnumerator WaitForRecord()
        {
            yield return new WaitForSecondsRealtime(2);
            scream = AudioClip.Create("Scream", 96000, 1, 48000, false);
            scream.SetData(buffer.ToArray(), 0);
        }

        //Unity callback on microphone input(“producer thread“)
        void OnAudioFilterRead(float[] data, int channels)
        {
            // acquire ownership of mutex and buffer
            mutex.WaitOne();

            // copy samples to buffer (de–interleave channels)
            for (int i = 0; i < data.Length; i++)
            {
                buffer.Add(data[i]);
            }
            
            // release ownership of mutex and buffer
            mutex.ReleaseMutex();
        }

        //This method can be called by other scripts to stop the callback
        public void StopCapturing()
        {
            AudioInputEnabled = false;
            src.Stop();
            Microphone.End(null);
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

#if UNITY_EDITOR
        private void Update()
        {
            if (RecordScreamButton)
            {
                RecordScream();
                RecordScreamButton = false;
            }
        }
#endif
    }
}