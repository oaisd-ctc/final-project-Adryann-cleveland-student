using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour
{
    public PlayerMovement weapon;
  
   
   
    public int damage;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Enemy" )
        {

            other.GetComponent<EnemyHealth>().TakeDamage(damage);
         
        }
    }
}
