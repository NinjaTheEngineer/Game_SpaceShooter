using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0.22f;
    private float _speedOn = 0.38f;

    [SerializeField]
    private float _horizontalInput;
    [SerializeField]
    private float _verticalInput;

    private float _fireRate = 0.5f;
    private float _ffRate = 0.15f;
    private float _reloaded = 0.0f;

    public bool _powerSpeed = false;
    public bool _powerShield = false;
    public bool _powerFastFire = false;

    public int _healthPoints = 3;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _shieldPrefab;

    private Ui_Manager _UImanager;
    private GameManager _gameManager;
    private SoundManager _soundManager;
    private Shake _shake;

    [SerializeField]
    private GameObject _explosion;

    private Animator _animator;
    private Rigidbody2D _playerRb;
    
    public Button btnFire;

    public VariableJoystick joystick;
    // Start is called before the first frame update
    void Start()
    {
        _shake = GameObject.Find("MainCamera").GetComponent<Shake>();
        _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _UImanager = GameObject.Find("Canvas").GetComponent<Ui_Manager>();
        if(_UImanager != null){
            _UImanager.UpdateHealth(_healthPoints);
        }

        btnFire = GameObject.FindGameObjectWithTag("Button").GetComponent<Button>();
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<VariableJoystick>();
        _shieldPrefab = Instantiate(_shieldPrefab, transform.position, Quaternion.identity);
        _shieldPrefab.transform.parent = transform;
        _shieldPrefab.SetActive(false);

        joystick.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        btnFire.onClick.AddListener(Fire);
        Movement();
    }

    void FixedUpdate(){
        _horizontalInput = joystick.Horizontal;
        _verticalInput = joystick.Vertical;

        
    }
// All movement control
    private void Movement(){
// With Speed On
        if(_powerSpeed == true){
            transform.Translate(Vector3.right * _speedOn * _horizontalInput);
            transform.Translate(Vector3.up * _speedOn * _verticalInput);
        }
// Normal Speed
        else{
            transform.Translate(Vector3.right * _speed * _horizontalInput);
            transform.Translate(Vector3.up * _speed * _verticalInput);
        }
// Player playable board Y
        if(transform.position.y < -4.75f){
            transform.position = new Vector3(transform.position.x, -4.75f, 0);
        }else if(transform.position.y > 1f){
            transform.position = new Vector3(transform.position.x, 1f, 0);
        }
// Player playable board X
        if(transform.position.x < -10.3f){
            transform.position = new Vector3(10.3f, transform.position.y, 0);
        }else if(transform.position.x > 10.3f){
            transform.position = new Vector3(-10.3f, transform.position.y, 0);
        }
    }
// Fire
    private void Fire(){
// With FastFire On
        if(_powerFastFire == true){
            if(Time.time > _reloaded){
                _soundManager.PlaySound("fire");
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                _reloaded = Time.time + _ffRate;
            }
        }
// Normal Fire
        else{
            if(Time.time > _reloaded){
                _soundManager.PlaySound("fire");
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            _reloaded = Time.time + _fireRate;
            }
        }
    }
    
// Player takes gets hit
    public void EnemyHit(){
        _shake.CamShake();
        _soundManager.PlaySound("playerDamage");
// If player has shield, he doesn't take damage but breaks the shield
        if(_powerShield){
            shieldOff();
            return;
        }
// Take 1 life away
        _healthPoints--;
// Update hp on UI
        _UImanager.UpdateHealth(_healthPoints);

// If hp is below 1 destroy the player, resets score, show start screen
        if(_healthPoints < 1){
            Instantiate(_explosion, transform.position, Quaternion.identity);
            _soundManager.PlaySound("explosion");
            _UImanager.ShowStart();
            Destroy(this.gameObject);
            _UImanager.ResetScore();
        }
    }
// Turn on Powers
    public void SpeedPowerOn(){
        _powerSpeed = true;
        StartCoroutine(SpeedCoroutine());
    }
    public void ShieldPowerOn(){        
        _powerShield = true;
        _shieldPrefab.SetActive(true);
    }
    public void FFPowerOn(){
        _powerFastFire = true;
        StartCoroutine(FFCoroutine());
    }
// Turn off Powers
    private IEnumerator SpeedCoroutine(){
        yield return new WaitForSeconds(4f);
        _powerSpeed = false;
    }
    private void shieldOff(){
        _powerShield = false;
        _shieldPrefab.SetActive(false);
    }
    private IEnumerator FFCoroutine(){
        yield return new WaitForSeconds(4f);
        _powerFastFire = false;
    }
//On player destroy set gameOver
    void OnDestroy(){
        _gameManager._gameOver = true;
    }
}
