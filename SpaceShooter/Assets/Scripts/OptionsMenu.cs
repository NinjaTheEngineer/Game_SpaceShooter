using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer; 

    //Set background volume
    public void SetVolume(float volume){
        audioMixer.SetFloat("volume", volume);
    }
}
