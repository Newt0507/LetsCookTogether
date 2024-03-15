using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayingManager : MonoBehaviour
{
    public static GamePlayingManager Instance { get; private set; }

    public event EventHandler OnStageChanged;

    private enum GameState
    {
        Loading,
        Ready,
        Playing,
        GameOver,
    }

    [SerializeField] private float _loadingTimer = 1f;
    [SerializeField] private float _readyTimer = 3.95f;
    [SerializeField] private float _playingTimerMax = 1000f;

    private GameState _gameState;
    private float _playingTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        AudioManager.Instance.PlayMusic(SoundEnum.GamePlayingSound);
        _playingTimer = _playingTimerMax;
        _gameState = GameState.Loading;
    }

    private void Update()
    {
        switch (_gameState)
        {
            case GameState.Loading:
                _loadingTimer -= Time.deltaTime;
                if (_loadingTimer < 0)
                {
                    
                    _gameState = GameState.Ready;
                }

                OnStageChanged?.Invoke(this, EventArgs.Empty);
                break;

            case GameState.Ready:
                _readyTimer -= Time.deltaTime;
                if (_readyTimer < 0)
                    _gameState = GameState.Playing;

                OnStageChanged?.Invoke(this, EventArgs.Empty);
                break;

            case GameState.Playing:
                _playingTimer -= Time.deltaTime;

                if (_playingTimer < 0)
                    _gameState = GameState.GameOver;

                OnStageChanged?.Invoke(this, EventArgs.Empty);
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                //AudioListener.pause = true;
                break;
        }
    }

    public bool IsPlayingState()
    {
        return _gameState == GameState.Playing;
    }

    public bool IsReadyState()
    {
        return _gameState == GameState.Ready;
    }

    public bool IsGameOverState()
    {
        return _gameState == GameState.GameOver;
    }

    public int GetReadyTimer()
    {
        return (int)_readyTimer;
    }
    

    public int GetRemainTimer()
    {
        return (int)_playingTimer;
    }

    public float GetPlayingTimer()
    {
        return _playingTimer / _playingTimerMax;
    }
}
