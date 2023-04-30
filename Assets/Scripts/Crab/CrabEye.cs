using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabEye : MonoBehaviour
{
    [SerializeField]
    private CrabBody crabBody;

    [SerializeField]
    private Player player;

    bool isDead = false;

    public event Action Snipped;

    public Player GetPlayer()
    {
        return player;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void GetSnipped()
    {
        if (!isDead)
        {
            isDead = true;

            Snipped?.Invoke();
        }
    }
}
