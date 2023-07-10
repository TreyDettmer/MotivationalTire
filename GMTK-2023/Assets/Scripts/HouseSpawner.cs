using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSpawner : MonoBehaviour
{
    public static HouseSpawner Instance;

    [SerializeField] GameObject[] _housePrefabs;
    [SerializeField] Transform _spawnPoint;

    GameplayManager _gameplayManager;

    GameObject _mostRecentlySpawnedHouse = null;

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
        for (int i = 0; i < 8; i++)
        {
            SpawnHouse();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameplayManager._GameState != GameplayManager.GameState.Gameplay)
        {
            return;
        }


    }

    public void OnGameStateChanged(GameplayManager.GameState newGameState)
    {

    }

    void SpawnHouse()
    {
        int houseIndex = Random.Range(0,_housePrefabs.Length);
        Vector3 spawnPosition = _spawnPoint.position;
        if (_mostRecentlySpawnedHouse != null)
        {
            spawnPosition = _mostRecentlySpawnedHouse.transform.position + new Vector3(3f,0f,0f);
        }
        GameObject spawnedHouse = Instantiate(_housePrefabs[houseIndex],spawnPosition,Quaternion.identity);
        _mostRecentlySpawnedHouse = spawnedHouse;
    }

    public void DespawnHouse(GameObject houseObject)
    {
        Destroy(houseObject);
        SpawnHouse();
    }
}
