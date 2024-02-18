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
        [SerializeField] private bool Screaming = false;
        [SerializeField] private bool RecordScreamButton = false;

        private bool previousScreaming = false;                         //DEBUG

        private float timer = 1.2f;
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

            //IsCapturing = true;

            // start Wwise “consumer thread“
            //AkAudioInputManager.PostAudioInputEvent("StartAudioCapture_1", gameObject, AudioSamplesDelegate, AudioFormatDelegate);
        }

        //private IEnumerator WaitForRecord()
        //{
        //    yield return new WaitForSecondsRealtime(2);
        //    recordMode = false;
        //    scream = AudioClip.Create("Scream", 96000, 1, 48000, false);
        //    scream.SetData(buffer.ToArray(), 0);
        //}

        // Wwise callback that sends buffered samples to Wwise (“consumer thread“)
        bool AudioSamplesDelegate(uint playingID, uint channelIndex, float[] samples)
        {
            mutex.WaitOne();

            // copy samples from buffer to temporary block
            int blockSize = Math.Min(buffer.Count, samples.Length);
            List<float> block = buffer.GetRange(0, blockSize);
            buffer.RemoveRange(0, blockSize);

            // release ownership of mutex and buffer (release mutex as quickly as possible)
            mutex.ReleaseMutex();

            // copy samples from temporary block to output array
            block.CopyTo(samples);

            //// Return false to indicate that there is no more data to provide. This will also stop the associated event.
            return AudioInputEnabled;
        }

        // Wwise callback that specifies format of samples
        void AudioFormatDelegate(uint playingID, AkAudioFormat audioFormat)
        {
            audioFormat.channelConfig.uNumChannels = 1;
            audioFormat.uSampleRate = SampleRate;
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
            AkSoundEngine.PostEvent("StopAudioCapture", gameObject);
        }

        bool firstplay = false;
        /// <summary>
        /// Call this on the player script, that wants to scream
        /// </summary>
        public void Scream()
        {
            if(!firstplay)
            {
                AudioInputEnabled = true;
                audioState = AudioState.Play;
                AkAudioInputManager.PostAudioInputEvent("StartAudioCapture_1", gameObject, AudioSamplesDelegate, AudioFormatDelegate);
                src.loop = false;
                src.mute = false;
                src.ignoreListenerVolume = true;
                firstplay = true;
            }            

            src.Play();
            AkSoundEngine.SetState("Screaming", "Scream");
            Screaming = true;
            if (TIMER != null) StopCoroutine(TIMER);
            TIMER = StartCoroutine(Timer());
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
            //AudioInputEnabled = false;
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
            if (RecordScreamButton)
            {
                RecordScream();
                RecordScreamButton = false;
            }
        }
#endif
    }
}