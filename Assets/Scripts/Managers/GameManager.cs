using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using System.Collections;

namespace CV
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState
        {
            PRE_GAME,
            RUNNING,
            PAUSED,
            ENDGAME
        }

        public GameObject[] SystemPrefabs;
        public Events.EventGameState OnGameStateChanged;
        public Events.EventGameUIUpdate OnGameUIChanged;
        public Events.EventEnemyKilled OnEnemyKilledChanged;

        private List<GameObject> instantiatedSystemPrefabs;
        private string currentLevel;

        private int goldCount = 0;
        private int enemiesKilledCount = 0;
        private int playerHealth = 0;

        [SerializeField] string firstLevel = "Level 1";

        bool levelLoadInProgress = false;

        //internal void RestartGame()
        //{
        //    SceneManager.UnloadSceneAsync(currentLevel);
        //    SceneManager.LoadScene(firstLevel);
        //}

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
            if (levelLoadInProgress)
            {
                Debug.LogWarning($"Couldn't load {level} - level load already in progress.");
                return;
            }
            levelLoadInProgress = true;

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
            StartCoroutine(StartLoadLevel(level));
        }

        IEnumerator StartLoadLevel(string level)
        {
            var ao = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);

            ao.allowSceneActivation = false;
            while (ao.progress < 0.9f)
            {
                yield return null;
            }
            ao.allowSceneActivation = true;

            while (!ao.isDone)
            {
                yield return null;
            }

            //if (ao == null)
            //{
            //    Debug.LogError($"[GameManager] unable to load {level}");
            //}
            //else
            {
                UpdateState(GameState.RUNNING);
                currentLevel = level;
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLevel));
                levelLoadInProgress = false;
                MusicManager.Instance.Play(level);
                Debug.Log("Load complete.");
            }
        }

        public void UnloadLevel(string level)
        {
            StartCoroutine(StartUnloadLevel(level));
        }

        IEnumerator StartUnloadLevel(string level)
        {
            var operation = SceneManager.UnloadSceneAsync(level);
            yield return operation;
            if (operation == null)
            {
                Debug.LogError($"[GameManager] unable to unload {level}");
            }
            else
            {
                Debug.Log("Unload complete.");
            }
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
                case GameState.ENDGAME:
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