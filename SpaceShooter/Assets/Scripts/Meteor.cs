using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float _speed = 5f;
    public GameObject _warningPrefab;

    void Start(){
        _warningPrefab = Instantiate(_warningPrefab, transform.position, Quaternion.identity);
        _warningPrefab.SetActive(false);    
    }

// Update is called once per frame
    void Update()
    {
        SetWarning();
        Movement();
    }
//Sets meteor warning where it will appear
    private void SetWarning(){
        if(transform.position.y > 8.3f){
            _warningPrefab.gameObject.transform.position = new Vector3(transform.position.x, 4.5f, transform.position.z);
            _warningPrefab.SetActive(true);
        }else if(transform.position.y < 8.3f){
            _warningPrefab.SetActive(false);
        }
    }
    private void Movement(){
// Descending movement
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

// Respawn after out of view
        if(transform.position.y < -6.7f){
            float randomX = Random.Range(-9.8f , 9.8f);
            float randomY = Random.Range(20f , 22f);
            transform.position = new Vector3(randomX, randomY, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            Player player = other.GetComponent<Player>();
            player.EnemyHit();
        }
    }
    void OnDestroy(){
        Destroy(_warningPrefab);
    }
}
