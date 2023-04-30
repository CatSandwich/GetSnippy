using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatAnimation : MonoBehaviour
{
    [SerializeField]
    Sprite Splat1_0;

    [SerializeField]
    Sprite Splat1_1;

    [SerializeField]
    Sprite Splat1_2;

    [SerializeField]
    Sprite Splat1_3;

    [SerializeField]
    Sprite Splat1_4;

    [SerializeField]
    Sprite Splat1_5;

    [SerializeField]
    Sprite Splat2_0;

    [SerializeField]
    Sprite Splat2_1;

    [SerializeField]
    Sprite Splat2_2;

    [SerializeField]
    Sprite Splat2_3;

    [SerializeField]
    Sprite Splat3_0;

    [SerializeField]
    Sprite Splat3_1;

    [SerializeField]
    Sprite Splat3_2;

    [SerializeField]
    Sprite Splat3_3;

    [SerializeField]
    Sprite Splat3_4;

    List<Sprite> sprites = new List<Sprite>();

    const float animTime = 0.1f;
    const int startLayer = 10;
    const int endLayer = 0;

    float animTimer = 0;

    int splatState = -1;

    bool done = false;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        int splatType = 0; // Random.Range(0, 2);

        if (splatType == 0)
        {
            sprites.Add(Splat1_0);
            sprites.Add(Splat1_1);
            sprites.Add(Splat1_2);
            sprites.Add(Splat1_3);
            sprites.Add(Splat1_4);
            sprites.Add(Splat1_5);
        }
        else if (splatType == 1)
        {
            sprites.Add(Splat2_0);
            sprites.Add(Splat2_1);
            sprites.Add(Splat2_2);
            sprites.Add(Splat2_3);
        }    
        else
        {
            sprites.Add(Splat3_0);
            sprites.Add(Splat3_1);
            sprites.Add(Splat3_2);
            sprites.Add(Splat3_3);
            sprites.Add(Splat3_4);
        }

        spriteRenderer.sortingOrder = startLayer;
        UpdateAnim(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!done)
        {
            if (animTimer < 0)
            {
                UpdateAnim(splatState + 1);
            }
            else
            {
                animTimer -= Time.deltaTime;
            }
        }
        
    }

    void UpdateAnim(int toState)
    {
        Debug.Log("toState: " + toState);
        Debug.Log("sprites.Count: " + sprites.Count);
        if (toState <= sprites.Count - 1)
        {
            spriteRenderer.sprite = sprites[toState];

            splatState = toState;
            animTimer = animTime;

            if (toState == sprites.Count - 1)
            {
                Debug.Log("done");
                done = true;
                spriteRenderer.sortingOrder = endLayer;
            }
        }
    }
}
