using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;
using DefaultNamespace;
using UnityEngine.InputSystem;

public class PlayerAudio : MonoBehaviour
{
    public enum WEAPON
    {
        Knife,
        Spoon
    }

    [Inject]
    public void Inject(AudioService audioService)
    {
        this.AudioService = audioService;
    }
    public static AudioClip[] ScreamClips = new AudioClip[4];

    public AudioClip ScreamSound;
    [SerializeField] private AudioSource src;
    [SerializeField] private int playerIndex;

    [SerializeField] private float panning;

    //Wwise Audio Input Logic
    private bool AudioInputEnabled = true;
    private AudioService AudioService;
    private List<float> buffer = new List<float>();
    private Mutex mutex = new Mutex();

    //Debug
    [SerializeField] private bool Scream = false;

    private void Start()
    {
        playerIndex = GetComponent<PlayerInput>().user.index + 1;
        src = GetComponent<AudioSource>();
        src.mute = true;
        src.loop = false;
        src.ignoreListenerVolume = true;
        AkAudioInputManager.PostAudioInputEvent("Scream_Player_" + playerIndex, gameObject, AudioSamplesDelegate, AudioFormatDelegate);
    }

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

    // Wwise callback that sends buffered samples to Wwise (“consumer thread“)
    bool AudioSamplesDelegate(uint playingID, uint channelIndex, float[] samples)
    {
        mutex.WaitOne();

        // copy samples from buffer to temporary block
        int blockSize = Mathf.Min(buffer.Count, samples.Length);
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
        audioFormat.uSampleRate = AudioService.SampleRate;
    }

    private void SetPosition()
    {
        AkSoundEngine.SetRTPCValue("Panning", transform.position.x, gameObject);
        AkSoundEngine.SetRTPCValue("Distance", transform.position.z, gameObject);

        float x = transform.position.x / 11.6f;
        float z = transform.position.z / (transform.position.z < 0 ? 3.1f : 5.2f);
        AkSoundEngine.SetRTPCValue("WoodPitchMod", Mathf.Abs(x) * (Mathf.Abs(z) * 0.5f + 0.5f), gameObject);
        Debug.Log(x + "\t" + z + "\t" + Mathf.Abs(x) * (Mathf.Abs(z) * 0.5f + 0.5f));
    }

    public void SetWeapon(WEAPON weapon)
    {
        AkSoundEngine.SetSwitch("Weapon", weapon.ToString(), gameObject);
    }

    bool firsttime = true;
    public void PlayScream()
    {
        if(firsttime)
        {
            //AkAudioInputManager.PostAudioInputEvent("Scream_Player_" + playerIndex, gameObject, AudioSamplesDelegate, AudioFormatDelegate);
            firsttime = false;
        }
        
        //if(ScreamClips[playerIndex] != null)
        //{
        //    ScreamSound = ScreamClips[playerIndex - 1];
        //}
        src.clip = ScreamSound;
        src.mute = false;
        src.Play();
        AudioService.Scream();
    }

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("StopAudioInput", gameObject);
        AudioInputEnabled = false;
    }

    private void Update()
    {
        SetPosition();
    }
}
