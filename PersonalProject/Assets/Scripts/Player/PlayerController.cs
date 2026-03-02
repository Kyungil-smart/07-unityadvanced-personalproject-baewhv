using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private MainPlayerInput _input;
    private Camera _camera;

    private Transform _body;
    [SerializeField] private Animator _anim;

    public ObserveValue<float> Speed = new ObserveValue<float>();
    private float _jumpHeight = 2.0f;
    private float _jumpVelocity = 0.0f;
    public ObserveValue<Vector3> InputAxis = new ObserveValue<Vector3>();
    public ObserveValue<bool> IsWalk = new ObserveValue<bool>();
    public ObserveValue<bool> IsJump = new ObserveValue<bool>();

    //StateMachine
    private StateMachine _stateMachine;
    public StandState Standby { get; private set; }
    public RunState Run { get; private set; }
    public WalkState Walk { get; private set; }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _body = _anim.gameObject.transform;
        _camera = Camera.main;

        //States
        _stateMachine = new StateMachine();
        Standby = new StandState(this);
        Walk = new WalkState(this);
        Run = new RunState(this);
        _stateMachine.ChangeState(Standby);

        //Input
        _input = new MainPlayerInput();

        InputAxis.AddListener(ChangeDirection);
        Speed.AddListener(ChangeSpeed);
    }

    private void OnEnable()
    {
        _input.asset.Enable();
        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMoveCancel;
        _input.Player.Walk.started += OnWalk;
        _input.Player.Walk.canceled += OffWalk;
        _input.Player.Jump.started += OnJump;
        _input.Player.Jump.canceled += OnJump;
    }

    private void OnDisable()
    {
        _input.Player.Move.performed -= OnMove;
        _input.Player.Move.canceled -= OnMoveCancel;
        _input.Player.Walk.started -= OnWalk;
        _input.Player.Walk.canceled -= OffWalk;
        _input.Player.Jump.started -= OnJump;
        _input.Player.Jump.canceled -= OnJump;
        _input.asset.Disable();
    }

    public void Update()
    {
        _stateMachine.Update();
    }

    public void FixedUpdate()
    {
        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Gravity();

        Vector3 move = right * InputAxis.Value.x + forward * InputAxis.Value.z;
        _body.forward = forward;
        _controller.Move(move * Speed.Value * Time.fixedDeltaTime);
    }

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
        if (ctx.started)
        {
            IsJump.Value = true;
            _anim.CrossFade("Jump_Up", 0.1f);
        }
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        if (_jumpVelocity > 0)
            _jumpVelocity *= 0.5f;
    }

    public void ChangeMovement(IState state)
    {
        _stateMachine.ChangeState(state);
    }

    public void ChangeDirection(Vector3 value)
    {
        _anim.SetFloat("Direction", Mathf.Atan2(value.x, value.z) / Mathf.PI);
    }

    public void ChangeSpeed(float value)
    {
        _anim.SetFloat("Speed", value / 3);
    }

    private void Gravity()
    {
        if (isGrounded()) //없으면 이전 낙하정보가 있어서 빠른 속도로 떨어짐.
        {
            _jumpVelocity = -2f;
        }

        if (IsJump.Value && isGrounded())
        {
            _jumpVelocity = Mathf.Sqrt(_jumpHeight * -2f * Physics.gravity.y);
            _anim.CrossFade("Jump_Land", 0.1f);
            IsJump.Value = false;
        }

        _jumpVelocity += Physics.gravity.y * Time.fixedDeltaTime;
        _controller.Move(Vector3.up * _jumpVelocity * Time.fixedDeltaTime);
    }

    private bool isGrounded()
    {
        if (_controller.isGrounded) return true;
        Vector3 ray = transform.position + Vector3.up * 0.1f;
        if (Physics.Raycast(ray, Vector3.down, out RaycastHit Hit, 0.2f)) return true;
        return false;
    }
}