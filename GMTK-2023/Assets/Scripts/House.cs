using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] float _movementSpeed;

    GameplayManager _gameplayManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameplayManager = GameplayManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameplayManager._GameState != GameplayManager.GameState.Gameplay)
        {
            return;
        }
        transform.position += new Vector3(-_gameplayManager.HumanMovementSpeed * Time.deltaTime,0f,0f);
        if (transform.position.x < -7f)
        {
            HouseSpawner.Instance.DespawnHouse(gameObject);
        }
    }
}
