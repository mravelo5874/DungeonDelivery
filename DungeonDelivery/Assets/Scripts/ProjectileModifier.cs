using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileModifier
{
    // ################################
    // #    FACTORS
    // ################################

    // Range (seconds)
    public float rangeMult = 1f;
    public float rangeAdd = 0f;

    // rate (seconds)
    public float rateMult = 1f;
    public float rateAdd = 0f;

    // damage
    public float damageMult = 1f;
    public float damageAdd = 0f;

    // size
    public float sizeMult = 1f;
    public float sizeAdd = 0f;

    // speed
    public float speedMult = 1f;
    public float speedAdd = 0f;

    // ################################
    // #    INTS
    // ################################

    // bounce (bounce off walls)
    public int numBounces = 0;

    // split (splits into two projectiles when they hit a collider)
    public int numSplits = 0;

    // multishot
    public bool multishot = false;
    public int numShots = 1;

    // ################################
    // #    OTHER
    // ################################

    public bool ignoreFirstCollision = false;
}
