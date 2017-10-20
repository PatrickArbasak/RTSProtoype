using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : Placeable , ISelectable {

    [SerializeField] protected int maxHealth;

    protected float baseHealth;
    public float BaseHealth{get{return baseHealth;}}

    [SerializeField] int baseCost;
    public int BaseCost {get { return baseCost; }set { baseCost = value; }}

    protected override void Start()
    {
        base.Start();
        baseHealth = maxHealth;
	}

    protected override void Update()
    {
        base.Update();
        AddjustCurrentHealth(0);
    }

    // returns true if health is now lower than 0.
    public virtual bool Damage(float damage)
    {
        baseHealth -= damage;
        return baseHealth <= 0.0f;
    }

    public void AddjustCurrentHealth(int adj)
    {
        baseHealth += adj;

        if (baseHealth < 0)
            baseHealth = 0;
        else if (baseHealth > maxHealth)
            baseHealth = maxHealth;
    }

    public void Selected()
    {
        UIManager.instance.EnableHealthBarUI();
        UIManager.instance.healthBar.sizeDelta = new Vector2(baseHealth, UIManager.instance.healthBar.sizeDelta.y);
    }

    public void UnSelected()
    {
        UIManager.instance.DisableHealthBarUI();
    }
}
