using System;
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

    private float _speed;

    public float Speed
    {
        get => _speed;
        set
        {
            _speed = value;
            _anim.SetFloat("Speed", _speed * 100);
        }
    }

    private Vector3 _inputAxis;

    public Vector3 InputAxis
    {
        get => _inputAxis;
        private set
        {
            _inputAxis = value;
            OnChangeAngle?.Invoke(Vector3.SignedAngle(Vector3.forward, value, Vector3.up));
        }
    }

    public UnityEvent<float> OnChangeAngle;


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
        OnChangeAngle.AddListener(ChangeDirection);
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
        Vector3 right =  _camera.transform.right;
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

    public void ChangeDirection(float value)
    {
        _anim.SetFloat("Direction", value);
    }
}