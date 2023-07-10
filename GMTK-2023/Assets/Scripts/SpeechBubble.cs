using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeechBubble : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private RawImage _backgroundImage;

    [SerializeField] Animation _fadingAnimation;


    bool _isVisible = false;
    // Start is called before the first frame update
    void Start()
    {
        _text.color = new Color(_text.color.r,_text.color.g,_text.color.b,0f);
        _backgroundImage.color = new Color(_backgroundImage.color.r,_backgroundImage.color.g,_backgroundImage.color.b,0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTransformParent(Transform parent, float offsetX = 0.05f, float offsetY = 0.05f, float offsetZ = 0.0f)
    {
        transform.position = parent.position + new Vector3(offsetX,offsetY,offsetZ);
        transform.parent = parent;
    }

    public void SpeakInspirationally()
    {
        _text.text = InspirationalPhraseManager.Instance?.GetRandomPhrase();
        FadeIn();
    }

    public void FadeIn()
    {
        if (!_isVisible)
        {
            _isVisible = true;
            _fadingAnimation.Play("SpeechBubbleFadeIn");
            StartCoroutine(VisibilityCoroutine());
        }
        
    }

    public void FadeOut()
    {
        _fadingAnimation.Play("SpeechBubbleFadeOut");
    }

    IEnumerator VisibilityCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        FadeOut();
        _isVisible = false;

    }
}
