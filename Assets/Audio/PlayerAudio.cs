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

    [SerializeField] private int playerIndex;
    [SerializeField] private float panning;

    private AudioService AudioService;

    private void Start()
    {
        playerIndex = GetComponent<PlayerInput>().user.index + 1;
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

    public void PlayScream()
    {
        AudioService.Scream();
    }

    private void Update()
    {
        SetPosition();
    }
}
