using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCombat : MonoBehaviour
{

    int comboCounter;
    float coolDownTime = 0.1f;
    float lastClicked;
    float lastComboEnd;
    Animator anim;
    Weapon currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon != null)
        {
            Attack(currentWeapon.weaponName);
        }
    }
    void Attack(string weapon)
    {
        if (Input.GetButtonDown("Q") && Time.time - lastComboEnd > coolDownTime)
        {
            comboCounter++;
            //  comboCounter = Mathf.Clamp(comboCounter, 0, currentWeapon.comboLenght);

            for (int i = 0; i < comboCounter; i++)
            {
                if (i == 0)
                {
                    if (comboCounter == i && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
                    {
                        anim.SetTrigger("Attack");
                        lastClicked = Time.time;
                    }
                }
                //   else
                //  {
                //    if(comboCounter >= (i + 1) && anim.GetCurrentAnimatorStateInfo(0).IsName()
                //   {
                //  anim.SetTrigger("Attack");
                //  lastClicked = Time.time;
                //}
                // }
            }
        }

        // for(int i =0; i < currentWeapon.comboLenght; i++)
        {
            //if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName(weapon + "Attack" + (i + 1)))
          //  {
          //      comboCounter = 0;
           //     lastComboEnd = Time.time;
           //     anim.ResetTrigger("Attack");
         //   }
            // }
        }
    }
}