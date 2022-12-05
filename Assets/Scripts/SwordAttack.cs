using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
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
        other.gameObject.GetComponent<EnemyController>().takeDamage(damage);
    }
}
