using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour
{

    public static HumanSpawner Instance;

    [SerializeField] Human[] _humanObjectPool;
    [SerializeField] Transform _spawnPoint;
    Queue<Human> _humanObjectPoolQueue = new Queue<Human>();
    GameplayManager _gameplayManager;

    [SerializeField] float _baseSpawnDelay;
    [SerializeField] float _spawnDelayVariance;
    [SerializeField] float _minSpawnDelay;
    float _currentSpawnDelay;
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
        for(int i = 0; i < _humanObjectPool.Length; i++)
        {
            Human human = _humanObjectPool[i];
            human.gameObject.SetActive(false);
            _humanObjectPoolQueue.Enqueue(human);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameplayManager._GameState != GameplayManager.GameState.Gameplay)
        {
            return;
        }
        _currentSpawnDelay = Mathf.Lerp(_minSpawnDelay,_baseSpawnDelay, 1f - _gameplayManager.PercentageToMaxDifficulty);
        if (Time.time > _timeOfNextSpawn)
        {
            SpawnHuman();

            _timeOfNextSpawn = Time.time + _currentSpawnDelay + Random.Range(-_spawnDelayVariance,_spawnDelayVariance);
        }
    }

    public void OnGameStateChanged(GameplayManager.GameState newGameState)
    {
        if (newGameState == GameplayManager.GameState.Gameplay)
        {
            _timeOfNextSpawn = Time.time + _currentSpawnDelay + Random.Range(-_spawnDelayVariance,_spawnDelayVariance);
        }
    }

    void SpawnHuman()
    {
        if (_humanObjectPoolQueue.Count == 0)
        {
            Debug.LogError("Can't spawn anymore humans!");
            return;
        }
        Human human = _humanObjectPoolQueue.Dequeue();
        human.transform.position = _spawnPoint.position;
        human.ResetHumanCondition();
        human.gameObject.SetActive(true);
    }

    public void DespawnHuman(Human human)
    {
        human.gameObject.SetActive(false);
        _humanObjectPoolQueue.Enqueue(human);
    }
}
