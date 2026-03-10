using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine;

namespace SurviveWar
{
    [RequireComponent(typeof(PlayerInput))]
    public class ThirdPersonController : MonoBehaviour
    {
        #region Public Fields
        public bool isOnPc = true;

        [Header("Global")]
        public ObjectPooler objectPooler;
        public WeaponsManager weaponsManager;
        [SerializeField] private Animator _animator;
        [SerializeField] private FixedTouchField _touchField;

        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;
        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;
        [Tooltip("Crouch speed of the character in m/s")]
        public float CrouchSpeed = 1.0f;
        [Tooltip("Move speed of the character in m/s when is Aim or Shoot")]
        public float AimMoveSpeed = 1.0f;
        [Tooltip("Sprint speed of the character in m/s when is Aim or Shoot")]
        public float AimSprintSpeed = 1.9f;
        [Tooltip("Crouch speed of the character in m/s when is Aim or Shoot")]
        public float AimCrouchSpeed = 0.5f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;
        [Tooltip("Crouch interpolation between layers")]
        public float CrouchChangeRate = 5.0f;

        [Header("Jump")]
        [Tooltip("The height the player can Jump")]
        public float JumpHeight = 1.2f;
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;
        [Tooltip("Time required to pass before being able to Jump again. Set to 0f to instantly Jump again")]
        public float JumpTimeout = 0.50f;
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;    
        
        [Header("Sensitivity")]
        [Tooltip("Input sensitivity for look")]
        public float SensitivityOnGamepad = 0.5f;
        public float SensitivityOnMouse = 1;
        public float SensitivityOnMobile = 5;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;
        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;
        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;
        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;
        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;
        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;
        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        [Header("Aim")]
        public LayerMask rayMask;
        public CinemachineVirtualCamera aimCam;
        public GameObject dinamicTargetAim;

        [Header("Shoot")]
        public GameObject projectilePrefab;
        [Space(10)]
        #endregion

        #region Private Values 
        // cinemachine
        [HideInInspector] public float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _verticalVelocity;
        private readonly float _terminalVelocity = 53.0f;
        private float _targetSensitivity;
        private float _sensitivity;
        private float _crouchLerp;

        // aim
        private Ray _ray;
        private Vector3 mouseWorldPos = Vector3.zero;
        [HideInInspector] public Vector3 worldAimTarget;
        private Vector3 aimDirection;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDCrouchSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDMoveX;
        private int _animIDMoveY;
        private int _animIDAim;

        private PlayerInput _playerInput;
        private CharacterController _Controller;

        private EntityInputs _input;
        private GameObject _mainCamera;
        private const float _threshold = 0.01f;

        #endregion

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }
        private void Start()
        {
            _Controller = GetComponent<CharacterController>();
            _input = GetComponent<EntityInputs>();
            _playerInput = GetComponent<PlayerInput>();

            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }
        private void Update()
        {
            GroundedCheck();
            JumpAndGravity();
            Move();
            Crouch();
            Aim();

            if (_input.SetRifle) { weaponsManager.SetRifleAsCurrentWeapon(); _input.SetRifle = false; }
            if (_input.SetGun) { weaponsManager.SetGunAsCurrentWeapon(); _input.SetGun = false; }
            if (_input.SetKnife) { weaponsManager.SetKnifeAsCurrentWeapon(); _input.SetKnife = false; }
        }
        private void LateUpdate()
        {
            CameraRotation();            
        }

