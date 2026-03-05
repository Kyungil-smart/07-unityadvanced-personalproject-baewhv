using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    private PlayerStatus _status;
    private CharacterMovement _movement;
    private MainPlayerInput _input;
    public Camera _camera { get; private set; }

    public bool LockCharacterRotateCamera { get; set; }

    Vector3 InputAxis;
    Vector3 LastInputAxis;


    //unityEvents
    private void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
        _status = new PlayerStatus();
        _movement.Init(_status);
        _camera = Camera.main;

        //Input
        _input = new MainPlayerInput();

        _status.Direction.AddListener(_movement.ChangeDirectionAnim);
        _status.CurrentSpeed.AddListener(_movement.ChangeSpeedAnim);
        _status.IsJump.AddListener(value =>
        {
            if (value == true)
            {
                _movement._jumpStateMachine.ChangeState(_movement.JumpingStart);
            }
        });
        _status.IsCrouch.AddListener(_movement.ChangeCrouchAnim);
        _status.IsAction.AddListener(_movement.ChangeAction);
    }

    private void OnEnable()
    {
        _input.asset.Enable();
        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMoveCancel;
        _input.Player.Walk.started += OnWalk;
        _input.Player.Walk.canceled += OnWalkCancel;
        _input.Player.Jump.started += OnJump;
        _input.Player.Jump.canceled += OnJumpCancel;
        _input.Player.Crouch.started += OnCrouch;
        _input.Player.Crouch.canceled += OnCrouchCancel;
        _input.Player.MeleeWeapon.performed += SwitchMeleeWeapon;
        _input.Player.RangeWeapon.performed += SwitchRangeWeapon;
        _input.Player.RemoveWeapon.performed += RemoveWeapon;
    }



    private void OnDisable()
    {
        _input.Player.Move.performed -= OnMove;
        _input.Player.Move.canceled -= OnMoveCancel;
        _input.Player.Walk.started -= OnWalk;
        _input.Player.Walk.canceled -= OnWalkCancel;
        _input.Player.Jump.started -= OnJump;
        _input.Player.Jump.canceled -= OnJumpCancel;
        _input.Player.Crouch.started -= OnCrouch;
        _input.Player.Crouch.canceled -= OnCrouchCancel;
        _input.asset.Disable();
    }

    private void FixedUpdate()
    {
        Vector3 cameraForward = _camera.transform.forward;
        Vector3 cameraRight = _camera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        _status.Direction.Value =
            Vector3.MoveTowards(_status.Direction.Value, InputAxis, Time.fixedDeltaTime * 10.0f);

        _status.TargetDirection = cameraRight * InputAxis.x + cameraForward * InputAxis.z;
        
        _movement.SetForward(LockCharacterRotateCamera
            ? cameraRight * LastInputAxis.x + cameraForward * LastInputAxis.z
            : cameraForward);
    }

    //interfaces
    public void TakeDamage(int value)
    {
        throw new System.NotImplementedException();
    }


    //member
    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();

        InputAxis = new Vector3(value.x, 0, value.y);
        LastInputAxis = InputAxis;
    }

    private void OnMoveCancel(InputAction.CallbackContext ctx)
    {
        InputAxis = Vector3.zero;
    }

    private void OnWalk(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            _status.IsWalk.Value = true;
    }

    private void OnWalkCancel(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
            _status.IsWalk.Value = false;
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !_status.IsJump.Value)
        {
            _status.IsJump.Value = true;
        }
    }

    private void OnJumpCancel(InputAction.CallbackContext ctx)
    {
        if (_status.JumpVelocity.Value > 0)
            _status.JumpVelocity.Value *= 0.5f;
    }

    private void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !_status.IsJump.Value)
        {
            _status.IsCrouch.Value = true;
            LockCharacterRotateCamera = true;
        }
    }

    private void OnCrouchCancel(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            _status.IsCrouch.Value = false;
            LockCharacterRotateCamera = false;
        }
    }
    
    private void RemoveWeapon(InputAction.CallbackContext obj)
    {
        _movement.SetMotionState(MotionState.Unarmed);
    }

    private void SwitchRangeWeapon(InputAction.CallbackContext obj)
    {
        _movement.SetMotionState(MotionState.Range);
    }

    private void SwitchMeleeWeapon(InputAction.CallbackContext obj)
    {
        _movement.SetMotionState(MotionState.Melee);
    }
}