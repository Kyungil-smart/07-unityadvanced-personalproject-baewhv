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

    private ObserveValue<float> _speed;
    public float Speed
    {
        get => _speed.Value;
        private set => _speed.Value = value;
    }

    private ObserveValue<Vector3> _inputAxis = new ObserveValue<Vector3>();

    public Vector3 InputAxis
    {
        get => _inputAxis.Value;
        private set => _inputAxis.Value = value;
    }

    //MovementState
    private PlayerMovement Movement;
    public StandState Standby { get; private set; }
    public RunState Run { get; private set; }
    public WalkState Walk { get; private set; }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _body = _anim.gameObject.transform;
        Movement = new PlayerMovement();
        _camera = Camera.main;

        _input = new MainPlayerInput();
        Standby = new StandState(this);
        Walk = new WalkState(this);
        Run = new RunState(this);
        Movement.ChangeState(Standby);
        _inputAxis.AddListener(ChangeDirection);
        _speed.AddListener(ChangeSpeed);
    }

    private void OnEnable()
    {
        _input.asset.Enable();
        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMoveCancel;
    }

    private void OnDisable()
    {
        _input.Player.Move.performed -= OnMove;
        _input.Player.Move.canceled -= OnMoveCancel;
        _input.asset.Disable();
    }

    public void Update()
    {
        Movement.Update();
    }

    public void FixedUpdate()
    {
        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 move = right * InputAxis.x + forward * InputAxis.z;
        _body.forward = forward;
        _controller.Move(move * Speed * Time.fixedDeltaTime);
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();
        InputAxis = new Vector3(value.x, 0, value.y);
    }

    private void OnMoveCancel(InputAction.CallbackContext ctx)
    {
        InputAxis = Vector3.zero;
    }

    public void ChangeMovement(IState state)
    {
        Movement.ChangeState(state);
    }

    public void ChangeDirection(Vector3 value)
    {
        _anim.SetFloat("Direction", Mathf.Atan2(value.x, value.z) / Mathf.PI);
    }

    public void ChangeSpeed(float value)
    {
        _anim.SetFloat("Speed", value/3);
    }
}