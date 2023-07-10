using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] GameObject _tireMesh;
    [SerializeField] GameObject _tireParent;
    [SerializeField] SpeechBubble _speechBubble;
    // Start is called before the first frame update
    void Start()
    {
        _speechBubble.SetTransformParent(_tireParent.transform,0.3f, 0.3f,0.0f);
        StartCoroutine(TireQuoteRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        _tireMesh.transform.Rotate(new Vector3(0f,0f,-60f * Time.deltaTime * 2f),Space.World);
    }

    IEnumerator TireQuoteRoutine()
    {
        for (int i = 0; i < 2000; i++)
        {
            _speechBubble.SpeakInspirationally();
            yield return new WaitForSeconds(3f);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
