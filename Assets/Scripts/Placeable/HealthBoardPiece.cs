using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoardPiece : BoardPiece, ISelectable, IDamageable<float>
{
    [SerializeField] protected int maxHealth;

    protected float baseHealth;
    //public float BaseHealth { get { return baseHealth; } }

    protected override void Start()
    {
        base.Start();
        baseHealth = maxHealth;
    }

    public virtual void Selected()
    {
        UIManager.instance.EnableHealthBarUI(baseHealth, maxHealth);
    }

    public virtual void UnSelected()
    {
        UIManager.instance.DisableHealthBarUI();
    }

    public void Damage(float damageTaken)
    {
        baseHealth -= damageTaken;
    }
}
