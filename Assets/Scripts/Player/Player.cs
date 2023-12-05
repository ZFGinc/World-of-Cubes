using UnityEngine;
using UnityEngine.InputSystem;

namespace ZFGinc.Assets.WorldOfCubes
{
    [SelectionBase]
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        #region Movement
        [SerializeField] private float _gravityMultiplier = 3.0f;
        [SerializeField] private float _jumpPower = 4.0f;
        [SerializeField] private float _defaultSpeed;
        [SerializeField] private Transform _skinsParent;

        [SerializeField] private Animator _animator;
        private CharacterController _characterController;

        private Vector2 _input = Vector2.zero;
        private Vector3 _direction;

        private float _currentVelocity;
        private float _velocity;
        private float _angleRotate = -45f;
        private int _id = -1;

        private const float GRAVITY = -9.81f;
        private const float SMOOTHTIME = 0.05f;

        private IContactable _contactable;
        #endregion

        public void Initialization(int id, Transform position)
        {
            _id = id;

            transform.position = position.position + Vector3.up * 3;

            _characterController = GetComponent<CharacterController>();
            _characterController.enabled = true;

            SetSkin();
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
        }

        public void OnAction(InputAction.CallbackContext context)
        {
            if (!IsGrounded()) return;
            if (!IsAnimatorChaged()) return;

            _animator.SetTrigger("action");

            if (_contactable == null) return;

            _contactable.Contact(true);
            Hinput.anyGamepad.Vibrate();
        }

        private bool IsGrounded() => _characterController.isGrounded && _velocity < 0.0f;

        private bool IsAnimatorChaged() => _animator != null;

        public void ControllAnimationRun()
        {
            if(_animator==null) return;

            if (_input != Vector2.zero)
            {
                _animator.SetBool("run", true);
                return;
            }

            _animator.SetBool("run", false);
        }

        public void ApplyMovement()
        {
            if (!IsAnimatorChaged()) return;

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
            if (!IsAnimatorChaged()) return;

            if (IsGrounded())
            {
                _velocity = -1f;
            }
            else
            {
                _velocity += GRAVITY * _gravityMultiplier * Time.deltaTime;
            }

            _direction.y = _velocity;
        }

        private void SetSkin()
        {
            int skinIndex = PlayerPrefs.GetInt("skin_"+_id.ToString(), 0);
            Transform skin = _skinsParent.GetChild(skinIndex);
            GameObject armature = skin.GetChild(0).gameObject;

            _animator = armature.GetComponent<Animator>();
            skin.gameObject.SetActive(true);
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IContactable blockcontact))
            {
                if (_contactable == null) _contactable = blockcontact;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IContactable blockcontact))
            {
                if (_contactable == blockcontact)
                {
                    _contactable = null;
                }
            }
        }
    }
}