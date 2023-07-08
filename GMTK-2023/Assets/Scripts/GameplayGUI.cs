using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayGUI : MonoBehaviour
{

    GameplayManager _gameplayManager;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _countdownText;
    [SerializeField] GameObject _gameOverMenu;
    // Start is called before the first frame update
    void Start()
    {
        _gameplayManager = GameplayManager.Instance;
        _gameplayManager.OnScoreChanged += OnScoreChanged;
        _gameplayManager.OnGameStateChanged += OnGameStateChanged;
        _gameplayManager.OnCountdownTimeChanged += OnCountdownTimeChanged;
        _gameOverMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnScoreChanged(int newScoreValue)
    {
        _scoreText.text = $"Score: {newScoreValue.ToString()}";
    }

    public void OnCountdownTimeChanged(int newCountdownValue)
    {
        if (newCountdownValue != 0)
        {
            _countdownText.text = newCountdownValue.ToString();
        }
        else
        {
            _countdownText.text = "";
        }
        
    }

    public void OnGameStateChanged(GameplayManager.GameState newGameState)
    {
        if (newGameState == GameplayManager.GameState.Gameplay)
        {
            _scoreText.text = $"Score: 0";
        }
        if (newGameState == GameplayManager.GameState.Pregame)
        {
            
        }
        if (newGameState == GameplayManager.GameState.Endgame)
        {
            _gameOverMenu.SetActive(true);
        }
    }


}
