using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Constants
    private const float rangeDefault = 1f;
    private const float damageDefault = 1f;
    private const float sizeDefault = 0.5f;
    private const float speedDefault = 500f;

    // self
    private float range;
    private float damage;
    private float size;
    private float speed;
    
    
    private Rigidbody rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update() 
    {
        //transform.localScale *= sizeMultiplier;
    }

    public void modify(List<ProjectileModifier> modifiers)
    {
        range = rangeDefault;
        damage = damageDefault;
        size = sizeDefault;
        speed = speedDefault;

        foreach (var modifier in modifiers)
        {
            range *= modifier.rangeMult;
            range += modifier.rangeAdd;

            damage *= modifier.damageMult;
            damage += modifier.damageAdd;

            size *= modifier.sizeMult;
            size += modifier.sizeAdd;

            speed *= modifier.speedMult;
            speed += modifier.speedAdd;
        }

        transform.localScale = new Vector3(size, size, size);
    }

    public void send(Vector3 direction)
    {
        rb.AddForce(direction * speed);
        StartCoroutine(DespawnObject());
    }

    private IEnumerator DespawnObject()
    {
        yield return new WaitForSeconds(range);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.layer == 11)
        {
            Destroy(gameObject);
        }
    }
}
