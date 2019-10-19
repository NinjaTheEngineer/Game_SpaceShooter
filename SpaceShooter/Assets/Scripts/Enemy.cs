using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _speed = 2f;

    [SerializeField]
    private GameObject _explosion;
    
    private SoundManager _soundManager;
    private Shake _shake;
    private Ui_Manager Ui_Manager;

// Start is called before the first frame update
    void Start()
    {
        _shake = GameObject.Find("MainCamera").GetComponent<Shake>();
        _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        Ui_Manager = GameObject.Find("Canvas").GetComponent<Ui_Manager>();
    }

// Update is called once per frame
    void Update()
    {
        Movement();
    }

// Movement Function
private void Movement(){
    transform.Translate(Vector3.down * _speed * Time.deltaTime);
// if below screen, spawn at random position above screen
        if(transform.position.y < -6.16f){
            float randomX = Random.Range(-8f, 8f);
            float randomY = Random.Range(6.3f, 8f);
            transform.position = new Vector3(randomX, randomY, 0);
        }
}

// OnTriggerFunction
    private void OnTriggerEnter2D(Collider2D other){
       
        if(other.tag == "Player"){
            Player player = other.GetComponent<Player>();

            if(player != null){
                _soundManager.PlaySound("explosion");
                Instantiate(_explosion, transform.position, Quaternion.identity);
                player.EnemyHit();
                Ui_Manager.UpdateScore();
                Destroy(gameObject);
            } 
        }

        else if(other.tag == "Laser"){
            _shake.CamShake();
            _soundManager.PlaySound("explosion");
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Ui_Manager.UpdateScore();
        }
    }
}
