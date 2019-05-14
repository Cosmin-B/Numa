using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player_SoundHolder
{
    [Header("Character Controller Sounds")]
    public static string footStepSoundName = "footStepSound";
    public static string jumpSoundName = "jumpSound";
    public static string landSoundName = "landSound";
    public static string runSoundName = "runSound";
    public static string wheelSoundName = "wheel_Sound";
    public static string transitionSoundName = "transitionSound";

    [Header("Collectable Sounds")]
    public static string collectUsableSoundName = "collectBatteryFuel";
    public static string collectItemSoundName = "collectItem";
    public static string collectStoryItemSoundName = "collectStoryItem";
    public static string useItemSoundName = "useBatteryFuel";

    ///  Player Controller Sounds
    public static AudioClip footStepSound = null;
    public static AudioClip jumpSound = null;
    public static AudioClip landSound = null;
    public static AudioClip runSound = null;
    public static AudioClip wheelSound = null;
    public static AudioClip transitionSound = null;

    /// Collectable Sounds
    public static AudioClip collectUsable = null;
    public static AudioClip collectItem = null;
    public static AudioClip collectStoryItem = null;
    public static AudioClip useItemSound = null;

    [HideInInspector]
    public static AudioSource audioSource = null;

    private static Game_SoundManager _soundManager = null;

    public static void Init()
    {
        _soundManager = Game_Manager.Instance.GetSoundManager();

        footStepSound = _soundManager.GetAudioClip(footStepSoundName);
        runSound = _soundManager.GetAudioClip(runSoundName);
        jumpSound = _soundManager.GetAudioClip(jumpSoundName);
        landSound = _soundManager.GetAudioClip(landSoundName);
        wheelSound = _soundManager.GetAudioClip(wheelSoundName);
        transitionSound = _soundManager.GetAudioClip(transitionSoundName);
        collectUsable = _soundManager.GetAudioClip(collectUsableSoundName);
        collectItem = _soundManager.GetAudioClip(collectItemSoundName);
        collectStoryItem = _soundManager.GetAudioClip(collectStoryItemSoundName);
        useItemSound = _soundManager.GetAudioClip(useItemSoundName);
    }
}

public class Player_Controller : MonoBehaviour {

    [Header("Player Settings")]
    public float walkSpeed = 3.5f;
    public float runningSpeed = 4.7f;
    public float wheelModeSpeed = 7.5f;
    public float jumpSpeed = 0.5f;
    public float gravityMultiplier = 0.5f;
    public float cameraOffset = 0.5f;
    public float cameraTransitionSpeed = 2.0f;
    public float cameraLerpTime = 1.2f;

    private float _speed = 0.0f;
    private float _stepCycle = 0.0f;
    private float _nextStepCycle = 0.0f;
    private float _runStepLengthen = 1.0f;
    private float _stepInterval = 4.5f;

    private bool _isPreviouslyJumping = false;
    private bool _isWalking = false;
    private bool _jump = false;
    private bool _isJumping = false;
    private bool _isPreviouslyGrounded = false;
    private bool _isWheelSoundStarted = false;

    private Vector2 _input = Vector2.zero;
    private Vector3 _moveDirection = Vector3.zero;

    private CharacterController _controller;
    private Game_SoundManager _soundManager;

   [SerializeField]
    private MovementType _movementType;

    [HideInInspector]
    public bool isChangeingWalkMode = false;

    //[HideInInspector]
    public bool enableJump = true;

    public enum MovementType
    {
        MOVE_WALK = 1,
        MOVE_WHEEL = 2,
    }

    public static Animator animator;

    // Use this for initialization
    void Start()
    {
        _movementType = MovementType.MOVE_WALK;

        animator = GetComponentInChildren<Animator>();

        _controller = GetComponent<CharacterController>();
        _soundManager = Game_Manager.Instance.GetSoundManager();
        Player_SoundHolder.audioSource = GetComponent<AudioSource>();

        Debug.Assert(animator != null);
        Debug.Assert(_soundManager != null);
        Debug.Assert(_controller != null);

        Physics.gravity = new Vector3(0, -1.0f, 0);

        isChangeingWalkMode = false;

        _nextStepCycle = _stepCycle / 2.0f;
    }

    private void GetInput(out float speed)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _input = new Vector2 (horizontal, vertical);

        _isWalking = !Input.GetKey(KeyCode.LeftShift);

        speed = 0.0f;

        if (_movementType == MovementType.MOVE_WALK)
            speed = _isWalking ? walkSpeed : runningSpeed;
        else if (_movementType == MovementType.MOVE_WHEEL)
            speed = wheelModeSpeed;

        if (_input.magnitude < 0.00001f)
            speed = 0.0f;

