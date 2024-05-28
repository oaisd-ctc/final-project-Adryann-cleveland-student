using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public HealthSystem playerHealth;
    public float Damage;
    public Player player;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            
            HealthSystem.Instance.TakeDamage(Damage);
        }

    }
    
}
