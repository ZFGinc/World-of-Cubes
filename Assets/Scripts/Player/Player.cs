using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    #region Movement
    [SerializeField] private float _gravityMultiplier = 3.0f;
    [SerializeField] private float _jumpPower = 4.0f;
    [SerializeField] private float _defaultSpeed;

    private Animator _animator;
    private CharacterController _characterController;

    private Vector2 _input = Vector2.zero;
    private Vector3 _direction;

    private float _currentVelocity;
    private float _velocity;
    private float _angleRotate = -45f;

    private const float GRAVITY = -9.81f;
    private const float SMOOTHTIME = 0.05f;
    #endregion

    #region UIComponents

    [SerializeField] private PlayerUI _playerUI;
    #endregion

    private void OnValidate()
    {
        //_animator = GetComponent<Animator>();
        //_characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //Gravity player
        ApplyGravity();

        //Rotate player
        ApplyRotation();

        //Move player
        ApplyMovement();
    }

    public void OnMove(InputAction.CallbackContext context) 
    { 
        _input = Rotate(context.ReadValue<Vector2>(), _angleRotate);
    }

    public void OnJump(InputAction.CallbackContext context) 
    {
        if (!IsGrounded()) return;

        _velocity += _jumpPower;
        _animator.SetTrigger("isJump");
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!IsGrounded()) return;

        _animator.SetTrigger("isAttack");
        StartCoroutine(StopMovement(0.7f));
    }

    private bool IsGrounded() => _characterController.isGrounded && _velocity < 0.0f;

    public void SetNumber(int namber) => _playerUI.SetNumber(namber);

    public void ControllAnimationRun()
    {
        if (_input != Vector2.zero)
        {
            _animator.SetBool("isRun", true);
            return;
        }
        
        _animator.SetBool("isRun", false);
    }

    public void ApplyMovement() 
    {
        _direction.x = _input.x; 
        _direction.z = _input.y;

        _characterController.Move(_direction * Time.deltaTime * _defaultSpeed);
        ControllAnimationRun();
    }

    public void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0) return;

        var targetangel = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangel, ref _currentVelocity, SMOOTHTIME);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    public void ApplyGravity()
    {
        if(IsGrounded())
        {
            _velocity = -1f;
        }
        else
        {
            _velocity += GRAVITY * _gravityMultiplier * Time.deltaTime;
        }

        _direction.y = _velocity;
    }

    private IEnumerator Pause(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private IEnumerator StopMovement(float seconds)
    {
        float tempSpeed = _defaultSpeed;
        _defaultSpeed = 0f;
        yield return StartCoroutine(Pause(seconds));
        _defaultSpeed = tempSpeed;
    }

    private Vector2 Rotate(Vector2 v, float angle)
    {
        angle *= Mathf.Deg2Rad;
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float tx = v.x;
        float ty = v.y;

        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);

        return v;
    }
}
