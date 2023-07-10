using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public string CauseOfDeath = "";

    
    public float HumanMovementSpeed = 0f;
    [SerializeField] AnimationCurve _humanMovementSpeedCurve;
    [SerializeField] float _humanSpeedupFactor = 0f;
    [SerializeField] float _startingHumanMovementSpeed = 3f;

    // the time at which we should be speeding up at the fastest rate
    [SerializeField] float _timeOfMaxSpeedUp = 40f;
    [HideInInspector] public float PercentageToMaxDifficulty = 0f;
    float _gameplayStartTime = 0f;

    [SerializeField] int _countdownLength;

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
        AudioManager.Instance?.Play("MainTheme");
    }

    // Update is called once per frame
    void Update()
    {
        if (_GameState == GameState.Gameplay)
        {
            float timeSinceGameplayStart = Time.time - _gameplayStartTime;
            PercentageToMaxDifficulty = Mathf.Clamp01(timeSinceGameplayStart/_timeOfMaxSpeedUp);
            _humanSpeedupFactor = _humanMovementSpeedCurve.Evaluate(Mathf.Clamp01(timeSinceGameplayStart/_timeOfMaxSpeedUp));
            HumanMovementSpeed += Time.deltaTime * _humanSpeedupFactor;
        }
    }

    void SetGameState(GameState newGameState)
    {
        if (_GameState == newGameState)
        {
            return;
        }
        _GameState = newGameState;
        if (_GameState == GameState.Gameplay)
        {
            HumanMovementSpeed = _startingHumanMovementSpeed;
            _gameplayStartTime = Time.time;          
        }
        OnGameStateChanged?.Invoke(_GameState);
    }

    public void OnHumanSoothed(Human soothedHuman)
    {
        Score += 1;
        soothedHuman.BecomeHappy();
        OnScoreChanged?.Invoke(Score);
    }

    public void OnUnhappyHuman(Human unhappyHuman)
    {
        if (_GameState != GameState.Gameplay)
        {
            return;
        }
        CauseOfDeath = "Someone was left unmotivated.";
        SetGameState(GameState.Endgame);
        AudioManager.Instance?.Play("Sad");
        if (FindObjectOfType<CameraShake>())
        {
            StartCoroutine(FindObjectOfType<CameraShake>().Shake(0.15f,.4f));
        }
    }

    public void OnPotholeHit()
    {
        if (_GameState != GameState.Gameplay)
        {
            return;
        }
        CauseOfDeath = "A pothole ended you.";
        SetGameState(GameState.Endgame);
        AudioManager.Instance?.Play("Fail");
        FindObjectOfType<Player>().DoFlyOffLerp();
        if (FindObjectOfType<CameraShake>())
        {
            StartCoroutine(FindObjectOfType<CameraShake>().Shake(0.15f,.3f));
        }
    }

    IEnumerator GameStartCountdown()
    {
        int count = _countdownLength;
        yield return new WaitForSeconds(0.2f);
        OnCountdownTimeChanged?.Invoke(_countdownLength);
        while (count > 0)
        {
            OnCountdownTimeChanged?.Invoke(count);
            yield return new WaitForSeconds(1f);
            count--;
        }
        OnCountdownTimeChanged?.Invoke(count);
        FindObjectOfType<Player>().DoOpeningLerp();
        SetGameState(GameState.Gameplay);

        
    }

    public void RestartGame()
    {
        AudioManager.Instance?.StopAllSounds();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        AudioManager.Instance?.StopAllSounds();
        SceneManager.LoadScene(0);
    }
}