        public CharacterController GetCharacterController()
        {
            return _Controller;
        }
        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDCrouchSpeed = Animator.StringToHash("CrouchSpeed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDMoveX = Animator.StringToHash("movX");
            _animIDMoveY = Animator.StringToHash("movY");
            _animIDAim = Animator.StringToHash("Aim");
        }
        public void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_animator != null)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }
        private void CameraRotation()
        {
            _sensitivity = (isOnPc) ? (Gamepad.all.Count > 0) ? SensitivityOnGamepad : SensitivityOnMouse: SensitivityOnMobile;
            _targetSensitivity =  _sensitivity;

            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = 1;

            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * _targetSensitivity;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier * _targetSensitivity;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(
                    _cinemachineTargetPitch + CameraAngleOverride,
                    _cinemachineTargetYaw, 0.0f);
        }
        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.Sprint ? SprintSpeed : _input.Crouch ? CrouchSpeed : MoveSpeed;
            float targetAimSpeed = _input.Sprint ? AimSprintSpeed : _input.Crouch ? AimCrouchSpeed : AimMoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
                targetAimSpeed = 0.0f;
            }

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_Controller.velocity.x, 0.0f, _Controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;          

            if (_input.Aim || _input.Shoot)
            {
                // accelerate or decelerate to target speed
                if (currentHorizontalSpeed < targetAimSpeed - speedOffset ||
                    currentHorizontalSpeed > targetAimSpeed + speedOffset)
                {
                    // creates curved result rather than a linear one giving a more organic speed change
                    // note T in Lerp is clamped, so we don't need to clamp our speed
                    _speed = Mathf.Lerp(currentHorizontalSpeed, targetAimSpeed * inputMagnitude,
                        Time.deltaTime * SpeedChangeRate);

                    // round speed to 3 decimal places
                    _speed = Mathf.Round(_speed * 1000f) / 1000f;
                }
                else
                {
                    _speed = targetAimSpeed;
                }

                _animationBlend = Mathf.Lerp(_animationBlend, targetAimSpeed, Time.deltaTime * SpeedChangeRate);
                if (_animationBlend < 0.01f) _animationBlend = 0f;
            }
            else
            {
                // accelerate or decelerate to target speed
                if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                    currentHorizontalSpeed > targetSpeed + speedOffset)
                {
                    // creates curved result rather than a linear one giving a more organic speed change
                    // note T in Lerp is clamped, so we don't need to clamp our speed
                    _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                        Time.deltaTime * SpeedChangeRate);

                    // round speed to 3 decimal places
                    _speed = Mathf.Round(_speed * 1000f) / 1000f;
                }
                else
                {
                    _speed = targetSpeed;
                }

                _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
                if (_animationBlend < 0.01f) _animationBlend = 0f;
            }

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(
                    inputDirection.x, inputDirection.z) * 
                    Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _Controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_animator != null)
            {
                if (_input.Crouch)
                {                    
                    _animator.SetFloat(_animIDCrouchSpeed, _animationBlend);
                    _animator.SetFloat(_animIDSpeed, 0);
                }
                else if ((_input.Shoot && !_input.Crouch) || (_input.Aim && !_input.Crouch))
                {
                    _animator.SetFloat(_animIDSpeed, _animationBlend);
                }
                else
                {
                    _animator.SetFloat(_animIDSpeed, _animationBlend);
                }

                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
                _animator.SetFloat(_animIDMoveX, _input.move.x);
                _animator.SetFloat(_animIDMoveY, _input.move.y);
            }
        }
        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_animator != null)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.Jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_animator != null)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_animator != null)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.Jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
        private void Crouch()
        {
            //_animator.SetBool(_animIDCrouch, _input.crouch);

            int animLayerIndex = _animator.GetLayerIndex("Crouching");

            if (_input.Crouch)
            {               
                _crouchLerp += 1 / CrouchChangeRate;

                if (_crouchLerp >= 1) _crouchLerp = 1.0f;
                _animator.SetLayerWeight(animLayerIndex, _crouchLerp);
            }
            else if(!_input.Crouch)
            {
                _crouchLerp -= 1 / CrouchChangeRate;

                if (_crouchLerp <= 0) _crouchLerp = 0.0f;
                _animator.SetLayerWeight(animLayerIndex, _crouchLerp);
            }
        }
        private void Aim()
        {
            Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f);

            _ray = Camera.main.ScreenPointToRay(screenCenterPoint);

            if (Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity, rayMask))
            {
                mouseWorldPos = hit.point;
                Debug.DrawLine(_ray.origin, mouseWorldPos, Color.yellow);
            }

            // Aim body to ray direction in base of camera rotation
            aimDirection = _ray.direction;
            Vector3 aimDir = new(aimDirection.x, 0.0f, aimDirection.z);
            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * 20f);

            if (_input.Aim && weaponsManager.currentWeapon.inUse && weaponsManager.currentWeapon.weaponType != Enums.WeaponType.Knife)
                 aimCam.gameObject.SetActive(true);
            else aimCam.gameObject.SetActive(false);


            worldAimTarget = mouseWorldPos;
            dinamicTargetAim.transform.position = worldAimTarget;

            bool targetInput = _input.Aim ? _input.Aim : _input.Shoot;
            _animator.SetBool(_animIDAim, targetInput);
        }       
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }      

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }
    }
}