        if (_input.magnitude > 1.0f)
            _input.Normalize();
    }

    public void Update()
    {
        if (enableJump && !_jump && _controller.isGrounded)
            _jump = Input.GetButton("Jump");

        if (!_isPreviouslyGrounded && _controller.isGrounded)
        {
            _moveDirection.y = 0f;
            _isJumping = false;
        }

        if (!_isPreviouslyGrounded && !_isJumping && _controller.isGrounded)
        {
            _moveDirection.y = 0f;
        }

        if(!_isPreviouslyGrounded && _isPreviouslyJumping && !_isJumping)
        {
            PlayLandingSound();
            _isPreviouslyJumping = false;
        }

        _isPreviouslyGrounded = _controller.isGrounded;

    }

    private void FixedUpdate()
    {

        float speed;

        GetInput(out speed);

        if (isChangeingWalkMode)
            return;

        bool isWalking = _movementType == MovementType.MOVE_WALK;

        if (!isWalking)
        {
            if (speed <= Mathf.Abs(0.1f))
                _speed = Mathf.Lerp(_speed, speed, Time.fixedDeltaTime * 3.0f);
            else
            {
                _speed = Mathf.Lerp(_speed, speed, Time.fixedDeltaTime);
            }
        }

        if (!_isJumping)
        {
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * _input.y + transform.right * _input.x;

            if (isWalking)
            {
                _moveDirection.x = desiredMove.x * speed;
                _moveDirection.z = desiredMove.z * speed;
            }
            else
            {
                _moveDirection.x = desiredMove.x * _speed;
                _moveDirection.z = desiredMove.z * _speed;
            }

            //// get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, _controller.radius, Vector3.down, out hitInfo,
                               _controller.height / 2.2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            if (isWalking)
            {
                _moveDirection.x = desiredMove.x * speed;
                _moveDirection.z = desiredMove.z * speed;
            }
            else
            {
                _moveDirection.x = desiredMove.x * _speed;
                _moveDirection.z = desiredMove.z * _speed;
            }
        }

        if (_controller.isGrounded)
        {   
            if (_movementType == MovementType.MOVE_WALK)
            {
                if (_jump)
                {
                    _moveDirection.y = jumpSpeed;
                    _jump = false;
                    _isJumping = true;
                    _isPreviouslyJumping = true;
                    _soundManager.PlaySoundForPlayer(Player_SoundHolder.jumpSound);
                }
            }
        }
        else
        {
            _moveDirection += Physics.gravity * gravityMultiplier * Time.deltaTime;
        }

        _controller.Move(_moveDirection * Time.fixedDeltaTime);

        animator.SetFloat("Speed", _controller.velocity.magnitude);

        if (_movementType == MovementType.MOVE_WHEEL)
        {
            if (_speed > 0.1f && !_isWheelSoundStarted)
            {
                _isWheelSoundStarted = true;
                _soundManager.PlaySoundForPlayer(Player_SoundHolder.wheelSound, true);
            }

            if(_isWheelSoundStarted)
                Player_SoundHolder.audioSource.pitch = _speed / wheelModeSpeed;
        }

        if (_isJumping || _movementType != MovementType.MOVE_WALK)
            return;

        ProgressStepCycle(speed);
    }

    private void ProgressStepCycle(float speed)
    {
        if (_controller.velocity.sqrMagnitude > 0 && (_input.x != 0 || _input.y != 0))
        {
            _stepCycle += (_controller.velocity.magnitude + (speed * (_isWalking ? 1f : _runStepLengthen))) *
                         Time.fixedDeltaTime;
        }

        if (!(_stepCycle > _nextStepCycle))
            return;

        _nextStepCycle = _stepCycle + _stepInterval;

        PlayFootStepAudio();
    }

    private void PlayLandingSound()
    {
        _soundManager.PlaySoundForPlayer(Player_SoundHolder.landSound);
        _nextStepCycle = _stepCycle + _stepInterval;
    }

    private void PlayFootStepAudio()
    {
        if (_isJumping || _movementType != MovementType.MOVE_WALK)
            return;

        _soundManager.PlaySoundForPlayer(Player_SoundHolder.footStepSound);
    }

    public MovementType GetMovementType()
    {
        return _movementType;
    }

    public void SetMovementType(MovementType movementType)
    {
        _speed = 0.0f;
        _stepCycle = 0.0f;
        _nextStepCycle = _stepCycle / 2f;
        _isWheelSoundStarted = false;
        Player_SoundHolder.audioSource.pitch = 1.0f;
        //_soundManager.StopPlaySoundForPlayer();
        _jump = false;

        _movementType = movementType;
    }
}
