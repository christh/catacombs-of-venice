using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private PauseMenu pauseMenu;
        [SerializeField] private GameUI gameUI;
        [SerializeField] private Camera dummyCamera;

        private bool sceneTransitionInProgress = false;

        public Events.EventFadeComplete OnMainMenuFadeComplete;

        private void Start()
        {
            //mainMenu.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
            GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        }

        private void HandleMainMenuFadeComplete(FadeTypes fadeType)
        {
            OnMainMenuFadeComplete.Invoke(fadeType);
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentGameState != GameManager.GameState.PRE_GAME)
                return;

            foreach (Touch touch in Input.touches)
            {

                if (touch.fingerId == 0)
                {
                    // Finger 1 is touching! (remember, we count from 0)
                }

                if (touch.fingerId == 1)
                {
                    // finger 2 is touching! Huzzah!
                }
            }

            if ((Input.GetKeyDown(KeyCode.Space) || Input.touches.Length > 0) 
                && !sceneTransitionInProgress)
            {
                sceneTransitionInProgress = true;
                Debug.Log("Space pressed.");
                GameManager.Instance.StartGame();

                mainMenu.FadeOut();
            }
        }

        private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
        {
            pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
            gameUI.gameObject.SetActive(currentState != GameManager.GameState.PRE_GAME);
        }

        public void SetDummyCameraActive(bool active)
        {
            sceneTransitionInProgress = !active;
            dummyCamera.gameObject.SetActive(active);
        }

    }
}
