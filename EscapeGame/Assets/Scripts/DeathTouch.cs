using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class DeathTouch : MonoBehaviour {

    [SerializeField]
    int damage;

    private void Start()
    {
        this.OnCollisionEnterAsObservable().Subscribe(c =>
        {
            IDamageable damageable = c.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        });
    }
}
