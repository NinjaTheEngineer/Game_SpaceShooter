using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float _searchCountdown;
    public bool _gameOver = true;

    public Button btnScreen;
    private Ui_Manager _UImanager;

    [SerializeField]
    private GameObject _player;

// Start is called before the first frame update
    void Start()
    {
// Initialization of variables
        btnScreen = GameObject.Find("StartScreen").GetComponent<Button>();
        _UImanager = GameObject.Find("Canvas").GetComponent<Ui_Manager>();
    }

// Update is called once per frame
    void Update()
    {
// If gameover touch to start
        if(_gameOver){
            btnScreen.onClick.AddListener(startGame);
        }
    }
// Starts the game
    private void startGame(){
        if(_gameOver){
            Instantiate(_player, Vector3.zero, Quaternion.identity);
            _UImanager.HideStart();
            _gameOver = false;
        }else{
            return;
        }
    }
}