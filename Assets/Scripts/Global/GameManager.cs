using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using PlayerInput = Input.PlayerInput;

public class GameManager : MonoBehaviour
{
    public event Action GameStart;
    public event Action GameEnd;
    public event Action<CrabBody> CrabSpawn;

    public GameObject Title;

    public CrabBody Crab1Prefab;
    public CrabBody Crab2Prefab;

    private Vector3 Crab1SpawnPoint = new Vector3(-3, 0, 0);
    private Vector3 Crab2SpawnPoint = new Vector3(3, 0, 0);

    public SpriteRenderer BigWaveSR;
    public Sprite BigWave1;
    public Sprite BigWave2;
    public Sprite BigWave3;

    protected CrabBody Crab1;
    protected CrabBody Crab2;

    private bool _gameActive = false;

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
        if (!_gameActive)
        {
            if (Joystick.all.Any(j => j.stick.x.ReadValue() != 0 || j.stick.y.ReadValue() != 0))
            {
                OnGameStart();
            }
            else if (Joystick.all.Any(j => j.allControls.OfType<ButtonControl>().Any(b => b.wasPressedThisFrame)))
            {
                OnGameStart();
            }
            else if (UnityEngine.Input.anyKeyDown)
            {
                OnGameStart();
            }
        }
    }

    void OnGameStart()
    {
        StartCoroutine(OnGameStartCoroutine());
    }

    void OnGameEnd()
    {
        StartCoroutine(OnGameEndCoroutine());
    }

    IEnumerator OnGameStartCoroutine()
    {
        _gameActive = true;
        GameStart?.Invoke();
        BigWaveSR.sprite = BigWave1;
        yield return new WaitForSeconds(.5f);
        BigWaveSR.sprite = BigWave2;
        yield return new WaitForSeconds(.5f);
        BigWaveSR.sprite = BigWave3;
        HideTitle();
        SpawnCrabs();
        yield return new WaitForSeconds(.5f);
        BigWaveSR.sprite = BigWave2;
        yield return new WaitForSeconds(.5f);
        BigWaveSR.sprite = BigWave1;
        yield return new WaitForSeconds(.5f);
        BigWaveSR.sprite = null;
    }

    IEnumerator OnGameEndCoroutine()
    {
        GameEnd?.Invoke();
        BigWaveSR.sprite = BigWave1;
        yield return new WaitForSeconds(.5f);
        BigWaveSR.sprite = BigWave2;
        yield return new WaitForSeconds(.5f);
        BigWaveSR.sprite = BigWave3;
        ShowTitle();
        DespawnCrabs();
        yield return new WaitForSeconds(.5f);
        BigWaveSR.sprite = BigWave2;
        yield return new WaitForSeconds(.5f);
        BigWaveSR.sprite = BigWave1;
        yield return new WaitForSeconds(.5f);
        BigWaveSR.sprite = null;
        _gameActive = false;
    }

    void SpawnCrabs()
    {
        Assert.IsTrue(!Crab1 && !Crab2);

        Crab1 = Instantiate(Crab1Prefab, Crab1SpawnPoint, transform.rotation);
        Crab2 = Instantiate(Crab2Prefab, Crab2SpawnPoint, transform.rotation);

        Crab1.Died += OnGameEnd;
        Crab2.Died += OnGameEnd;

        CrabSpawn?.Invoke(Crab1);
        CrabSpawn?.Invoke(Crab2);
    }

    void DespawnCrabs()
    {
        Assert.IsTrue(Crab1 && Crab2);
        
        Crab1.Died -= OnGameEnd;
        Crab2.Died -= OnGameEnd;

        Destroy(Crab1.gameObject);
        Destroy(Crab2.gameObject);
    }

    void ShowTitle()
    {
        Title.SetActive(true);
    }

    void HideTitle()
    {
        Title.SetActive(false);
    }
}
