using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject projectile;
    public List<ProjectileModifier> modifiers;

    private const float rateDefault = 0.5f;
    private float rate;
    private float rateTimer = 0f;
    private bool recharge = false;

    private void Awake() 
    {
        UpdateRate();    
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !recharge)
        {
            Shoot();
            recharge = true;
        }

        if (recharge)
        {
            rateTimer += Time.deltaTime;
            if (rateTimer >= rate)
            {
                rateTimer = 0;
                recharge = false;
            }
        }
    }

    void Shoot()
    {
        var proj = Instantiate(projectile, transform.position + (transform.forward * 2), transform.rotation);
        proj.GetComponent<Projectile>().modify(modifiers);
        proj.GetComponent<Projectile>().send(transform.forward);
    }

    void UpdateRate()
    {
        rate = rateDefault;
        foreach (var modifier in modifiers)
        {
            rate *= modifier.rateMult;
            rate += modifier.rateAdd;
        }
    }
}
