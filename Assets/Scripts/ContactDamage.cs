//Created by Robert Bryant
//
//Deals damage to entities on contact with the trap component
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public float damage;                            //Damage dealt on contact

    private SpikeTrapController trapController;     //Rederence to the Spike Trap Controller

    public delegate void DealDamage(float dmg, float crit);
    public static event DealDamage DamageDelt;

    // Start is called before the first frame update
    private void Start()
    {
        //Get the reference to the Spike Trap Controller
        trapController = GetComponentInParent<SpikeTrapController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Detecting a player
        if(collision.tag == "Player")
        {
            //Check if the trap is active
            if(trapController.trapActive)
            {
                DamageDelt.Invoke(damage, 0f);
            }
        }

        //Detecting an enemy
        if(collision.tag == "Enemy")
        {
            //Check if the trap is active
            if(trapController.trapActive)
            {
                DamageDelt.Invoke(damage, 0f);
            }
        }
    }


}
