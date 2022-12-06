using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    [SerializeField]
    private float damageAmmount;
    float damage;

    void Start()
    {
        damage = damageAmmount;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Melee Hit");
        other.gameObject.GetComponentInParent<HeroKnight>().takeDamage(damage);
    }
}
