public class RangeMode : IState
{
    private Equipment _equipment;
    private CharacterMovement _movement;
    public RangeMode(Equipment equipment, CharacterMovement movement)
    {
        _equipment = equipment;
        _movement = movement;
    }

    public void Enter()
    {
        _equipment.CurrentMode = WeaponMode.Range;
        _movement.ChangeAnimController(_equipment.Modes[(int)WeaponMode.Range]);
        _equipment.RangeWeapon.transform.SetParent(_equipment.RightHand, false);
    }

    public void Update()
    {
    }

    public void Exit()
    {
        _equipment.RangeWeapon.transform.SetParent(_equipment.RangeWeaponHolster, false);
    }
}