using System;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    private Equipment _owner;
    public int Damage;

    private int _durability;

    private int Durability
    {
        get => _durability;
        set
        {
            _durability = value;
            if(Durability < 0) DestroyWeapon();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if ((_owner.CompareTag("Enemy") || _owner.CompareTag("Player")) && !other.CompareTag(_owner.tag))
        {
            IDamageable target = other.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(Damage);
                Durability--;
            }
        }
    }

    private void DestroyWeapon()
    {
        Destroy(gameObject);
    }
}
