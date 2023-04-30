using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabEye : MonoBehaviour
{
    [SerializeField]
    private CrabBody crabBody;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private GameObject splatPrefab;

    [SerializeField]
    private GameObject eyePrefab;

    [SerializeField]
    private Player player;

    bool isDead = false;

    public event Action Snipped;

    public Player GetPlayer()
    {
        return player;
    }

    private void Update()
    {
        //GetSnipped();
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

            GameObject newSplat = Instantiate(splatPrefab, spawnPoint.position, spawnPoint.rotation);
            GameObject newEye = Instantiate(eyePrefab, spawnPoint.position, spawnPoint.rotation);

            Snipped?.Invoke();
        }
    }
}
