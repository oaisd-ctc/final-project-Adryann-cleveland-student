using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTester : MonoBehaviour
{
    public AtrobutesManager playerAtm;
    public AtrobutesManager enemyAtm;
   private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            playerAtm.DealDamage(enemyAtm.gameObject);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            enemyAtm.DealDamage(enemyAtm.gameObject);
        }
    }
}
