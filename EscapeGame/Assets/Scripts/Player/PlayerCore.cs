using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerCore : MonoBehaviour, IDamageable
{
    public IReadOnlyReactiveProperty<bool> IsDead => isDead;
    BoolReactiveProperty isDead = new BoolReactiveProperty(false);

    public void TakeDamage(int damage)
    {
        isDead.Value = true;
    }
}
