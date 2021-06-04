using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IR
{

    public class GameUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text GoldCount;
        [SerializeField]
        private TMP_Text EnemiesKilledCount;
        [SerializeField]
        private TMP_Text PlayerHealth;
        [SerializeField]
        private TMP_Text EndGameDucats;

        private void Start()
        {
            GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
            GameManager.Instance.OnGameUIChanged.AddListener(HandleGameUIChanged);
        }

        private void HandleGameUIChanged()
        {
            GoldCount.text = $"Thine Ducats: {GameManager.Instance.GetGold()}";
            EnemiesKilledCount.text = $"Thou hast slain enemies: {GameManager.Instance.GetEnemiesKilled()}";
            PlayerHealth.text = $"Thine Health: {GameManager.Instance.GetPlayerHealth()}";
            EndGameDucats.text = $"Thou didst procure {GameManager.Instance.GetGold()} ducats.";
        }

        private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
        {
            switch (previousState)
            {
                case GameManager.GameState.PRE_GAME:
                    if (currentState == GameManager.GameState.RUNNING)
                        Show();
                    break;
                case GameManager.GameState.RUNNING:
                case GameManager.GameState.PAUSED:
                    if (currentState == GameManager.GameState.PRE_GAME)
                    {
                        Hide();
                    }
                    break;

                default:
                    break;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}