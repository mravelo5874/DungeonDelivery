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

    // randomizer options
    private float[] rangeMultOpts = { 0.5f, 1f, 2f };
    private float[] rangeAddOpts = { 0f, 1f, 5f };
    
    private float[] rateMultOpts = { 0.25f, 0.5f, 1f };
    private float[] rateAddOpts = { 0f, 0.25f, 1f };

    private float[] damageMultOpts = { 0.5f, 1f, 2f };
    private float[] damageAddOpts = { 0f, 5f, 10f };

    private float[] sizeMultOpts = { 0.25f, 1f, 2f };
    private float[] sizeAddOpts = { 0f, 1f, 2f };

    private float[] speedMultOpts = { 0.25f, 1f, 1.5f };
    private float[] speedAddOpts = { 0f, 2f, 4f };

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
        
        // randomize 2 modifiers
        if (GameManager.instance.devMode)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                modifiers.Clear();

                ProjectileModifier mod1 = new ProjectileModifier();
                mod1.rangeMult = rangeMultOpts[Random.Range(0, 3)];
                mod1.rangeAdd = rangeAddOpts[Random.Range(0, 3)];
                mod1.rateMult = rateMultOpts[Random.Range(0, 3)];
                mod1.rateAdd = rateAddOpts[Random.Range(0, 3)];
                mod1.damageMult = damageMultOpts[Random.Range(0, 3)];
                mod1.damageAdd = damageAddOpts[Random.Range(0, 3)];
                mod1.sizeMult = sizeMultOpts[Random.Range(0, 3)];
                mod1.sizeAdd = sizeAddOpts[Random.Range(0, 3)];
                mod1.speedMult = speedMultOpts[Random.Range(0, 3)];
                mod1.speedAdd = speedAddOpts[Random.Range(0, 3)];

                ProjectileModifier mod2 = new ProjectileModifier();
                mod2.rangeMult = rangeMultOpts[Random.Range(0, 3)];
                mod2.rangeAdd = rangeAddOpts[Random.Range(0, 3)];
                mod2.rateMult = rateMultOpts[Random.Range(0, 3)];
                mod2.rateAdd = rateAddOpts[Random.Range(0, 3)];
                mod2.damageMult = damageMultOpts[Random.Range(0, 3)];
                mod2.damageAdd = damageAddOpts[Random.Range(0, 3)];
                mod2.sizeMult = sizeMultOpts[Random.Range(0, 3)];
                mod2.sizeAdd = sizeAddOpts[Random.Range(0, 3)];
                mod2.speedMult = speedMultOpts[Random.Range(0, 3)];
                mod2.speedAdd = speedAddOpts[Random.Range(0, 3)];

                modifiers.Add(mod1);
                modifiers.Add(mod2);

                UpdateRate();
                rateTimer = 0f;
                recharge = false;
                
                print("2 modifiers randomized!");
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
