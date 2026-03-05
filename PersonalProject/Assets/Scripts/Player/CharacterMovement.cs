using System;
using UnityEngine;



public class CharacterMovement : MonoBehaviour
{
    private CharacterStatus _status;
    private CharacterController _controller;

    private StateMachine _movementStateMachine;
    public StandState Standby { get; private set; }
    public RunState Run { get; private set; }
    public WalkState Walk { get; private set; }

    public StateMachine _jumpStateMachine { get; private set; }
    public JumpStandbyState JumpStandby { get; private set; }
    public JumpingState JumpingStart { get; private set; }
    public FallState Fall { get; private set; }
    public LandedState Landed { get; private set; }

    private Transform _body;
    [SerializeField] private Animator _anim;

    public Animator GetAnim => _anim;
    private AnimationReceiver _animReceiver;

    public MotionState CurrentMotionState { get; private set; }
    [SerializeField]private RuntimeAnimatorController[] _controllers;

    public void Init(CharacterStatus status)
    {
        _status = status;
        _controller = GetComponent<CharacterController>();
        _body = _anim.gameObject.transform;
        _animReceiver = _body.GetComponent<AnimationReceiver>();

        _movementStateMachine = new StateMachine();
        Standby = new StandState(this, _status);
        Walk = new WalkState(this, _status);
        Run = new RunState(this, _status);
        _movementStateMachine.ChangeState(Standby);

        _jumpStateMachine = new StateMachine();
        JumpStandby = new JumpStandbyState(this, _status);
        JumpingStart = new JumpingState(this, _status);
        Fall = new FallState(this, _status);
        Landed = new LandedState(this, _status);
        _jumpStateMachine.ChangeState(JumpStandby);
        _animReceiver.OnEndLand.AddListener(Landed.EndJump);
    }

    private void Update()
    {
        _movementStateMachine.Update();
        _jumpStateMachine.Update();
    }

    public void FixedUpdate()
    {
        //애니메이션 전이
        if (Mathf.Abs(_status.TargetSpeed - _status.CurrentSpeed.Value) < 0.01f)
            _status.CurrentSpeed.Value = _status.TargetSpeed;
        else
            _status.CurrentSpeed.Value = Mathf.Lerp(_status.CurrentSpeed.Value, _status.TargetSpeed, 0.2f);

        _controller.Move(_status.TargetDirection * _status.CurrentSpeed.Value * Time.fixedDeltaTime);

        _status.JumpVelocity.Value += Physics.gravity.y * Time.fixedDeltaTime;
        _controller.Move(Vector3.up * _status.JumpVelocity.Value * Time.fixedDeltaTime);
    }


    public void ChangeMovement(IState state)
    {
        _movementStateMachine.ChangeState(state);
    }

    public void ChangeJumpState(IState state)
    {
        _jumpStateMachine.ChangeState(state);
    }

    private void ChangeAnimController(RuntimeAnimatorController controller)
    {
        _anim.runtimeAnimatorController = controller;
    }

    public void ChangeDirectionAnim(Vector3 value)
    {
        _anim.SetFloat("Direction", Mathf.Atan2(value.x, value.z) / Mathf.PI);
    }

    public void ChangeSpeedAnim(float value)
    {
        _anim.SetFloat("Speed", value);
    }

    public void ChangeCrouchAnim(bool value)
    {
        _anim.SetBool("IsCrouch", value);
    }

    public void ChangeAction(bool value)
    {
        _anim.SetBool("Action", value);
    }

    public void SetForward(Vector3 value)
    {
        _body.forward = value;
    }

    public bool isGrounded()
    {
        if (_controller.isGrounded) return true;
        Vector3 ray = transform.position + Vector3.up * 0.1f;
        if (Physics.Raycast(ray, Vector3.down, out RaycastHit Hit, 0.2f)) return true;
        return false;
    }

    public void SetMotionState(MotionState state)
    {
        CurrentMotionState = state;
    }
}
public enum MotionState
{
    Unarmed,
    Melee,
    Range
}