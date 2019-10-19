using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState{SPAWNING, WAITING, COUNTING};
    public SpawnState state = SpawnState.COUNTING;

    public Wave[] waves;
    private int nextWave = 0;
    public Transform _meteorPrefab;
    public Transform[] _powerupsPrefab;
    public Wave currentWave;
    
    public int minEnemySize = 2;
    private float searchCountdown = 1f;
    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    private GameManager _gameManager;
    public Vector3 range;
// Draw Selection
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(this.transform.position, range * 2);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, 0.2f);
    }
// Start is called before the first frame update
    void Start()
    {
        waveCountdown = timeBetweenWaves;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

// Update is called once per frame
    void Update()
    {

        if(_gameManager._gameOver == true){
            nextWave = 0;
        }
// If waiting
        if(state == SpawnState.WAITING){
// Check if enemies are alive
            GameAlive();
            if(!EnemyIsAlive()){
// Form new wave
                WaveCompleted();
            }else{
                return;
            }

        }
// If game isn't over, and waveCountDown is done, spawn enemies
        if(_gameManager._gameOver == false){
            if(waveCountdown <= 0){
                if(state != SpawnState.SPAWNING){
                    //Start Spawning
                    currentWave = waves[nextWave];
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }else{
            waveCountdown -= Time.deltaTime;
            }
        }
                
    }
// What to do after wave is completed
    void WaveCompleted(){
        Debug.Log("Wave Completed");
// If there's a nextWave, change values
        if(nextWave < waves.Length -1){
            float nextCount = currentWave.count * 1.5f;
            nextWave = nextWave+1;
            waves[nextWave].count = (int) Mathf.Round(nextCount);
            waves[nextWave].enemy = currentWave.enemy;
            waves[nextWave].name = "Wave " + (currentWave.id + 1);
            waves[nextWave].rate = currentWave.rate * 0.85f;
            waves[nextWave].id = currentWave.id + 1;
        }

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
    }
// Check if enemies are still alive
    bool EnemyIsAlive(){

        searchCountdown -= Time.deltaTime;

        if(searchCountdown <= 0f){
            searchCountdown = 1f;
            if(GameObject.FindGameObjectsWithTag("Enemy").Length <= minEnemySize){
            return false;
            }
        }
        return true;
        
    }
    //If the player is destroyed destroys all enemies alive
    private void GameAlive(){
        searchCountdown -= Time.deltaTime;

        if(searchCountdown <= 0f){
            searchCountdown = 1f;
            if(GameObject.FindGameObjectWithTag("Player") == null){
                DestroyAll();
            }
        }
    }

    IEnumerator SpawnWave(Wave _wave){
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        if(isEven(nextWave)){
            foreach (Transform powerUp in _powerupsPrefab)
            {
                SpawnPowerUp(powerUp);
            }
        }

        if(canMeteor(nextWave) || nextWave == 0){
            SpawnMeteor(_meteorPrefab);
        }
        //Spawn
        for( int i = 0; i < _wave.count; i++){
            foreach (Transform enemy in _wave.enemy)
            {
                if(!_gameManager._gameOver){
                    SpawnEnemy(enemy);
                }/*else{
                    DestroyAlive();
                    yield return new WaitForSeconds(2f);
                }*/
            }
            yield return new WaitForSeconds(1f/_wave.rate);
        }

        state = SpawnState.WAITING;
        yield break;
    }
    private void DestroyAll(){
        DestroyEnemies();
        DestroyPowerUps();
        DestroyMeteor();
    }

    private void DestroyPowerUps(){
        foreach(GameObject pw in GameObject.FindGameObjectsWithTag("PowerUp")){
            Destroy(pw);
        }
    }

    private void DestroyMeteor(){
        foreach(GameObject m in GameObject.FindGameObjectsWithTag("Meteor")){
            Destroy(m);
        }
    }

    private void DestroyEnemies(){
        foreach (GameObject en in GameObject.FindGameObjectsWithTag("Enemy_Medium"))
        {
            Destroy(en);
        }
        foreach (GameObject enR in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enR);
        }
    }
    void SpawnMeteor(Transform _meteor){
        Vector3 powerPos = new Vector3(Random.Range(-range.x, range.x),
                                        Random.Range(11, 16), 
                                        Random.Range(0,0));
        Instantiate(_meteor, powerPos, Quaternion.identity);
    }

    void SpawnPowerUp(Transform _powerUp){
        Vector3 powerPos = new Vector3(Random.Range(-range.x, range.x),
                                        Random.Range(6, 11), 
                                        Random.Range(0,0));
        Instantiate(_powerUp, powerPos, Quaternion.identity);
    }

    void SpawnEnemy(Transform _enemy){
        //Spawn Enemy
        Vector3 enemyPos = new Vector3(Random.Range(-range.x, range.x),
                                        Random.Range(6, 11), 
                                        Random.Range(0,0));
        Instantiate(_enemy, enemyPos, Quaternion.identity);
        Debug.Log("Spawning Enemy: " + _enemy.name);
    }

    [System.Serializable]
    public class Wave{
        public string name;
        public Transform[] enemy;
        public int count;
        public float rate;
        public int id;
    }
    private void SpawnManager(){
        for(int xPos = -5; xPos < 3; xPos +=2){
            for(int yPos = 5; yPos < 25; yPos +=3){
                //Instantiate(, new Vector2(xPos, yPos), Quaternion.identity);
            }
        }
    }
    
    static bool isEven(int i){
        return i%2 == 0;
    }

    static bool canMeteor(int i){
        return i%5 == 0;
    }
}
