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

    [SerializeField] Transform _meshTransform;

    GameplayManager _gameplayManager;

    ResponsePulseEffect _responsePulseEffect;

    bool _doingOpeningLerp = false;
    [SerializeField] float _openingLerpDuration;
    float _openingLerpElapsedTime = 0f;
    Vector3 _openingLerpTargetPosition = new Vector3(-1.71f,-0.68f,0);
    Vector3 _openingLerpStartPostion = new Vector3(-5f,-0.68f,0);

    bool _doingFlyOffLerp = false;
    [SerializeField] float _flyOffLerpDuration;
    float _flyOffLerpElapsedTime = 0f;
    Vector3 _flyOffLerpTargetPosition = new Vector3(12f,0f,0f);
    float _flyOffLerpTargetScale = 9f;
    Vector3 _flyOffLerpStartPosition;

    [SerializeField] SpeechBubble _speechBubble;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _gameplayManager = GameplayManager.Instance;
        _speechBubble.SetTransformParent(transform);
        _responsePulseEffect = GetComponentInChildren<ResponsePulseEffect>();
    }

    public void DoOpeningLerp()
    {
        _doingOpeningLerp = true;
    }

    public void DoFlyOffLerp()
    {
        _rb.isKinematic = true;
        _flyOffLerpStartPosition = transform.position;
        _doingFlyOffLerp = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (_doingFlyOffLerp)
        {
            _flyOffLerpElapsedTime += Time.deltaTime;
            float percentageComplete = _flyOffLerpElapsedTime / _flyOffLerpDuration;
            transform.position = Vector3.Lerp(_flyOffLerpStartPosition,_flyOffLerpTargetPosition,percentageComplete);
            transform.localScale = new Vector3(Mathf.Lerp(1f,_flyOffLerpTargetScale,percentageComplete),Mathf.Lerp(1f,_flyOffLerpTargetScale,percentageComplete));
            if (percentageComplete >= 1)
            {
                _doingFlyOffLerp = false;
            }
            _meshTransform.Rotate(new Vector3(0f,0f,-60f * Time.deltaTime * _gameplayManager.HumanMovementSpeed * (1 + percentageComplete)),Space.World);
        }
        
        if (_gameplayManager._GameState != GameplayManager.GameState.Gameplay)
        {
            return;
        }
        GetInputs();
        _meshTransform.Rotate(new Vector3(0f,0f,-60f * Time.deltaTime * _gameplayManager.HumanMovementSpeed),Space.World);
        if (_doingOpeningLerp)
        {
            _openingLerpElapsedTime += Time.deltaTime;
            float percentageComplete = _openingLerpElapsedTime / _openingLerpDuration;
            transform.position = Vector3.Lerp(_openingLerpStartPostion,_openingLerpTargetPosition,percentageComplete);
            if (percentageComplete >= 1)
            {
                _doingOpeningLerp = false;
            }
            else
            {
                return;
            }
        }
        
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
            }
            else if (Input.GetButtonDown("Response2"))
            {
                SendResponse(2);
            }
            else if (Input.GetButtonDown("Response3"))
            {
                SendResponse(3);
            }
            _lastResponseTime = Time.time;
        }

    }

    void Jump()
    {
        if (_isGrounded)
        {
            if (AudioManager.Instance)
            {
                AudioManager.Instance?.Play("Jump");
            }
            _rb.AddForce(Vector3.up * _jumpForce,ForceMode.Impulse);
        }       
    }

    void SendResponse(int responseSent)
    {
        _responsePulseEffect.Pulse(responseSent);
        RaycastHit hit;
        bool successful = false;
        if (Physics.SphereCast(new Vector3(transform.position.x,0.8f,transform.position.z),0.57f,Vector3.forward,out hit,50,_humanLayerMask))
        {
            Human hitHuman = hit.collider.gameObject.GetComponent<Human>();
            if (hitHuman.ResponseNeeded == responseSent)
            {
                _gameplayManager.OnHumanSoothed(hitHuman);
                _speechBubble.SpeakInspirationally();
                AudioManager.Instance?.Play("Soothe");
                successful = true;
            }
            else
            {
                
            }
            
        }

        if (!successful)
        {
            AudioManager.Instance?.Play("Discomfort");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Pothole>())
        {
            _gameplayManager.OnPotholeHit();
        }
    }
}
