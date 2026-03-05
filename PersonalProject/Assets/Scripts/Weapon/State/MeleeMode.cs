public class MeleeMode : IState
{
    private Equipment _equipment;
    private CharacterMovement _movement;

    public MeleeMode(Equipment equipment, CharacterMovement movement)
    {
        _equipment = equipment;
        _movement = movement;
    }
    public void Enter()
    {
        _equipment.CurrentMode = WeaponMode.Melee;
        _movement.ChangeAnimController(_equipment.Modes[(int)WeaponMode.Melee]);
        _equipment.MeleeWeapon.transform.SetParent(_equipment.RightHand, false);
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        if(_equipment.MeleeWeapon) //근접 무기는 부서짐.
            _equipment.MeleeWeapon.transform.SetParent(_equipment.MeleeWeaponHolster, false);
    }
}