using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileParent : MonoBehaviour
{
    public static ProjectileParent instance;
    public int maxProjectiles;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        while (transform.childCount >= maxProjectiles)
        {
            Destroy(GetComponent<Transform>().GetChild(0).gameObject);
        }
    }

    public void add(GameObject projectile)
    {
        if (transform.childCount >= maxProjectiles)
        {
            Destroy(projectile);
            return;
        }

        projectile.transform.parent = transform;
    }
}
