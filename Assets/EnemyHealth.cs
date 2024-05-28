using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int HP = 100;
    public Animator animator;
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if( HP <=0)
        {
            //player death animation
            animator.SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
            
        }
        else
        {
            //get hit animation
            animator.SetTrigger("Damage");
        }
    }
}
