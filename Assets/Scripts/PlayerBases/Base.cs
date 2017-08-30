using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {

    [SerializeField] protected int maxHealth;

    protected float baseHealth;
    public float BaseHealth
    {
        get{return baseHealth;}
        //set{baseHealth = value;}
    }

    protected virtual void Start()
    {
        baseHealth = maxHealth;
	}

    // returns true if health is now lower than 0.
    public virtual bool Damage(float damage)
    {
        baseHealth -= damage;
        //Debug.Log("Base Damage() " + baseHealth);
        return baseHealth <= 0.0f;
    }
}
