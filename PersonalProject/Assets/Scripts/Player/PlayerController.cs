using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    private CharacterController _controller;
    private MainPlayerInput _input;
    private Camera _camera;

    public bool LockCharacterRotateCamera { get; set; }

    private Transform _body;
    [SerializeField] private Animator _anim;
    public Animator GetAnim => _anim;
    public AnimationReceiver _animReceiver { get; private set; }

    public float TargetSpeed { get; set; }
    public ObserveValue<float> Speed = new();
    public ObserveValue<Vector3> InputAxis = new();
    public ObserveValue<float> JumpVelocity = new();
    public ObserveValue<bool> IsWalk = new();
    public ObserveValue<bool> IsJump = new();
    public ObserveValue<bool> IsCrouch = new();

    //StateMachine
    private StateMachine _movementStateMachine;
    public StandState Standby { get; private set; }
    public RunState Run { get; private set; }
    public WalkState Walk { get; private set; }

    private StateMachine _jumpStateMachine;
    public JumpStandbyState JumpStandby { get; private set; }
    public JumpingState JumpingStart { get; private set; }
    public FallState Fall { get; private set; }
    public LandedState Landed { get; private set; }
    public float JumpHeight { get; private set; } = 2.0f;

    //unityEvents
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _body = _anim.gameObject.transform;
        _animReceiver = _body.GetComponent<AnimationReceiver>();
        _camera = Camera.main;

        //States
        _movementStateMachine = new StateMachine();
        Standby = new StandState(this);
        Walk = new WalkState(this);
        Run = new RunState(this);
        _movementStateMachine.ChangeState(Standby);
        
        _jumpStateMachine = new StateMachine();
        JumpStandby = new JumpStandbyState(this);
        JumpingStart = new JumpingState(this);
        Fall = new FallState(this);
        Landed = new LandedState(this);
        _jumpStateMachine.ChangeState(JumpStandby);
        _animReceiver.OnEndLand.AddListener(Landed.EndJump);

        //Input
        _input = new MainPlayerInput();

        InputAxis.AddListener(ChangeDirection);
        Speed.AddListener(ChangeSpeed);
        IsJump.AddListener(value =>
        {
            if (value == true)
            {
                _jumpStateMachine.ChangeState(JumpingStart);
            }
        });
        IsCrouch.AddListener(ChangeCrouch);
    }

    private void OnEnable()
    {
        _input.asset.Enable();
        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMoveCancel;
        _input.Player.Walk.started += OnWalk;
        _input.Player.Walk.canceled += OffWalk;
        _input.Player.Jump.started += OnJump;
        _input.Player.Jump.canceled += OnJumpCanceled;
        _input.Player.Crouch.started += OnCrouch;
        _input.Player.Crouch.canceled += OnCrouchCancel;
    }

    private void OnDisable()
    {
        _input.Player.Move.performed -= OnMove;
        _input.Player.Move.canceled -= OnMoveCancel;
        _input.Player.Walk.started -= OnWalk;
        _input.Player.Walk.canceled -= OffWalk;
        _input.Player.Jump.started -= OnJump;
        _input.Player.Jump.canceled -= OnJumpCanceled;
        _input.Player.Crouch.started -= OnCrouch;
        _input.Player.Crouch.canceled -= OnCrouchCancel;
        _input.asset.Disable();
    }

    public void Update()
    {
        _movementStateMachine.Update();
        _jumpStateMachine.Update();
    }

    public void FixedUpdate()
    {
        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();


        Vector3 move = right * InputAxis.Value.x + forward * InputAxis.Value.z;
        _body.forward = forward;
        _controller.Move(move * Speed.Value * Time.fixedDeltaTime);

        JumpVelocity.Value += Physics.gravity.y * Time.fixedDeltaTime;
        _controller.Move(Vector3.up * JumpVelocity.Value * Time.fixedDeltaTime);
    }
    
    //interfaces
    public void Damaged(int value)
    {
        throw new System.NotImplementedException();
    }
    
    //member

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();
        InputAxis.Value = new Vector3(value.x, 0, value.y);
    }

    private void OnMoveCancel(InputAction.CallbackContext ctx)
    {
        InputAxis.Value = Vector3.zero;
    }

    private void OnWalk(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            IsWalk.Value = true;
    }

    private void OffWalk(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
            IsWalk.Value = false;
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !IsJump.Value)
        {
            IsJump.Value = true;
        }
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        if (JumpVelocity.Value > 0)
            JumpVelocity.Value *= 0.5f;
    }

    private void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            IsCrouch.Value = true;
    }
    private void OnCrouchCancel(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
            IsCrouch.Value = false;
    }

    public void ChangeMovement(IState state)
    {
        _movementStateMachine.ChangeState(state);
    }
    
    public void ChangeJumpState(IState state)
    {
        _jumpStateMachine.ChangeState(state);
    }

    public void ChangeDirection(Vector3 value)
    {
        _anim.SetFloat("Direction", Mathf.Atan2(value.x, value.z) / Mathf.PI);
    }

    public void ChangeSpeed(float value)
    {
        _anim.SetFloat("Speed", value);
    }

    public void ChangeCrouch(bool value)
    {
        _anim.SetBool("IsCrouch", value);
    }

    public bool isGrounded()
    {
        if (_controller.isGrounded) return true;
        Vector3 ray = transform.position + Vector3.up * 0.1f;
        if (Physics.Raycast(ray, Vector3.down, out RaycastHit Hit, 0.2f)) return true;
        return false;
    }
    
    


}