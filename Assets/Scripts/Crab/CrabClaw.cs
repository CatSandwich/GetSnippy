using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabClaw : MonoBehaviour
{
    [SerializeField]
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CrabEye hitEye = collision.gameObject.GetComponent<CrabEye>();

        if (hitEye != null && hitEye.GetPlayer() != player)
        {
            hitEye.Die();
        }
    }
}
