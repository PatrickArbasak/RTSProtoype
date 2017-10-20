using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : Placeable , ISelectable , IDamageable<float>{

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

    public void Selected()
    {
        UIManager.instance.EnableHealthBarUI();
        UIManager.instance.healthBar.sizeDelta = new Vector2(baseHealth, UIManager.instance.healthBar.sizeDelta.y);
    }

    public void UnSelected()
    {
        UIManager.instance.DisableHealthBarUI();
    }

    public void Damage(float damageTaken)
    {
        baseHealth -= damageTaken;
    }
}
