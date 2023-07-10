using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotholeSpawner : MonoBehaviour
{
    public static PotholeSpawner Instance;

    [SerializeField] Pothole[] _potholeObjectPool;
    [SerializeField] Transform _spawnPoint;
    Queue<Pothole> _potholeObjectPoolQueue = new Queue<Pothole>();
    GameplayManager _gameplayManager;

    [SerializeField] float _baseSpawnDelay;
    [SerializeField] float _spawnDelayVariance;
    float _timeOfNextSpawn = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _gameplayManager = GameplayManager.Instance;
        _gameplayManager.OnGameStateChanged += OnGameStateChanged;
        for(int i = 0; i < _potholeObjectPool.Length; i++)
        {
            Pothole pothole = _potholeObjectPool[i];
            pothole.gameObject.SetActive(false);
            _potholeObjectPoolQueue.Enqueue(pothole);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameplayManager._GameState != GameplayManager.GameState.Gameplay)
        {
            return;
        }
        if (Time.time > _timeOfNextSpawn)
        {
            SpawnPothole();

            _timeOfNextSpawn = Time.time + _baseSpawnDelay + Random.Range(-_spawnDelayVariance,_spawnDelayVariance);
        }
    }

    public void OnGameStateChanged(GameplayManager.GameState newGameState)
    {
        if (newGameState == GameplayManager.GameState.Gameplay)
        {
            _timeOfNextSpawn = Time.time + _baseSpawnDelay + Random.Range(-_spawnDelayVariance,_spawnDelayVariance);
        }
    }

    void SpawnPothole()
    {
        if (_potholeObjectPoolQueue.Count == 0)
        {
            Debug.LogError("Can't spawn anymore humans!");
            return;
        }
        Pothole pothole = _potholeObjectPoolQueue.Dequeue();
        pothole.transform.position = _spawnPoint.position;
        pothole.gameObject.SetActive(true);
    }

    public void DespawnPothole(Pothole pothole)
    {
        pothole.gameObject.SetActive(false);
        _potholeObjectPoolQueue.Enqueue(pothole);
    }
}
