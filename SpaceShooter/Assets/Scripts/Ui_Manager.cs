using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ui_Manager : MonoBehaviour
{
    public Image[] _lifes;
    public Text _scoreText, _bestScoreText;

    public int _score;
    public int _bestScore;
   
    public VariableJoystick _joystick;
    public GameObject _backButton;
    public GameObject _startScreen;
    public Button _pauseButton;

// Start is called before the first frame update
    void Start()
    {
// Inicialization of variables
        _joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<VariableJoystick>();
        _bestScore = PlayerPrefs.GetInt("highscore", _bestScore);
        _pauseButton.enabled = false;

        _bestScoreText.text = "HighScore: " + _bestScore;
// Set displayed lifes
        foreach(Image l in _lifes){
            l.enabled = false;
        }

// Enable joystick usage
        _joystick.enabled = false;
    }
// Updates UI lifes
    public void UpdateHealth(int currentHp){
// Remove all lifes
        foreach(Image l in _lifes){
            l.enabled = false;
        }
// Add player lifes
        for(int i = 0; i < currentHp; i++){
            _lifes[i].enabled = true;
        }
    }
// Updates score by 10
    public void UpdateScore(){
        _score += 10;
        _scoreText.text = "Score: " + _score;
// Update and save highscore
        if(_score > _bestScore){
            _bestScore = _score;
            PlayerPrefs.SetInt("highscore", _bestScore);
            _bestScoreText.text = "HighScore: " + _bestScore;
        }
    }
// Resets player score to 0
    public void ResetScore(){
        _score = 0;
        _scoreText.text = "Score: 0";
    }
// Shows start screen and back button
    public void ShowStart(){
        _startScreen.SetActive(true);
        _backButton.SetActive(true);
        _scoreText.text = "Score: 0";
    }
// Hides the start menu
    public void HideStart(){
        _startScreen.SetActive(false);
        _pauseButton.enabled = true;
        _scoreText.text = "Score: 0";
    }
// Opens previous scene
    public void backButton(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
// Pauses time
    public void pauseButton(){
        Time.timeScale = 0;

    }
// Restores time
    public void continueButton(){
        Time.timeScale = 1;
    }
}
