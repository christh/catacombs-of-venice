using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CV {
    public class PlayerTorch : MonoBehaviour
    {
        [SerializeField] Camera mainCamera;

        Vector3 lastMousePosition;

        Player player;

        bool mouseMode = true;
        bool controllerMode = false;

        float speed = 10f;

        void Start()
        {
            player = FindObjectOfType<Player>();
            transform.position = player.transform.position;
        }

        void Update()
        {
            HandleMouseMove();
            HandleControllerMove();
        }

        private void HandleControllerMove()
        {
            var lookHorizontal = Input.GetAxis("Left Stick Horizontal");
            var lookVertical = Input.GetAxis("Left Stick Vertical");
            var hasAbsHorizontal = Mathf.Abs(lookHorizontal) > 0.6;
            var hasAbsVertical = Mathf.Abs(lookVertical) > 0.6;

            if (mouseMode == false && !hasAbsHorizontal && !hasAbsVertical)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                return;
            }

            if (hasAbsHorizontal || hasAbsVertical)
            {
                controllerMode = true;
                mouseMode = false;

                if (Vector2.Distance(player.transform.position, transform.position) > 3)
                {
                    return;
                }
                transform.position += new Vector3(lookHorizontal, lookVertical, 0) * speed * Time.deltaTime;
            }
        }

        private bool HandleMouseMove()
        {
            var mousePos = Input.mousePosition;

            if (mousePos != lastMousePosition)
            {
                mouseMode = true;
                controllerMode = false;
                lastMousePosition = mousePos;
                transform.position = mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);

                return true;
            }

            return false;
        }
    }
}