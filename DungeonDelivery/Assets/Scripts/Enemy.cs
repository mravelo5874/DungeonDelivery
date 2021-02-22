using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public const float healthDefault = 25f;
    [HideInInspector] public float health;  

    public GameObject model;
    private bool isDead;
    public Image healthBar;

    void Awake()
    {
        health = healthDefault;
        isDead = false;
    }

    public void Damage(float num)
    {
        if (isDead) return;

        health -= num;
        if (health <= 0f)
        {
            StartCoroutine(DieAndRespawn());
        }

        healthBar.transform.localScale = new Vector3(health / healthDefault, 1f, 1f);
    }

    private IEnumerator DieAndRespawn()
    {
        isDead = true;
        model.SetActive(false);
        healthBar.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        model.SetActive(true);
        healthBar.gameObject.SetActive(true);
        health = healthDefault;
        healthBar.transform.localScale = new Vector3(1f, 1f, 1f);
        isDead = false;
    }
}
