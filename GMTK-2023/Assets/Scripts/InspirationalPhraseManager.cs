using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspirationalPhraseManager : MonoBehaviour
{

    public static InspirationalPhraseManager Instance;
    public string[] InspirationalPhrases;

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

    public string GetRandomPhrase()
    {
        int randomIndex = Random.Range(0,InspirationalPhrases.Length);
        return InspirationalPhrases[randomIndex];
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
