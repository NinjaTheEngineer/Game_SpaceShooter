using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip fireSound, explosionSound, playerHitSound, enemyFireSound, pickPowerSound;

    [SerializeField]
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.Find("SoundManager").GetComponent<AudioSource>();
    }
    
    //Play audio by name
    public void PlaySound(string clip){
        switch(clip){
            case "fire":
                AudioSource.PlayClipAtPoint(fireSound, Vector3.zero);
                break;
            case "explosion":
                AudioSource.PlayClipAtPoint(explosionSound, Vector3.zero);
                break;
            case "playerDamage":
                AudioSource.PlayClipAtPoint(playerHitSound, Vector3.zero);
                break;
            case "enemyFire":
                AudioSource.PlayClipAtPoint(enemyFireSound, Vector3.zero);
                break;
            case "powerUp":
                AudioSource.PlayClipAtPoint(pickPowerSound, Vector3.zero);
                break;
        }
    }
}
