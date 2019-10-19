using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public float _speed = 3.5f;
    public int powerId; //0 -> Speed; 1 -> Shield; 2 -> FastFire.

    private SoundManager _soundManager;

    // Update is called once per frame
    void Update()
    {
        Movement();    
        _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    private void Movement(){
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6.6f){
            float randomX = Random.Range(-8f, 8f);
            float randomY = Random.Range(6.3f, 8f);
            transform.position = new Vector3(randomX, randomY, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            Player player = other.GetComponent<Player>();
            if(player != null){
                switch(powerId){
                case 0:
                    player.SpeedPowerOn();
                    break;
                case 1:
                    player.ShieldPowerOn();
                    break;
                case 2:
                    player.FFPowerOn();
                    break;
                }
                _soundManager.PlaySound("powerUp");
                Destroy(this.gameObject);
            }
        }
    }
}
