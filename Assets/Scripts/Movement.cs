// GameDev.tv ChallengeClub.Got questionsor wantto shareyour niftysolution?
// Head over to - http://community.gamedev.tv

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] public bool isActiveBool = false;
    [SerializeField] public float DIAGONAL_SCALE = 1.4f;
    private Vector2 moveDirection;
    private SpriteRenderer mySpriteRenderer;
    private Rigidbody2D rb;

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
}
