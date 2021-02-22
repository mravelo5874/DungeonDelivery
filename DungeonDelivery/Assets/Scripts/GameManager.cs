using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool devMode;

    public static int WallLayer = 11;
    public static int EnemyLayer = 12;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
