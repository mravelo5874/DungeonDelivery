using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject projectile;
    public List<ProjectileModifier> modifiers;
    public int randomModifiers;

    private const float rateDefault = 0.5f;
    private float rate;
    private float rateTimer = 0f;
    private bool recharge = false;

    // randomizer options
    private float[] rangeMultOpts = { 0.75f, 1f, 1.25f };
    private float[] rangeAddOpts = { 0f, 1f, 2f };
    
    private float[] rateMultOpts = { 0.25f, 0.5f, 1f };
    private float[] rateAddOpts = { 0f, 0.25f, 1f };

    private float[] damageMultOpts = { 0.5f, 1f, 1.5f };
    private float[] damageAddOpts = { 0f, 1f, 5f };

    private float[] sizeMultOpts = { 0.25f, 1f, 1.25f };
    private float[] sizeAddOpts = { 0f, 0.25f, 0.5f };

    private float[] speedMultOpts = { 0.25f, 1f, 1.5f };
    private float[] speedAddOpts = { 0f, 2f, 4f };

    private int[] bouncesOpts = { 0, 0, 1 };
    private int[] splitOpts = { 1, 1, 1 };


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

                for (int i = 0; i < randomModifiers; i++)
                {
                    ProjectileModifier mod = new ProjectileModifier();
                    mod.rangeMult = rangeMultOpts[Random.Range(0, 3)];
                    mod.rangeAdd = rangeAddOpts[Random.Range(0, 3)];
                    mod.rateMult = rateMultOpts[Random.Range(0, 3)];
                    mod.rateAdd = rateAddOpts[Random.Range(0, 3)];
                    mod.damageMult = damageMultOpts[Random.Range(0, 3)];
                    mod.damageAdd = damageAddOpts[Random.Range(0, 3)];
                    mod.sizeMult = sizeMultOpts[Random.Range(0, 3)];
                    mod.sizeAdd = sizeAddOpts[Random.Range(0, 3)];
                    mod.speedMult = speedMultOpts[Random.Range(0, 3)];
                    mod.speedAdd = speedAddOpts[Random.Range(0, 3)];
                    mod.numBounces = bouncesOpts[Random.Range(0, 3)];
                    mod.numSplits = splitOpts[Random.Range(0, 3)];

                    modifiers.Add(mod);
                }

                UpdateRate();
                rateTimer = 0f;
                recharge = false;
                
                print(randomModifiers + " modifiers randomized!");
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
