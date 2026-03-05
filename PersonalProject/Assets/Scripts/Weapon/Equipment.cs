using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField]private MeleeWeapon _melee;
    [SerializeField]private RangeWeapon _range;

    private void SetMeleeWeapon(MeleeWeapon weapon)
    {
        //_melee.DropWeapon;
        _melee = weapon;
    }

    private void SetRangeWeapon()
    {
        
    }
}
