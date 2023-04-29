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

    private CrabDirection crabDirection;
    private bool isNeutral = true;

    private CrabBody crabBody;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        crabBody = GetComponent<CrabBody>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        crabBody.ChangeDirection += OnChangeDirection;
        crabBody.Move += OnMove;
    }

    void OnChangeDirection(CrabDirection direction)
    {
        crabDirection = direction;
        isNeutral = true;

        SetSprite();
    }

    void OnMove()
    {
        isNeutral = !isNeutral;
        SetSprite();
    }

    void SetSprite()
    {
        if (!isNeutral)
        {
            if (crabDirection == CrabDirection.Left)
            {
                spriteRenderer.sprite = leftSprite;
                return;
            }
            else if (crabDirection == CrabDirection.Right)
            {
                spriteRenderer.sprite = rightSprite;
                return;
            }
        }

        spriteRenderer.sprite = neutralSprite;
    }
}
