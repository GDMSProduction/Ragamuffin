using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundAffect : MonoBehaviour {
    [SerializeField]
    AudioSource laso;
    [SerializeField]
    AudioSource footsteps;
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
    }
    public void StopFootSteps()
    {

        footsteps.Stop();
    }
}
