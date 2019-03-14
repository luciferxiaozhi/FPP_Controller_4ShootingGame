using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController playerControl;
    public float walkSpeed;
    public float runSpeed;
	public float crouchSpeed;
	public Camera cam;
	public float jumpSpeed = 10.0f;
	public float gravity = 20.0f;
	public bool isCrouching = false;
	public bool isJumping = false;
	public bool isZooming = false;
	public AudioClip[] concreteSurface;
	public AudioClip[] grassSurface;
	private AudioSource randomSound;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 cameraStartPos;
	private Vector3 cameraEndPos;
	private float cameraMaxPosY = -2f;
	private float lerpTime = 0.2f;
	private float currentLerpTime1;
	private float currentLerpTime2;

	void Start()
    {
        playerControl = GetComponent<CharacterController>();
		cameraStartPos = cam.transform.localPosition;
		cameraEndPos = cam.transform.localPosition + Vector3.up * cameraMaxPosY;
		randomSound = GetComponent<AudioSource>();
	}

    void Update()
    {
        Movement();
		Jump();
		Crouch();
		}

    void Movement()
    {
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
		if (isCrouching == false)
		{
        Vector3 moveDirSide = transform.right * horiz * walkSpeed;
        Vector3 moveDirForward = transform.forward * vert * walkSpeed;
		playerControl.SimpleMove(moveDirSide);
        playerControl.SimpleMove(moveDirForward);
		}
		if (isCrouching == true)
		{
		Vector3 moveDirSide = transform.right * horiz * crouchSpeed;
        Vector3 moveDirForward = transform.forward * vert * crouchSpeed;
		playerControl.SimpleMove(moveDirSide);
        playerControl.SimpleMove(moveDirForward);	
		}
		if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && isCrouching == false && isJumping == false && isZooming == false
	   || Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift) && isCrouching == false && isJumping == false && isZooming == false
       || Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift) && isCrouching == false && isJumping == false && isZooming == false
	   || Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift) && isCrouching == false && isJumping == false && isZooming == false
	   || Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift) && isCrouching == false && isJumping == false && isZooming == false
	   || Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftShift) && isCrouching == false && isJumping == false && isZooming == false
	   || Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftShift) && isCrouching == false && isJumping == false && isZooming == false
	   || Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftShift) && isCrouching == false && isJumping == false && isZooming == false)
		{
		Vector3 moveDirSide = transform.right * horiz * runSpeed;
        Vector3 moveDirForward = transform.forward * vert * runSpeed;
		playerControl.SimpleMove(moveDirSide);
        playerControl.SimpleMove(moveDirForward);
		}
		if (Input.GetMouseButtonDown(1))
		{
			isZooming = true;
		}
		else if (Input.GetMouseButtonUp(1))
		{
			isZooming = false;
		}
    }
		void Jump()
		{
		if (Input.GetKeyDown(KeyCode.Space) && isCrouching == false && isZooming == false)
		{
			isJumping = true;
		}
		else if (Input.GetKeyUp(KeyCode.Space))
		{
			isJumping = false;
		}
		if (playerControl.isGrounded) {
            moveDirection = new Vector3(0, 0, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= jumpSpeed;
            if (isJumping == true)
             moveDirection.y = jumpSpeed*2f;

        }
        moveDirection.y -= gravity * Time.deltaTime;
        playerControl.Move(moveDirection * Time.deltaTime);
		
    }
	
	void Crouch()
	{
	float Perc1 = currentLerpTime1/lerpTime;
    float Perc2 = currentLerpTime2/lerpTime;	
    if (Input.GetKeyDown(KeyCode.C))
	{
		if (isCrouching)
		{
			isCrouching = false;
		}
		else
		{
			isCrouching = true;
		}
	}
	if (isCrouching == true && cam.transform.localPosition.y > -2f)
	{
		currentLerpTime2 = 0;
		currentLerpTime1 += Time.deltaTime;
		cam.transform.localPosition = Vector3.Lerp(cameraStartPos, cameraEndPos, Perc1);
	}
	
	if (isCrouching == false && cam.transform.localPosition.y < 0f)
	
	{
		currentLerpTime1 = 0;
		currentLerpTime2 += Time.deltaTime;
		cam.transform.localPosition = Vector3.Lerp(cameraEndPos,cameraStartPos, Perc2);
	}
   }
   
    public void Footsteps()
     {
		 RaycastHit hit = new RaycastHit();
		 string floortag;
		if(playerControl.isGrounded == true)
		{
		if(Physics.Raycast(transform.position, Vector3.down,out hit ))
        {
		floortag = hit.collider.gameObject.tag;
		if (floortag == "Concrete")
		{
         randomSound.clip = concreteSurface[Random.Range(0, concreteSurface.Length)];
		 randomSound.Play ();
		}
		else if (floortag == "Grass")
		{
         randomSound.clip = grassSurface[Random.Range(0, grassSurface.Length)];
		 randomSound.Play ();
		}
		}
		}
     }

}