using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAnimator : MonoBehaviour
{
    public SpriteRenderer SR;
    public Sprite Sprite1;
    public Sprite Sprite2;
    public Sprite Sprite3;
    public float Delay;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (true)
        {
            SR.sprite = Sprite1;
            yield return new WaitForSeconds(Delay);
            SR.sprite = Sprite2;
            yield return new WaitForSeconds(Delay);
            SR.sprite = Sprite3;
            yield return new WaitForSeconds(Delay);
        }
    }
}
