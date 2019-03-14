using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    CharacterController _controller;

    private float _speed = normalWalkSpeed;
    [SerializeField] private float _jumpSpeed = 7f;
    [SerializeField] private float _gravity = 20.0f;
    [SerializeField] private float _mouseXSensitivity = 1f;
    [SerializeField] private float _mouseYSensitivity = 1f;
    [SerializeField] private Camera _FPPCamera;
    [SerializeField] private AudioClip[] _footStepSounds;
    [SerializeField] private AudioClip _jumpSounds;
    [SerializeField] private AudioClip _landSounds;
    [SerializeField] private float LeftAndRightwardAngle = 30f;


    private const float normalWalkSpeed = 3.0f;
    private const float slowWalkSpeed = normalWalkSpeed / 2;
    private const float fastWalkSpeed = normalWalkSpeed * 2;
    private float stepRate = normalStepRate;
    private float stepTimer = slowStepRate;
    private float minY = -90f;
    private float maxY = 90f;
    private float rotationX = 0;
    private float rotationY = 0;
    [SerializeField]private float rotationZ = 0;
    private float smoothRotationZ= 8f;
    private float crouchHeight = 0.5f;
    private const float standHeight = 2f;
    private const float normalStepRate = 0.6f;
    private const float fastStepRate = normalStepRate / 2;
    private const float slowStepRate = normalStepRate * 2;
    private AudioSource _audioSource;

    public bool isRunning = false;
    public bool isCrunching = false;
    public bool isJumping = false;
    private bool _PreviouslyGrounded;
    private bool isLeftward = false;
    private bool isRightward = false;

    public Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _controller.height = standHeight;
        _audioSource = GetComponent<AudioSource>();

        // Hide the mouse Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        MoveCursor();

        // leftward and rightward
        if (Input.GetKey(KeyCode.Q))
        {
            Leftward();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rightward();
        }
        else if(Input.GetKeyUp(KeyCode.Q))
        {
            LeftwardRecovery();
        }
        else if(Input.GetKeyUp(KeyCode.E))
        {
            RightwardRecovery();
        }
    }

    private void FixedUpdate()
    {
        /*
        if (!_PreviouslyGrounded && _controller.isGrounded) // just grounded bugged
        {
            Debug.Log("Grounded!!!!");
            PlayLandAudio();
        }
        
        _PreviouslyGrounded = _controller.isGrounded;
        */
    }


    private void Move()
    {
        
        if (_controller.isGrounded) // grounded
        {
            isJumping = false;
            // Crouch
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Crouching();
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                GetUp();
            }

            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            if (Input.GetKey(KeyCode.LeftShift)) // fast move
            {
                isRunning = true;
                _speed = fastWalkSpeed;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift)) // fast move
            {
                _speed = normalWalkSpeed;
                isRunning = false;
            }
            
            moveDirection *= _speed;
            moveDirection = transform.TransformDirection(moveDirection);
            if (Input.GetButton("Jump")) // jump
            {
                isJumping = true;
                PlayJumpAudio();
                moveDirection.y = _jumpSpeed;
            }
            // adjust steprate
            if (isRunning) stepRate = fastStepRate;
            if (isCrunching) stepRate = slowStepRate;
            stepCycle();// a step cycle
            if (stepTimer < stepRate) stepTimer += Time.deltaTime;
            stepRate = normalStepRate;
        }

        moveDirection.y -= _gravity * Time.deltaTime;

        // Move the controller
        _controller.Move(moveDirection * Time.deltaTime);
    }

    private void MoveCursor()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationX += mouseX * _mouseXSensitivity;
        rotationY -= mouseY * _mouseYSensitivity;
        rotationY = Mathf.Clamp(rotationY, minY, maxY); // restrict the range of cursor
        
        transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);

        // unhide cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!Cursor.visible)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void changeRotationY(float valueMin, float valueMax, float smoothSpeed)
    {
        float randomedValue = Random.Range(valueMin, valueMax);
        rotationY = Mathf.Lerp(rotationY, rotationY - randomedValue, Time.deltaTime * smoothSpeed);
    }

    public void changeRotationX(float value, float smoothSpeed)
    {
        float randomedValue = Random.Range(-value, value);
        rotationX = Mathf.Lerp(rotationX, rotationX + randomedValue, Time.deltaTime * smoothSpeed);
    }

    private void Crouching()
    {
        isCrunching = true;
        _speed = slowWalkSpeed;
        _controller.height = crouchHeight;
    }

    private void GetUp()
    {
        isCrunching = false;
        _speed = normalWalkSpeed;
        _controller.height = standHeight;
    }

    private void stepCycle()
    {
        if (stepTimer < stepRate) return; // follow the stepRate

        if (_controller.velocity.sqrMagnitude > 0 && (moveDirection.x != 0 || moveDirection.y != 0))
        {
            PlayFootStepAudio(); // play the foot step sound
        }

        stepTimer = 0f; // reset step timer
    }

    private void PlayFootStepAudio()
    {
        if (isCrunching)
        {
            _audioSource.volume = 0.3f;
        }
        else
        {
            _audioSource.volume = 1f;
        }
        // pick & play a random footstep sound from the array,
        // exclusive index 0
        int n = Random.Range(1, _footStepSounds.Length);
        _audioSource.clip = _footStepSounds[n];
        _audioSource.PlayOneShot(_audioSource.clip);
        // exchange index n and 0
        _footStepSounds[n] = _footStepSounds[0];
        _footStepSounds[0] = _audioSource.clip;
    }

    private void PlayJumpAudio()
    {
        if(isCrunching)
        {
            _audioSource.volume = 0.3f;
        }
        else
        {
            _audioSource.volume = 1f;
        }
        _audioSource.clip = _jumpSounds;
        _audioSource.Play();
    }

    private void PlayLandAudio()
    {
        if (isCrunching)
        {
            _audioSource.volume = 0.3f;
        }
        else
        {
            _audioSource.volume = 1f;
        }
        _audioSource.clip = _landSounds;
        _audioSource.Play();
    }

    private void Leftward()
    {
        isLeftward = true;
        /*
        Vector3 currentRotation = transform.localEulerAngles;
        rotationZ = Mathf.Lerp(0, rotationZ, Time.deltaTime * smoothRotationZ);
        transform.localEulerAngles -= new Vector3(0, 0, rotationZ);
        */
    }

    private void Rightward()
    {
        isRightward = true;
    }

    private void LeftwardRecovery()
    {
        isLeftward = false;
    }

    private void RightwardRecovery()
    {
        isRightward = false;
    }
}
