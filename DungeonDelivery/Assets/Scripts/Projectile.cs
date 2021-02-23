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
    private List<ProjectileModifier> mods;

    private float range;
    private float damage;
    private float size;
    private float speed;
    private int bounces;
    private int splits;
    private bool ignoreFirstCollision;

    // test
    private bool drawVectors;
    private Vector3 contactPos;
    private Vector3 velocityVector;
    private Vector3 normalVector;
    private Vector3 splitAngle1;
    private Vector3 splitAngle2;
    
    
    private Rigidbody rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        mods = new List<ProjectileModifier>();
    }

    void Update() 
    {
        //transform.localScale *= sizeMultiplier;
    }

    public void modify(List<ProjectileModifier> modifiers)
    {
        mods.Clear();
        mods.AddRange(modifiers);

        range = rangeDefault;
        damage = damageDefault;
        size = sizeDefault;
        speed = speedDefault;
        bounces = 0;
        splits = 0;

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

            bounces += modifier.numBounces;
            splits += modifier.numSplits;

            if (modifier.ignoreFirstCollision)
                ignoreFirstCollision = true;
        }

        transform.localScale = new Vector3(size, size, size);
    }

    public void modify(ProjectileModifier mod)
    {
        mods.Clear();
        mods.Add(mod);

        range = rangeDefault;
        damage = damageDefault;
        size = sizeDefault;
        speed = speedDefault;
        bounces = 0;
        splits = 0;

        range *= mod.rangeMult;
        range += mod.rangeAdd;

        damage *= mod.damageMult;
        damage += mod.damageAdd;

        size *= mod.sizeMult;
        size += mod.sizeAdd;

        speed *= mod.speedMult;
        speed += mod.speedAdd;

        bounces += mod.numBounces;
        splits += mod.numSplits;
        if (mod.ignoreFirstCollision)
            ignoreFirstCollision = true;

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
        bool hitValid = false;

        // wall layer
        if (other.gameObject.layer == GameManager.WallLayer)
        {
            hitValid = true;
        }
        // enemy layer
        else if (other.gameObject.layer == GameManager.EnemyLayer)
        {
            hitValid = true;
            other.gameObject.GetComponent<Enemy>().Damage(damage);
        }

        if (hitValid)
        {
            if (ignoreFirstCollision)
            {
                ignoreFirstCollision = false;
                return;
            }

            // calculate splits 
            if (splits > 0)
            {
                splits--;

                // get movement vector and collision normal
                var vect = rb.velocity.normalized;
                var norm = other.contacts[0].normal.normalized;

                velocityVector = vect;
                normalVector = norm;
                contactPos = other.contacts[0].point;

                // get angles
                var angle = 90 - Vector3.Angle(norm, vect);
                // print ("angle: " + angle);
                // print ("norm: " + norm);
                // print ("vect: " + vect);

                float angle1 = angle + (angle / 2);
                float angle2 = angle - (angle / 2);

                // print ("angle1: " + angle1);
                // print ("angle2: " + angle2);

                var fixedNorm = norm;
                // +Z NORM
                if (norm.z > norm.x && vect.x > 0f && vect.z > 0f)
                {
                    print ("+z norm 1");
                    angle1 *= -1;
                    angle2 *= -1;
                    fixedNorm = new Vector3(1, 0, 0);
                }
                else if (norm.z >norm.x && vect.x < 0f && vect.z > 0f)
                {
                    print ("+z norm 2");
                    fixedNorm = new Vector3(-1, 0, 0);
                }

                // -Z NORM
                else if (norm.z < norm.x && vect.x > 0f && vect.z < 0f)
                {
                    print ("-z norm 1");
                    fixedNorm = new Vector3(1, 0, 0);
                }
                else if (norm.z < norm.x && vect.x < 0f && vect.z < 0f)
                {
                    print ("-z norm 2");
                    angle1 *= -1;
                    angle2 *= -1;
                    fixedNorm = new Vector3(-1, 0, 0);
                }

                // -X NORM
                else if (norm.x < norm.z && vect.x < 0f && vect.z > 0f)
                {
                    print ("-x norm 1");
                    fixedNorm = new Vector3(0, 0, 1);
                }
                else if (norm.x < norm.z && vect.x < 0f && vect.z < 0f)
                {
                    print ("-x norm 2");
                    fixedNorm = new Vector3(0, 0, -1);
                }

                // +X NORM
                else if (norm.x > norm.z && vect.x > 0f && vect.z < 0f)
                {
                    print ("+x norm 1");
                    angle1 *= -1;
                    angle2 *= -1;
                    fixedNorm = new Vector3(0, 0, -1);
                }
                else if (norm.x > norm.z && vect.x > 0f && vect.z > 0f)
                {
                    print ("+x norm 2");
                    fixedNorm = new Vector3(0, 0, 1);
                }

                var vect1 = Quaternion.AngleAxis(angle1, transform.up);
                var vect2 = Quaternion.AngleAxis(angle2, transform.up);

                splitAngle1 = (vect1 * fixedNorm);
                splitAngle2 = (vect2 * fixedNorm);

                drawVectors = true;

                // instantiate 2 projectiles from this point
                GameObject projectile = (GameObject)Resources.Load("prefabs/projectile", typeof(GameObject));

                var proj1 = Instantiate(projectile, contactPos, Quaternion.identity);
                var proj2 = Instantiate(projectile, contactPos, Quaternion.identity);

                ProjectileModifier mod = new ProjectileModifier();
                mod.rangeMult = 1f;
                mod.damageMult = 0.5f;
                mod.sizeMult = 0.5f;
                mod.speedMult = 0.5f;
                mod.numBounces = bounces;
                mod.numSplits = splits;
                mod.ignoreFirstCollision = true;

                mods.Add(mod);

                // apply force in two vectors
                proj1.GetComponent<Projectile>().modify(mods);
                proj1.GetComponent<Projectile>().send(splitAngle1);

                proj2.GetComponent<Projectile>().modify(mods);
                proj2.GetComponent<Projectile>().send(splitAngle2);

                // destroy this projectile
                Destroy(gameObject);
                return;
            }

            // calculate bounces
            if (bounces > 0)
            {
                bounces--;
                return;
            }
        }

        Destroy(gameObject);
    }

    private bool SameSign(float num1, float num2)
    {           
        return Mathf.Sign(num1) == Mathf.Sign(num2);
    }

    private bool SignGreater(float num1, float num2)
    {           
        return Mathf.Sign(num1) > Mathf.Sign(num2);
    }

    private void OnDrawGizmos() 
    {
        if (drawVectors)
        {
            StartCoroutine(StopDrawing());

            Gizmos.color = Color.red;
            Gizmos.DrawLine(contactPos, contactPos + velocityVector);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(contactPos, contactPos + normalVector);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(contactPos, contactPos + splitAngle1);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(contactPos, contactPos + splitAngle2);
        }
    }   

    private IEnumerator StopDrawing()
    {
        yield return new WaitForSeconds(5f);
        drawVectors = false;
    }
}
