using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    public GameObject Title;

    public GameObject Crab1Prefab;
    public GameObject Crab2Prefab;

    public Vector3 Crab1SpawnPoint;
    public Vector3 Crab2SpawnPoint;

    protected GameObject Crab1;
    protected GameObject Crab2;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Instead, start in a title screen.
        // Then when player presses a button, trigger the wave to go up.
        // When the wave covers the screen, remove the title.
        // When the wave comes down, spawn the crabs offscreen and make them walk on.
        // Once they are in position, switch the crabs to controllable.

        HideTitle();
        SpawnCrabs();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void HideTitle()
    {
        Title.SetActive(false);
    }

    void SpawnCrabs()
    {
        Assert.IsTrue(!Crab1 && !Crab2);

        Crab1 = Instantiate(Crab1Prefab, Crab1SpawnPoint, transform.rotation);
        Crab2 = Instantiate(Crab2Prefab, Crab2SpawnPoint, transform.rotation);
    }

    void DespawnCrawbs()
    {
        Assert.IsTrue(Crab1 && Crab2);

        Destroy(Crab1);
        Destroy(Crab2);

        // TODO: Clean up severed eye stalks.
    }
}
