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

    GameObject splatter;
    GameObject detachedEye;

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

            splatter = Instantiate(splatPrefab, spawnPoint.position, spawnPoint.rotation);
            detachedEye = Instantiate(eyePrefab, spawnPoint.position, spawnPoint.rotation);

            Snipped?.Invoke();
        }
    }

    void OnDestroy()
    {
        if (detachedEye != null)
        {
            Destroy(detachedEye);
        }

        if (splatter != null)
        {
            Destroy(splatter);
        }
    }
}
