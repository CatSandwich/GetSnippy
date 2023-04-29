using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabEye : MonoBehaviour
{
    [SerializeField]
    private Player player;

    public Player GetPlayer()
    {
        return player;
    }

    public void Die()
    {
        transform.eulerAngles = new Vector3(90, 0, 0);
        // TODO
    }
}
