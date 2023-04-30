using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    public GameObject Title;

    public CrabBody Crab1Prefab;
    public CrabBody Crab2Prefab;

    public Vector3 Crab1SpawnPoint;
    public Vector3 Crab2SpawnPoint;

    protected CrabBody Crab1;
    protected CrabBody Crab2;

    private readonly PlayerInput _player1 = PlayerInput.Player1;
    private readonly PlayerInput _player2 = PlayerInput.Player2;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Instead, start in a title screen.
        // Then when player presses a button, trigger the wave to go up.
        // When the wave covers the screen, remove the title.
        // When the wave comes down, spawn the crabs offscreen and make them walk on.
        // Once they are in position, switch the crabs to controllable.

        ShowTitle();
    }

    void Update()
    {
        _player1.Update();
        _player2.Update();
    }

    void OnGameStart()
    {
        Debug.LogError("Game start");
        StartCoroutine(OnGameStartCoroutine());
    }

    void OnGameEnd()
    {
        Debug.LogError("Game end");
        StartCoroutine(OnGameEndCoroutine());
    }

    IEnumerator OnGameStartCoroutine()
    {
        HideTitle();
        SpawnCrabs();
        yield break;
    }

    IEnumerator OnGameEndCoroutine()
    {
        ShowTitle();
        DespawnCrabs();
        yield break;
    }

    void SpawnCrabs()
    {
        Debug.Log("Spawn crabs");
        Assert.IsTrue(!Crab1 && !Crab2);

        Crab1 = Instantiate(Crab1Prefab, Crab1SpawnPoint, transform.rotation);
        Crab2 = Instantiate(Crab2Prefab, Crab2SpawnPoint, transform.rotation);

        Crab1.Died += OnGameEnd;
        Crab2.Died += OnGameEnd;
    }

    void DespawnCrabs()
    {
        Debug.Log("Despawn crabs");
        Assert.IsTrue(Crab1 && Crab2);
        
        Crab1.Died -= OnGameEnd;
        Crab2.Died -= OnGameEnd;

        Destroy(Crab1);
        Destroy(Crab2);

        // TODO: Clean up severed eye stalks.
    }

    void ShowTitle()
    {
        Debug.Log("Show title");
        Title.SetActive(true);
        _player1.AnyInput += OnGameStart;
        _player2.AnyInput += OnGameStart;
    }

    void HideTitle()
    {
        Debug.Log("Hide title");
        Title.SetActive(false);
        _player1.AnyInput -= OnGameStart;
        _player2.AnyInput -= OnGameStart;
    }
}
