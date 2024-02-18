using System;
using System.Linq;
using Constants;
using DefaultNamespace;
using JetBrains.Annotations;
using Player;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{
    private TimerService timerService;
    private BalancingConfig balancingConfig;
    private GamePlayerManagerController gamePlayerManagerController;
    private GameService gameService;
    private DiContainer diContainer;
    private UIService _uiService;
    private SetupPanel _setupPanel;
    private InGameHUD _inGameHUD;
    private ResultScreen _ResultScreen;

    private UIMonoBehaviour _inGameHudInstance;
    private UIMonoBehaviour _setupPanelInstance;
    private ResultScreen _resultScreen;
    private BattlefieldService _battlefieldService;
    private UIMonoBehaviour _resultScreenInstance;
    private ScoreService _scoreService;
    private AudioService _audioService;

    [Inject]
    [UsedImplicitly]
    public void Inject(TimerService timerService, BalancingConfig balancingConfig, GamePlayerManagerController gamePlayerManagerController,
        GameService gameService, DiContainer diContainer, UIService uiService, SetupPanel setupPanel, InGameHUD inGameHUD, ResultScreen resultScreen,
        BattlefieldService battlefieldService, ScoreService scoreService, AudioService audioService)
    {
        _audioService = audioService;
        _scoreService = scoreService;
        _battlefieldService = battlefieldService;
        _resultScreen = resultScreen;
        _inGameHUD = inGameHUD;
        _setupPanel = setupPanel;
        _uiService = uiService;
        this.diContainer = diContainer;
        this.gameService = gameService;
        this.gamePlayerManagerController = gamePlayerManagerController;
        this.balancingConfig = balancingConfig;
        this.timerService = timerService;

        gamePlayerManagerController.PlayerJoined += OnPlayerJoined;
    }

    private void Start()
    {
        gameService.OnRestart += OnRestart;
        _setupPanelInstance = _uiService.ShowUI(_setupPanel);
        LoadLevel((scene, _) =>
        {
            foreach (var environmentGameObject in scene.GetRootGameObjects())
            {
                diContainer.InjectGameObject(environmentGameObject);
            }
            EnablePlayerManager();
        });
    }

    private void OnRestart()
    {
        var components = FindObjectsOfType<Stuffing>();
        foreach(var stuffing in components)
        {
            Destroy(stuffing.gameObject);
        }

        _battlefieldService.ResetSpawnPositions();
        _scoreService.ResetScores();
        var players = FindObjectsOfType<GamePlayerActorController>().ToList().OrderBy(x => x.PlayerIndex);
        foreach(var player in players)
        {
            var location = _battlefieldService.GetAndRegisterFreeSpawnLocation(player.PlayerIndex);
            player.transform.position = location;
            _scoreService.RegisterPlayer(player.PlayerIndex);
        }

        foreach (var playerInput in PlayerInput.all)
        {
            playerInput.SwitchCurrentActionMap(ActionMaps.InGame);
        }

        timerService.StartTimer(balancingConfig.GameDuration);
        _uiService.HideUI(_resultScreenInstance);
        _inGameHudInstance = _uiService.ShowUI(_inGameHUD);
        _audioService.PlayMusic();
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.gameObject.GetComponent<GamePlayerInput>().OnStartInput += OnPlayerStart;
        gameService.RegisterPlayer(playerInput.user);
    }

    private void OnPlayerStart()
    {
        if (!gameService.IsGameRunning() && gameService.GetPlayerCount() >= balancingConfig.MinPlayerCount)
        {
            StartGame();
        }
    }

    private void EnablePlayerManager()
    {
        gamePlayerManagerController.AllowJoining();
    }

    private void StartGame()
    {
        foreach (var playerInput in PlayerInput.all)
        {
            playerInput.SwitchCurrentActionMap(ActionMaps.InGame);
        }

        Debug.Log("Game Started");
        timerService.OnTimerEnd += OnTimerEnd;
        timerService.StartTimer(balancingConfig.GameDuration);
        _uiService.HideUI(_setupPanelInstance);
        _inGameHudInstance = _uiService.ShowUI(_inGameHUD);
        _audioService.PlayMusic();
    }

    private void LoadLevel(UnityAction<Scene,LoadSceneMode> OnSceneLoaded)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneBuildIndex.GameEnvironment, LoadSceneMode.Additive);
    }

    private void OnTimerEnd()
    {
        _uiService.HideUI(_inGameHudInstance);
        _resultScreenInstance = _uiService.ShowUI(_resultScreen);
    }
}