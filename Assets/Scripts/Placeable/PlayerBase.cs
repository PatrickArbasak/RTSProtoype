using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : Base
{
    // Events
    public delegate void PlayerBaseDestroyedHandler();
    public event PlayerBaseDestroyedHandler OnPlayerBaseDestroyed;

    override public bool Damage(float damage)
    {
        base.Damage(damage);
        //if (baseHealth <= 0.0f && BaseManager.instance.PlayerBases.Count > 0.0f)
        //{
        //    BaseManager.instance.PlayerBases.Remove(this);
        //    OnPlayerBaseDestroyed();
        //    Destroy(gameObject);
        //}
        return baseHealth <= 0;
    }
}
