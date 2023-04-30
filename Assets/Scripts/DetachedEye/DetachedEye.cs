using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachedEye : MonoBehaviour
{
    [SerializeField]
    Animation anim;

    const int startLayer = 10;
    const int endLayer = 0;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = startLayer;
    }

    // Update is called once per frame
    void Update()
    {
        if (!anim.isPlaying)
        {
            spriteRenderer.sortingOrder = endLayer;
            gameObject.SetActive(false);
        }
    }
}
