using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Medium : MonoBehaviour
{
    public float _speed;
    public float _stoppingDistance = 5.5f;
    public float _maxStopDistance = 0f;
    public float _radius;
    public int _turn = 0;

    private SoundManager _soundManager;
    private GameObject[] _allies;
    public GameObject _laserPrefab;
    public GameObject _explosion;
    private Shake _shake;
    private Ui_Manager Ui_Manager;
    public Transform _player;

// Start is called before the first frame update
    void Start()
    {
// Inicialization of variables
        _shake = GameObject.Find("MainCamera").GetComponent<Shake>();
        _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        Ui_Manager = GameObject.Find("Canvas").GetComponent<Ui_Manager>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        Attack();
        levelDown();
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();        
    }
// Attack function
    private void Attack(){
// Spawn laser at enemy position
        Instantiate(_laserPrefab, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
        _soundManager.PlaySound("enemyFire");
        StartCoroutine(attackCd());
    }
// Movement function
    private void Movement(){
// Move towards player if in range
        if(transform.position.y > _stoppingDistance + 0.6f && _player != null){
            if(isEven(_turn)){
                transform.position = Vector2.MoveTowards(transform.position, _player.position, _speed * Time.deltaTime);
            }
        }
// Distances from allies when above range
        else if(transform.position.y > _stoppingDistance){
            distanceFromAllies();
        }
// Stop at max distance
        else if(transform.position.y < _stoppingDistance){ 
            transform.position = new Vector3(transform.position.x, _stoppingDistance, transform.position.z);
        }
    }
// Decrease stopping distance
    public void levelDown(){
        _stoppingDistance -= 0.75f;
        _turn ++;
        StartCoroutine(levelCd());
    }
// Cooldown between leving down
    public IEnumerator levelCd(){
        yield return new WaitForSeconds(4f);
        if(_stoppingDistance > _maxStopDistance){
            levelDown();    
        }
    }
// Attack cooldown & Repeat
    public IEnumerator attackCd(){
        yield return new WaitForSeconds(2f);
        Attack();
    }
// Function to distance from allies
    public void distanceFromAllies(){
// Find all allies (Enemy_Medium)
        _allies = GameObject.FindGameObjectsWithTag("Enemy_Medium");
// Foreach ally in range, distance from it
        foreach (var ally in _allies)
        {
            if(Vector3.Distance(transform.position, ally.transform.position) < _radius){
                transform.position = Vector2.MoveTowards(transform.position, ally.transform.position, -3 * Time.deltaTime);
            }
        }
    }
// Find if even function
    static bool isEven(int i){
        return i%2 == 0;
    }
// OnTriggerEnter function
    private void OnTriggerEnter2D(Collider2D other){
// If player update score and player hp or shield and destroy self
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
// If laser update score and destroy self
        else if(other.tag == "Laser"){
            _shake.CamShake();
            _soundManager.PlaySound("explosion");
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Ui_Manager.UpdateScore();
        }
    }
}
