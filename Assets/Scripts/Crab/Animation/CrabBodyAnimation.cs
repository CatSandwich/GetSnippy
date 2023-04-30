using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBodyAnimation : MonoBehaviour
{
    [SerializeField]
    private Sprite neutralSprite;

    [SerializeField]
    private Sprite leftSprite;

    [SerializeField]
    private Sprite rightSprite;

    [SerializeField]
    private Sprite forwardSprite;

    [SerializeField]
    private Sprite backwardSprite;

    [SerializeField]
    float hopTime = 0.25f;

    private CrabDirection crabDirection;
    private bool isNeutral = true;
    private float hopTimer = 0;

    private CrabBody crabBody;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        crabBody = GetComponent<CrabBody>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        crabBody.ChangeDirection += OnChangeDirection;
        crabBody.Move += OnMove;
        crabBody.Hop += OnHop;
    }

    private void Update()
    {
        if (hopTimer > 0)
        {
            hopTimer -= Time.deltaTime;
            UpdateSprite();
        }
    }

    void OnChangeDirection(CrabDirection direction)
    {
        crabDirection = direction;
        isNeutral = true;

        UpdateSprite();
    }

    void OnMove()
    {
        isNeutral = !isNeutral;
        UpdateSprite();
    }

    void OnHop(CrabDirection direction)
    {
        crabDirection = direction;

        hopTimer = hopTime;
        UpdateSprite();
    }

    void UpdateSprite()
    {
        if (crabDirection == CrabDirection.Forward)
        {
            if (hopTimer > 0)
            {
                spriteRenderer.sprite = forwardSprite;
                return;
            }
        }
        else if (crabDirection == CrabDirection.Backward)
        {
            if (hopTimer > 0)
            {
                spriteRenderer.sprite = backwardSprite;
                return;
            }
        }
        else if (crabDirection == CrabDirection.Left)
        {
            if (!isNeutral)
            {
                spriteRenderer.sprite = leftSprite;
                return;
            }
        }
        else if (crabDirection == CrabDirection.Right)
        {
            if (!isNeutral)
            {
                spriteRenderer.sprite = rightSprite;
                return;
            }
        }

        spriteRenderer.sprite = neutralSprite;
    }
}
