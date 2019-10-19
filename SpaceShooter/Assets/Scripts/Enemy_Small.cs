using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Small : MonoBehaviour
{
    public EditorPath[] Paths;
    public EditorPath PathToFollow;

    public int CurrentWayPointID = 0;
    public float speed;
    private float reachDistance = 0.1f;
    public float rotationSpeed = 7f;
    public float chance;
    public string pathName;

    public Transform _player;
    Vector3 last_position;
    Vector3 current_position;
    private SoundManager _soundManager;
    public GameObject _explosion;
    private Shake _shake;
    private Ui_Manager Ui_Manager;

    // Start is called before the first frame update
    void Start()
    {
// Variables initialization
        _shake = GameObject.Find("MainCamera").GetComponent<Shake>();
        _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        Ui_Manager = GameObject.Find("Canvas").GetComponent<Ui_Manager>();
        last_position = transform.position;

        setChanceAndPath();
    }

// Update is called once per frame
    void Update()
    {
        Movement();
    }
//Movement Function
    private void Movement(){
//Follow waypoints until there's no more, then follow player
        if(CurrentWayPointID >= PathToFollow.path_objs.Count){
            if(_player != null){
                RotateToPlayer();
                transform.position = Vector2.MoveTowards(transform.position, _player.position, speed/2 * Time.deltaTime);
            }
        }else{
            FollowPath();
        }
    
    }
// FollowPath movement function
    private void FollowPath(){
        Vector3 currentVector = PathToFollow.path_objs[CurrentWayPointID].position;

        float distance = Vector3.Distance(currentVector, transform.position);
        RotateToPoint();
        transform.position = Vector3.MoveTowards(transform.position, currentVector, Time.deltaTime * speed);

        if(distance <= reachDistance){
            CurrentWayPointID++;
        }
    }
// Rotate towards current waypoint
    private void RotateToPoint(){
        Vector3 direction = PathToFollow.path_objs[CurrentWayPointID].position - transform.position;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
// Rotate towards player
    private void RotateToPlayer(){
        Vector3 direction = _player.position - transform.position;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
// OnTriggerEnter Function
    private void OnTriggerEnter2D(Collider2D other){
       
        if(other.tag == "Player"){
            Player player = other.GetComponent<Player>();

            if(player != null){
                _soundManager.PlaySound("explosion");
                Instantiate(_explosion, transform.position, Quaternion.identity);
                player.EnemyHit();
                Ui_Manager.UpdateScore();
                Destroy(this.gameObject);
            } 
        }
        else if(other.tag == "Laser"){
            Debug.Log("Small Enemy Hit");
            _soundManager.PlaySound("explosion");
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Ui_Manager.UpdateScore();
            Destroy(this.gameObject);
            _shake.CamShake();
        }
    }
// Randomize chance of path
    void setChanceAndPath(){
        chance = Random.Range(0f, 1f);
        if(chance <= 0.5f){
            PathToFollow = Paths[0];
        }else{
            PathToFollow = Paths[1];
        }
    }
}
