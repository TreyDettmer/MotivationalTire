using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundCheckDistance;
    [SerializeField] float _jumpForce;
    Rigidbody _rb;
    [SerializeField] bool _isGrounded = false;
    [SerializeField] LayerMask _groundLayerMask;
    [SerializeField] LayerMask _humanLayerMask;
    [SerializeField] float _responseCooldown;
    float _lastResponseTime = 0f;
    [SerializeField] ParticleSystem[] _responseParticleSystems;

    GameplayManager _gameplayManager;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _gameplayManager = GameplayManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
    }

    void FixedUpdate()
    {
        GroundCheck();

    }

    void GroundCheck()
    {
        RaycastHit hit;
        Debug.DrawRay(_groundCheck.position,Vector3.down * _groundCheckDistance,Color.blue);
        if (Physics.Raycast(_groundCheck.position,Vector3.down,out hit,_groundCheckDistance,_groundLayerMask))
        {
            if (!_isGrounded)
            {
                _isGrounded = true;
            }
        }
        else
        {
            if (_isGrounded)
            {
                _isGrounded = false;
            }
        }
    }

    void GetInputs()
    {
        if (_gameplayManager._GameState != GameplayManager.GameState.Gameplay)
        {
            return;
        }
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetButtonDown("Response1") || Input.GetButtonDown("Response2") || Input.GetButtonDown("Response3"))
        {
            if (Time.time - _lastResponseTime < _responseCooldown)
            {
                return;
            }
            if (Input.GetButtonDown("Response1"))
            {
                SendResponse(1);
                Debug.Log("Response1");
            }
            else if (Input.GetButtonDown("Response2"))
            {
                SendResponse(2);
                Debug.Log("Response2");
            }
            else if (Input.GetButtonDown("Response3"))
            {
                SendResponse(3);
                Debug.Log("Response3");
            }
            _lastResponseTime = Time.time;
        }

    }

    void Jump()
    {
        if (_isGrounded)
        {
            _rb.AddForce(Vector3.up * _jumpForce,ForceMode.Impulse);
        }       
    }

    void SendResponse(int responseSent)
    {
        if (_responseParticleSystems.Length != 0)
        {
            for (int i = 0; i < _responseParticleSystems.Length; i++)
            {
                if (_responseParticleSystems[i].isPlaying)
                {
                    _responseParticleSystems[i].Stop();
                }
            }
            _responseParticleSystems[responseSent - 1].Play();
        }
        RaycastHit hit;
        if (Physics.SphereCast(transform.position,0.5f,Vector3.forward,out hit,50,_humanLayerMask))
        {
            Human hitHuman = hit.collider.gameObject.GetComponent<Human>();
            if (hitHuman.ResponseNeeded == responseSent)
            {
                Debug.Log("Good!");
                _gameplayManager.OnHumanSoothed(hitHuman);
            }
            else
            {
                Debug.Log("Bad!");
            }
            
        }
    }
}
