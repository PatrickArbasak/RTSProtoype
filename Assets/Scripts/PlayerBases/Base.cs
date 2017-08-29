using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {

    [SerializeField] protected int maxHealth;

    protected int baseHealth;
    public int BaseHealth
    {
        get{return baseHealth;}
        //set{baseHealth = value;}
    }

    protected virtual void Start()
    {
        baseHealth = maxHealth;
	}

    // returns true if health is now lower than 0.
    public virtual bool Damage(int damage)
    {
        baseHealth -= damage;
        return baseHealth <= 0;
    }
}
