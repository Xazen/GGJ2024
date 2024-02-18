using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnims : MonoBehaviour
{
    public PlayerAudio playerAudio;

    private void Start()
    {
        playerAudio = transform.parent.parent.GetComponent<PlayerAudio>();
    }

    public void PlayStep()
    {
        AkSoundEngine.PostEvent("Steps", playerAudio.gameObject);
    }

    public void PlayAttack()
    {
        AkSoundEngine.PostEvent("Swing", playerAudio.gameObject);
    }

    public void PlayHit()
    {
        AkSoundEngine.PostEvent("Hits", playerAudio.gameObject);
    }
}
