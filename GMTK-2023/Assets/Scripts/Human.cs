using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Human : MonoBehaviour
{
    private int _responseNeeded;
    public int ResponseNeeded { get => _responseNeeded; private set => _responseNeeded = value; }
    [SerializeField] float _movementSpeed;
    [SerializeField] Material[] _materials;

    bool _isHappy = false;
    GameplayManager _gameplayManager;

    // Start is called before the first frame update
    void Start()
    {
        ResetHumanCondition();
        _gameplayManager = GameplayManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameplayManager._GameState != GameplayManager.GameState.Gameplay)
        {
            return;
        }
        transform.position += new Vector3(-_movementSpeed * Time.deltaTime,0f,0f);
        if (transform.position.x < -5f)
        {
            transform.position = new Vector3(5f,transform.position.y,transform.position.z);
            ResetHumanCondition();
        }
    }

    void ResetHumanCondition()
    {
        _responseNeeded = Random.Range(1,4);
        if (_materials.Length == 0)
        {
            return;
        }
        GetComponent<MeshRenderer>().material = _materials[_responseNeeded - 1];
    }

    public void BecomeHappy()
    {
        _isHappy = true;
    }


}
