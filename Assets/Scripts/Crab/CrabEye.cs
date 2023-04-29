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

    public Player GetPlayer()
    {
        return player;
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            // TODO
            transform.eulerAngles = new Vector3(90, 0, 0);

            crabBody.numEyes -= 1;
        }
    }
}
