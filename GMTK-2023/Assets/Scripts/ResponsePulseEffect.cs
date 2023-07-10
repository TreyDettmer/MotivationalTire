using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponsePulseEffect : MonoBehaviour
{
    [SerializeField] float _maxSize;
    [SerializeField] float _speed;
    float _currentSize;
    bool _isPulsing = false;
    float _timeOfPulseStart = 0f;

    [SerializeField] RawImage _image;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPulsing)
        {
            _currentSize += _speed * Time.deltaTime;
            if (_currentSize > _maxSize)
            {
                _isPulsing = false;
                
                _image.transform.localScale = new Vector3(0f,0f);
            }
            else
            {
                _image.transform.localScale = new Vector3(_currentSize,_currentSize);
            }
        }
    }

    public void Pulse(int colorIndex)
    {
        _timeOfPulseStart = Time.time;
        _currentSize = 0f;
        _image.transform.localScale = new Vector3(0f,0f);
        if (colorIndex == 1)
        {
            _image.color = new Color(1f,0.1411549f,0f,1f);
        }
        else if (colorIndex == 2)
        {
            _image.color = new Color(1f,0.8315386f,0f,1f);
        }
        else
        {
            _image.color = new Color(0f,0.4721706f,1f,1f);
        }

        _isPulsing = true;
    }
}
