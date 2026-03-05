using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Equipment : MonoBehaviour
{
    public MeleeWeapon MeleeWeapon { get; private set; }
    public RangeWeapon RangeWeapon { get; private set; }

    [field: SerializeField] public Transform MeleeWeaponHolster { get; set; }
    [field: SerializeField] public Transform RangeWeaponHolster { get; set; }
    [field: SerializeField] public Transform RightHand { get; set; }

    private StateMachine ModeState;
    private MeleeMode _meleeMode;
    private RangeMode _rangeMode;
    private UnarmedMode _unarmedMode;

    public WeaponMode CurrentMode { get; set; } = WeaponMode.Unarmed;

    [field: SerializeField] public RuntimeAnimatorController[] Modes { get; set; }

    private void Start()
    {
        if(MeleeWeapon)
            MeleeWeapon.transform.SetParent(MeleeWeaponHolster);
        if(RangeWeapon)
            RangeWeapon.transform.SetParent(RangeWeaponHolster, false);
    }

    public void Init()
    {
        CharacterMovement _movement = GetComponent<CharacterMovement>();
        ModeState = new StateMachine();
        _meleeMode = new MeleeMode(this, _movement);
        _rangeMode = new RangeMode(this, _movement);
        _unarmedMode = new UnarmedMode(this, _movement);
        
        ModeState.ChangeState(_unarmedMode);
    }

    //외부에서 무기 설정
    public void SetMeleeWeapon(MeleeWeapon weapon)
    {
        if (MeleeWeapon) DropMeleeWeapon();
        MeleeWeapon = weapon;
    }

    public void SetRangeWeapon(RangeWeapon weapon)
    {
        RangeWeapon = weapon;
    }

    public void DestroyMeleeWeapon()
    {
        if (!MeleeWeapon) return;
        Destroy(MeleeWeapon.gameObject);
        MeleeWeapon = null;
    }

    public void DestroyRangeWeapon()
    {
        if (!RangeWeapon) return;
        Destroy(RangeWeapon.gameObject);
        RangeWeapon = null;
    }

    public void DropMeleeWeapon()
    {
        if (!MeleeWeapon) return;
        MeleeWeapon.transform.SetParent(ObjectManager.Instance.transform);
        MeleeWeapon.transform.position = transform.position + Vector3.up;
    }

    public void DropRangeWeapon()
    {
        if (!RangeWeapon) return;
        MeleeWeapon.transform.SetParent(ObjectManager.Instance.transform);
        MeleeWeapon.transform.position = transform.position + Vector3.up;
    }

    public void ChangeMode(WeaponMode mode)
    {
        if (CurrentMode == mode) return;
        switch (mode)
        {
            case WeaponMode.Melee:
                if (!MeleeWeapon) return;
                ModeState.ChangeState(_meleeMode);
                break;
            case WeaponMode.Range:
                if (!RangeWeapon) return;
                ModeState.ChangeState(_rangeMode);
                break;
            case WeaponMode.Unarmed:
                ModeState.ChangeState(_unarmedMode);
                break;
        }
    }

    public void EquipWeapon()
    {
        switch (CurrentMode)
        {
            case WeaponMode.Melee:
                if (!MeleeWeapon) return;
                MeleeWeapon.transform.SetParent(RightHand, false);
                break;
            case WeaponMode.Range:
                if (!RangeWeapon) return;
                RangeWeapon.transform.SetParent(RightHand, false);
                break;
        }
    }
}


public enum WeaponMode
{
    Range = 0,
    Melee = 1,
    Unarmed = 2,
}