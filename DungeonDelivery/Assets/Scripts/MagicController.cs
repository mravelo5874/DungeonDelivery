using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    public GameObject projectile;
    public float force = 10f;

    private void Update()
    {
        // Shoot ball with mouse click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }    
    }

    void Shoot()
    {
        var proj = Instantiate(projectile, transform.position + (transform.forward * 2), transform.rotation);
        proj.GetComponent<Rigidbody>().AddForce(transform.forward * force);
    }
}
