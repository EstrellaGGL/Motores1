using System.Diagnostics;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
using UnityEngine.Windows;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioSource AudioFootsteps;
        public AudioSource LandingAudio;
        public AudioSource AudioFoley;
        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        private void OnTriggerEnter(Collider collision) // Si entran en colision con un Trigger
        {
            if (collision.gameObject.CompareTag("LongJump")) // Si el objeto tiene el tag LongJump
            {
                JumpHeight = 2.6f;                          // Modifica la altura de salto y destruye el objeto con el que colisionó
                Destroy(collision.gameObject);
            }

            if (collision.gameObject.CompareTag("Egg")) // Si colisiona con un objeto con el tag Egg
            {
                collision.transform.SetParent(BackObjectPosition); // Lo pone de hijo en el objeto vacio BackObjectPosition

                collision.transform.localPosition = Vector3.zero;
                collision.transform.localRotation = Quaternion.identity;
            }
            if (collision.gameObject.CompareTag("Shrink")) // Si colisiona con un objeto Shrink
            {
                _canShrink = true;                          // Cambia el bool a verdadero y destruye el objeto con el que colisionó
                Destroy(collision.gameObject);
            }
        }


        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;


        [SerializeField] private Transform BackObjectPosition;

        [Header("Shrink PowerUp")]
        [SerializeField] private Transform PlayerModel;
        [SerializeField] private float ShrinkMultiplier = 0.25f; // Multiplicador para hacerse pequeño
        private bool _canShrink = false; // Booleano de puede hacerse pequeño
        private bool _isShrunk = false; // Booleano de estar pequeño
        private Vector3 _originalModelScale; // Guarda la escala inicial del personaje
        private float _originalControllerHeight;
        private float _originalControllerRadius;
        private Vector3 _originalControllerCenter;
        public bool _inShrinkZone = false;

        private void Shrink()
        {
            
            if (!_canShrink) // Si el shrink está deshabilitado, no hacer nada
                return;

            
            bool shrinkPressed = _playerInput.actions["Shrink"].IsPressed();// Chequear si el jugador presionó la tecla de shrink

            
            RaycastHit hit; // Raycast hacia abajo (0.5f) para detectar si está sobre una shrink zone (layer "Shrink").Esto es un respaldo por si el trigger no se detecta bien
             
            bool inShrinkZoneByRaycast = Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f, LayerMask.GetMask("Shrink"));

            
            bool inShrinkZone = _inShrinkZone || inShrinkZoneByRaycast;// El personaje está en shrink zone si: 1. El trigger lo detectó (_inShrinkZone = true) O
                                                                                                            // 2. El raycast detectó el layer "Shrink"

            
            _animator.SetBool(_animIDShrink, shrinkPressed); // Actualizar el animator con el estado de shrink 

            
            if (shrinkPressed || inShrinkZone) // Si presionó shrink y está en shrink zone se mantiene pequeño 
            {
                
                if (!_isShrunk) // Si no está encogido, encogerlo
                {
                    _isShrunk = true;

                    
                    float newHeight = _originalControllerHeight * ShrinkMultiplier; // Calcular nuevas dimensiones del CharacterController (reducidas por ShrinkMultiplier)
                    float newRadius = _originalControllerRadius * ShrinkMultiplier;

                    
                    float originalBottom = _originalControllerCenter.y - (_originalControllerHeight / 2f); // Calcular la posición del centro del controller para que quede alineado con el piso

                    _controller.height = newHeight; // Aplicar nuevas dimensiones al CharacterController
                    _controller.radius = newRadius;

                    
                    Vector3 newCenter = _originalControllerCenter; // Ajustar el centro del controller para que quede alineado con el piso
                    newCenter.y = originalBottom + (newHeight / 2f);
                    _controller.center = newCenter;

                    
                    PlayerModel.localScale = _originalModelScale * ShrinkMultiplier; // Escalar el modelo del personaje (visual)
                }
                
            }
            else  // Si ya está encogido, no hacer nada (se mantiene pequeño)
            {
                
                if (_isShrunk) // Si NO presionó shrink Y NO está en shrink zone entonces se agranda
                {
                    _isShrunk = false;

                    
                    PlayerModel.localScale = _originalModelScale; // Restaurar escala original del modelo

                    
                    _controller.height = _originalControllerHeight; // Restaurar dimensiones originales del CharacterController
                    _controller.radius = _originalControllerRadius;
                    _controller.center = _originalControllerCenter;

                    
                    _verticalVelocity = 0f; // Resetear velocidad vertical para evitar empujón al agrandarse
                }
            }
        }

        
        
        
        public void ForceShrink(bool shrink)  // Fuerza el estado de shrink desde fuera (usado por ShrinkZone).
        {
            
            if (shrink && !_isShrunk) // Si shrink = true y no está encogido entonces encoger
            {
                _isShrunk = true;

                
                float newHeight = _originalControllerHeight * ShrinkMultiplier; // Calcular y aplicar nuevas dimensiones (igual que en Shrink())
                float newRadius = _originalControllerRadius * ShrinkMultiplier;

                float originalBottom = _originalControllerCenter.y - (_originalControllerHeight / 2f);

                _controller.height = newHeight;
                _controller.radius = newRadius;

                Vector3 newCenter = _originalControllerCenter;
                newCenter.y = originalBottom + (newHeight / 2f);
                _controller.center = newCenter;

                PlayerModel.localScale = _originalModelScale * ShrinkMultiplier;
            }
            
            else if (!shrink && _isShrunk) // Si shrink = false y está encogido entonces agrandar
            {
                _isShrunk = false;

                
                PlayerModel.localScale = _originalModelScale; // Restaurar escala y dimensiones originales

                _controller.height = _originalControllerHeight;
                _controller.radius = _originalControllerRadius;
                _controller.center = _originalControllerCenter;
            }
        }


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

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDShrink; // Animacion de encogerse

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;
        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


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
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _originalModelScale = PlayerModel.localScale;

            _originalControllerHeight = _controller.height;
            _originalControllerRadius = _controller.radius;
            _originalControllerCenter = _controller.center;
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
           
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);
            
            
            JumpAndGravity();
            GroundedCheck();
            Shrink(); // Método de encogerse
            Move();
            
        }
        private void LateUpdate()
        {
            
            if (_isShrunk)
            {
                PlayerModel.localScale = _originalModelScale * ShrinkMultiplier; // Forzar el scale del modelo si está encogido
            }
            else
            {
                PlayerModel.localScale = _originalModelScale;
            }
            CameraRotation();
        }
        
        
        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDShrink = Animator.StringToHash("Shrink"); // Parametro del animator para la animacion encogerse
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            // Calcular dirección del movimiento
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player (SOLO movimiento del jugador, la plataforma la mueve PlatformParenting)
            Vector3 playerMovement = targetDirection.normalized * (_speed * Time.deltaTime);

            _controller.Move(playerMovement + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }
        public bool IsMoving()
        {
            return _input.move != Vector2.zero;
        }
        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
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
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
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
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
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

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {

                if (AudioFootsteps != null)
                    AudioFootsteps.Play();
                if (AudioFoley != null)
                    AudioFoley.Play();
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (LandingAudio != null)
                    LandingAudio.Play();

            }
        }
    }
}