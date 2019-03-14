using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectHealthController : MonoBehaviour {

    [SerializeField] private float health = 100f;

    public void ApplyDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
	
}
