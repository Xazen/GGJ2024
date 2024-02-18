using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DefaultNamespace;

public class PlayerAudio : MonoBehaviour
{
    public enum WEAPON
    {
        Knife,
        Spoon
    }

    public float panning;
    private AudioService AudioService;

    public void SetWeapon(WEAPON weapon)
    {
        AkSoundEngine.SetSwitch("Weapon", weapon.ToString(), gameObject);
    }


    public void PlayStep()
    {

    }

    public void PlayAttack()
    {
        AkSoundEngine.PostEvent("Swing", gameObject);
    }

    public void PlayHit()
    {
        AkSoundEngine.PostEvent("Hits", gameObject);
    }

    public void PlayScream()
    {
        //AudioService.Scream();
    }

    [Inject]
    public void Inject(AudioService audioService)
    {
        this.AudioService = audioService;
    }
    
    private void SetPosition()
    {
        AkSoundEngine.SetRTPCValue("Panning", transform.position.x, gameObject);
        AkSoundEngine.SetRTPCValue("Distance", transform.position.z, gameObject);
    }

    private void Update()
    {
        SetPosition();
    }
}
