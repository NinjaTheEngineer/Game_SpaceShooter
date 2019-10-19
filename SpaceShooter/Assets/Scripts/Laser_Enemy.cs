using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Enemy : MonoBehaviour
{
    public float _speed = 8.0f;

    // Update is called once per frame
    void Update()
    {
// Movement
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
// if below screen destroy self
        if(transform.position.y < -5.52f){
            Destroy(gameObject);
        }
    }
// OnTriggerEnter with player, destroy self and playerHit
    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            Player player = other.GetComponent<Player>();
            if(player != null){
                player.EnemyHit();
            }
            Destroy(gameObject);
        }
    }
}
