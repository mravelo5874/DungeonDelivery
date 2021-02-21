using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float despawnTime;
    private float sizeMultiplier = 1.005f;

    void Awake() 
    {
        StartCoroutine(DespawnObject());
    }

    void Update() 
    {
        transform.localScale *= sizeMultiplier;
    }

    private IEnumerator DespawnObject()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}
