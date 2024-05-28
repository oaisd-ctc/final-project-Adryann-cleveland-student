using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{


    public string weaponName;
    public int damage;
    public EnemyHealth health;
    public Animator anim;

    public void Start()
    {
     
    }
    public void Attack()
    {
        if(Input.GetKeyDown("Q"))
        {
            anim.SetTrigger("Attack");
            health.TakeDamage(damage);
        }
    }

}
