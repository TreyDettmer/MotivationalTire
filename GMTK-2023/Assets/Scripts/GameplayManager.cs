using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    public enum GameState
    {
        Pregame,
        Gameplay,
        Endgame
    }

    public delegate void OnIntegerChangedDelegate(int newIntValue);
    public delegate void OnGameStateChangedDelegate(GameState newGameState);
    public event OnIntegerChangedDelegate OnScoreChanged;
    public event OnIntegerChangedDelegate OnCountdownTimeChanged;
    public event OnGameStateChangedDelegate OnGameStateChanged;
    public GameState _GameState;

    public static GameplayManager Instance;

    public int Score = 0;

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
        SetGameState(GameState.Pregame);

    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameStartCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetGameState(GameState newGameState)
    {
        if (_GameState == newGameState)
        {
            return;
        }
        _GameState = newGameState;
        OnGameStateChanged?.Invoke(_GameState);
    }

    public void OnHumanSoothed(Human soothedHuman)
    {
        Score += 1;
        OnScoreChanged?.Invoke(Score);
    }

    public void OnUnhappyHuman(Human unhappyHuman)
    {
        if (_GameState != GameState.Gameplay)
        {
            return;
        }
        SetGameState(GameState.Endgame);
    }

    IEnumerator GameStartCountdown()
    {
        OnCountdownTimeChanged?.Invoke(3);
        yield return new WaitForSeconds(1f);
        OnCountdownTimeChanged?.Invoke(2);
        yield return new WaitForSeconds(1f);
        OnCountdownTimeChanged?.Invoke(1);
        yield return new WaitForSeconds(1f);
        OnCountdownTimeChanged?.Invoke(0);
        SetGameState(GameState.Gameplay);
        
    }
}
