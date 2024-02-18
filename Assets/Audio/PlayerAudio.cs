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
        if (playerIndex is 1 or 3)
        {
            SetWeapon(WEAPON.Knife);
        }
        else
        {
            SetWeapon(WEAPON.Spoon);
        }
    }

    private void SetPosition()
    {
        AkSoundEngine.SetRTPCValue("Panning", transform.position.x, gameObject);
        AkSoundEngine.SetRTPCValue("Distance", transform.position.z, gameObject);

        float x = transform.position.x / 11.6f;
        float z = transform.position.z / (transform.position.z < 0 ? 3.1f : 5.2f);
        AkSoundEngine.SetRTPCValue("WoodPitchMod", Mathf.Abs(x) * (Mathf.Abs(z) * 0.5f + 0.5f), gameObject);
    }

    public void SetWeapon(WEAPON weapon)
    {
        AkSoundEngine.SetSwitch("Weapon", weapon.ToString(), gameObject);
    }

    public void PlayScream()
    {
        AkSoundEngine.PostEvent("Scream_Player_" + playerIndex, gameObject);
        AudioService.Scream();
    }

    public void PlayHurt()
    {
        AkSoundEngine.PostEvent("Hit_Player_" + playerIndex, gameObject);
    }

    public void PlayHit()
    {
        AkSoundEngine.PostEvent("Hits", gameObject);
    }

    private void Update()
    {
        SetPosition();
    }
}
