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
    [SerializeField]
    AudioSource buttonsound;
    [SerializeField]
    AudioSource acceptedbuttonSound;
    [SerializeField]
    AudioSource backbutton;
    public   void PlaySound(string sound)
    {
        if (sound == "laso")
        {
            laso.Play();
        }
       else if (sound == "steps"&&footsteps.isPlaying==false)
        {
            footsteps.Play();
        }
       else  if (sound == "death")
        {
            death.Play();
        }
       else  if (sound == "latch")
        {
            laso.Stop();
            latch.Play();
        }
      else  if (sound == "buttonSound")
        {
            buttonsound.Play();
        }
        else if (sound == "acceptedClick")
        {
            acceptedbuttonSound.Play();
        }
        else if (sound == "backbutton")
        {
            backbutton.Play();
        }

    }
    public void StopFootSteps()
    {

        footsteps.Stop();
    }
}
