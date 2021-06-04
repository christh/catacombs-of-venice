using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

namespace IR
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState
        {
            PRE_GAME,
            RUNNING,
            PAUSED
        }

        public GameObject[] SystemPrefabs;
        public Events.EventGameState OnGameStateChanged;
        public Events.EventGameUIUpdate OnGameUIChanged;
        public Events.EventEnemyKilled OnEnemyKilledChanged;

        private List<GameObject> instantiatedSystemPrefabs;
        private string currentLevel;

        private List<AsyncOperation> loadOperations;
        private int goldCount = 0;
        private int enemiesKilledCount = 0;
        private int playerHealth = 0;

        [SerializeField] string firstLevel = "Level 1";

        internal void RestartGame()
        {
            UpdateState(GameState.PRE_GAME);
        }

        internal void AddGold(int value)
        {
            goldCount += value;
            // redraw UI
            OnGameUIChanged.Invoke();
        }


        internal void AddEnemyKillCount(int value)
        {
            enemiesKilledCount += value;
            // redraw UI
            OnGameUIChanged.Invoke();
        }

        internal void UpdateHealth(int value)
        {
            playerHealth = value;
            // redraw UI
            OnGameUIChanged.Invoke();
        }

        internal void QuitGame()
        {
            // TODO: implement quit / tidyup stuff here
            Application.Quit();
        }

        public GameState CurrentGameState { get; private set; } = GameState.PRE_GAME;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            instantiatedSystemPrefabs = new List<GameObject>();
            loadOperations = new List<AsyncOperation>();
            currentLevel = string.Empty;

            InstantiateSystemPrefabs();

            UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
        }

        private void HandleMainMenuFadeComplete(FadeTypes fadeType)
        {
            Debug.Log($"HandleMainMenuFadeComplete: {fadeType}");
            if (fadeType == FadeTypes.FADE_IN)
            {
                Debug.Log("HandleMainMenuFadeComplete - Unloading level");
                UnloadLevel(currentLevel);
            }
        }
        public void GoToFloor(string level)
        {
            UnloadLevel(currentLevel);
            LoadLevel(level);
        }
        private void Update()
        {
            if (CurrentGameState == GameManager.GameState.PRE_GAME)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        void InstantiateSystemPrefabs()
        {
            GameObject prefabInstance;

            foreach (var item in SystemPrefabs)
            {
                prefabInstance = Instantiate(item);
                instantiatedSystemPrefabs.Add(prefabInstance);
            }
        }

        public void LoadLevel(string level)
        {
            var operation = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);

            if (operation == null)
            {
                Debug.LogError($"[GameManager] unable to load {level}");
                return;
            }
            operation.completed += OnLoadOperationComplete;
            loadOperations.Add(operation);
            currentLevel = level;
            MusicManager.Instance.Play(level);
        }

        private void OnLoadOperationComplete(AsyncOperation ao)
        {
            if (loadOperations.Contains(ao))
            {
                loadOperations.Remove(ao);

                if (loadOperations.Count == 0)
                {
                    UpdateState(GameState.RUNNING);
                }
            }
            Debug.Log("Load complete.");
        }

        public void UnloadLevel(string level)
        {
            var operation = SceneManager.UnloadSceneAsync(level);

            if (operation == null)
            {
                Debug.LogError($"[GameManager] unable to load {level}");
                return;
            }

            operation.completed += OnUnloadOperationComplete;
        }

        private void OnUnloadOperationComplete(AsyncOperation ao)
        {
            Debug.Log("Unload complete.");
        }

        void UpdateState(GameState state)
        {
            var previousGameState = CurrentGameState;
            CurrentGameState = state;

            switch (CurrentGameState)
            {
                case GameState.PRE_GAME:
                    Time.timeScale = 1.0f;
                    break;
                case GameState.RUNNING:
                    Time.timeScale = 1.0f;
                    break;
                case GameState.PAUSED:
                    Time.timeScale = 0.0f;
                    break;
                default:

                    break;
            }

            OnGameStateChanged.Invoke(CurrentGameState, previousGameState);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var item in instantiatedSystemPrefabs)
            {
                Destroy(item);
            }

            instantiatedSystemPrefabs.Clear();
        }

        public void StartGame()
        {
            CurrentGameState = GameState.RUNNING;
            GameManager.Instance.LoadLevel(firstLevel);
        }

        public void TogglePause()
        {
            UpdateState(CurrentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
        }

        public int GetGold()
        {
            return goldCount;
        }
        public int GetEnemiesKilled()
        {
            return enemiesKilledCount;
        }

        public int GetPlayerHealth()
        {
            return playerHealth;
        }
    }
}