using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundAffect : MonoBehaviour {
    [SerializeField]
    AudioSource laso;
    [SerializeField]
    AudioSource footsteps;
    [SerializeField]
    AudioSource death;
    [SerializeField]
    AudioSource latch;
    public   void PlaySound(string sound)
    {
        if (sound == "laso")
        {
            laso.Play();
        }
        if (sound == "steps"&&footsteps.isPlaying==false)
        {
            footsteps.Play();
        }
        if (sound == "death")
        {
            death.Play();
        }
        if (sound == "latch")
        {
            laso.Stop();
            latch.Play();
        }
    }
    public void StopFootSteps()
    {

        footsteps.Stop();
    }
}
