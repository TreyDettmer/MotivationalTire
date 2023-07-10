using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Human : MonoBehaviour
{
    private int _responseNeeded;
    public int ResponseNeeded { get => _responseNeeded; private set => _responseNeeded = value; }
    [SerializeField] float _movementSpeed;
    [SerializeField] Animator _animator;

    [SerializeField] Outline _outline;
    bool _isHappy = false;
    GameplayManager _gameplayManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameplayManager = GameplayManager.Instance;
        ResetHumanCondition();
        
        
    }

    // Update is called once per frame
    void Update()
    {

        if (_gameplayManager._GameState != GameplayManager.GameState.Pregame)
        {
            transform.position += new Vector3(-_gameplayManager.HumanMovementSpeed * Time.deltaTime,0f,0f);
        }
        if (transform.position.x < -5f)
        {
            if (!_isHappy)
            {
                _gameplayManager.OnUnhappyHuman(this);
            }
            HumanSpawner.Instance.DespawnHuman(this);
        }
    }

    public void ResetHumanCondition()
    {
        _responseNeeded = Random.Range(1,4);
        if (_responseNeeded == 1)
        {
            _outline.OutlineColor = new Color(1f,0.1411549f,0f,1f);
        }
        else if (_responseNeeded == 2)
        {
            _outline.OutlineColor = new Color(1f,0.8315386f,0f,1f);
        }
        else
        {
            _outline.OutlineColor = new Color(0f,0.4721706f,1f,1f);
        }
        _isHappy = false;
        _animator.SetBool("IsHappy",_isHappy);
    }

    public void BecomeHappy()
    {
        _isHappy = true;
        _animator.SetBool("IsHappy",_isHappy);
    }


}
