using System;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public int Damage;
    public int Durability;

    private void OnTriggerEnter(Collider other)
    {
        (other.transform as IDamageable) 
    }
}
