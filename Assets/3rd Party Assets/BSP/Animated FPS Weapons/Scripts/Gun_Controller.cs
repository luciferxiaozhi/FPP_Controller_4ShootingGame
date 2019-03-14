using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Controller : MonoBehaviour {
	
public Animator animator;
private bool isZoomed = false;
private bool Idle  = false;
private bool Move = false;
private bool Reload = false;
private bool Fire = false;
private bool EmptyReload = false;
private bool FastMove = false;
private bool ZoomIdle = false;
private bool ZoomMove = false;
private bool ZoomFire = false;
private bool MeleeAttack = false;
private bool Select = false;
private bool PutAway = false;
private bool DryFire = false;
private bool AltToAndFrom = false;
private bool AltFire = false;
private bool AltZoomFire = false;
private bool ReloadLoop = false;
private bool EndReload = false;
private bool AlternateTo = false;
private bool AlternateFrom = false;
private bool GrenadeThrow = false;
private bool SwitchToAlt1;
private bool SwitchToAlt2;
private int clip;
private float CounterSpeed = 2;
public int AmmoQuantity;
public int AmmoReserve;
public float damage = 10f;
public float range = 100f;
public int zoom;
public int normalFOV;
public float zoomSmooth;
public CameraController mainCamera;
public Camera weaponCamera;
public PlayerMovement Player;
public GameObject Impact;
public float impactForce = 25f;
public bool HasAlternateFireMode;
public bool IsShotgun;
public bool isUnarmed;
public GUIStyle mystyle;
public GameObject MuzzleFlash1;
public GameObject MuzzleFlash2;
public GameObject heatDistortion;
public GameObject MuzzleFlashLight;
public ParticleSystem Smoke;
public GameObject ShellEjecting;
public GameObject Crosshair;
public GameObject Collimator;
public GameObject Grenade;
public GameObject GrenadeSlot;
public GameObject Projectile;
public GameObject AmmoIcon1;
public GameObject AmmoIcon2;
public AudioSource FireSound; 
public AudioSource DryFireSound;
public AudioSource ReloadPart1Sound; 
public AudioSource ReloadPart2Sound;
public AudioSource ReloadPart3Sound;
public AudioSource ReloadPart4Sound;
public AudioSource ReloadPart5Sound;
public AudioSource ReloadPart6Sound;
public AudioSource ReloadPart7Sound;
public AudioSource ReloadPart8Sound;
public AudioSource CockSound;
public AudioSource RetrieveSound;
public AudioSource PutawaySound;
public AudioSource ZoomSound;
public AudioSource MeleeSound;
public AudioSource SwitchSound;
public AudioSource BoltPart1Sound;
public AudioSource BoltPart2Sound;
public AudioSource FlapPart1Sound;
public AudioSource FlapPart2Sound;
public AudioSource SlideSound;
public AudioSource RemovingSafetyPin;
public AudioSource Throw;

	void Start () {
	transform.Rotate(0, -180, 0);
	Cursor.visible = false;
	Collimator.SetActive(false);
	MuzzleFlash1.SetActive(true);
    MuzzleFlash2.SetActive(true);
    heatDistortion.SetActive(true);
	AmmoIcon1. SetActive(true);
	AmmoIcon2. SetActive(false);
    ShellEjecting.SetActive(false);
	Grenade.SetActive(false);
	Projectile.SetActive(false);
	mystyle.fontSize = 20;
    mystyle.normal.textColor = Color.white;
	clip = AmmoQuantity;
    }
	
    void Update () {
		
    if (HasAlternateFireMode == true && IsShotgun == false){
	if(Input.GetKeyDown(KeyCode.X)){ // Switch to Alternate Fire Mode
	if(SwitchToAlt1){
    AltToAndFrom = true;
	StartCoroutine(AltToAndFromDelay());
	Idle = true;
	Select = false;
    SwitchToAlt1 = false;
	AmmoIcon1. SetActive(true);
	AmmoIcon2. SetActive(false);
     }
    else {
    AltToAndFrom = true;
	StartCoroutine(AltToAndFromDelay());
	Idle = true;
	Select = false;
    SwitchToAlt1 = true;
	AmmoIcon1. SetActive(false);
	AmmoIcon2. SetActive(true);
   }
	}
    }
if (HasAlternateFireMode == true && IsShotgun == true){
	if(Input.GetKeyDown(KeyCode.X) && Move == false && Fire == false && FastMove == false
	&& Reload == false && EmptyReload == false){ // Switch to Alternate Fire Mode (Shotgun)
	if(SwitchToAlt2){
	StartCoroutine(AltToAndFromDelay());
	Idle = true;
	Select = false;
    SwitchToAlt2 = false;
	AlternateTo = false;
	AlternateFrom = true;
	AmmoIcon1. SetActive(true);
	AmmoIcon2. SetActive(false);
     }
    else {
	AlternateTo = true;
	AlternateFrom = false;
	StartCoroutine(AltToAndFromDelay());
	Idle = true;
	Select = false;
    SwitchToAlt2 = true;
	AmmoIcon1. SetActive(false);
	AmmoIcon2. SetActive(true);
   }
	}
    }
if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)){ 
Idle = false;
	PutAway = true;
	Select = false;
	Fire = false;
	AltFire = false;
    }
if (Input.GetKey(KeyCode.W) //Move 
	   || Input.GetKey(KeyCode.A)
       || Input.GetKey(KeyCode.D) 
	   || Input.GetKey(KeyCode.S)
	   || Input.GetKey(KeyCode.UpArrow)
	   || Input.GetKey(KeyCode.DownArrow)
	   || Input.GetKey(KeyCode.LeftArrow)
	   || Input.GetKey(KeyCode.RightArrow)) {   
        Move = true;
        Idle = false;
  		Fire = false;
		AltFire = false;
		FastMove = false;
		Select = false;
    }
     else if (Input.GetKeyUp(KeyCode.W) 
		|| Input.GetKeyUp(KeyCode.A) 
	    || Input.GetKeyUp(KeyCode.D) 
		|| Input.GetKeyUp(KeyCode.S)
		|| Input.GetKeyUp(KeyCode.UpArrow)
	    || Input.GetKeyUp(KeyCode.DownArrow)
	    || Input.GetKeyUp(KeyCode.LeftArrow)
	    || Input.GetKeyUp(KeyCode.RightArrow)){  
        Move = false;
        Idle = true;
		Fire = false;
		AltFire = false;
		FastMove = false;
    }
if (Input.GetKey(KeyCode.W) && Player.isJumping == true 
	   || Input.GetKey(KeyCode.A) && Player.isJumping == true
       || Input.GetKey(KeyCode.D) && Player.isJumping == true
	   || Input.GetKey(KeyCode.S) && Player.isJumping == true
	   || Input.GetKey(KeyCode.UpArrow) && Player.isJumping == true
	   || Input.GetKey(KeyCode.DownArrow) && Player.isJumping == true
	   || Input.GetKey(KeyCode.LeftArrow) && Player.isJumping == true
	   || Input.GetKey(KeyCode.RightArrow) && Player.isJumping == true) {   
        Move = true;
        Idle = false;
		Fire = false;
		AltFire = false;
		FastMove = false;
    }
if (Input.GetKeyDown(KeyCode.R) && clip == AmmoQuantity || AmmoReserve == 0){   //Reload
        Reload = false;
		EmptyReload = false;
        Idle = true;
   	    } 
else if (Input.GetKeyDown(KeyCode.R) && clip >0){
        Reload = true;
        Idle = false;
		Fire = false;
		AltFire = false;
   	    }
 if (Input.GetMouseButtonUp(0)){ 
		DryFire = false;
        Idle = true;
        Move = false;;
		Fire = false;
		AltFire = false;
    }
	if (Input.GetMouseButtonUp(0) && Input.GetKey(KeyCode.W)){ 
        Idle = false;
        Move = true;
		Fire = false;
		AltFire = false;
    }
	if (Input.GetMouseButton(0) && Input.GetKeyDown(KeyCode.W)){ //Fire
		if (SwitchToAlt1 == false){
        Idle = false;
        Move = false;
		Fire = true;
		Select = false;
    }
	else 
	{
	    Idle = false;
        Move = false;
		AltFire = true;
		Select = false;
	}
		}
if (Input.GetMouseButton(0) && clip == 0 && AmmoReserve > 0) { 
		StartCoroutine(StopFireSound());
		isZoomed = false;
		Move = false;
		Fire = false;
		AltFire = false;
		EmptyReload = true;
		Idle = false;
		Select = false;
    }
else if (Input.GetMouseButton(0) && clip > 0){
if (SwitchToAlt1 == false){
EmptyReload = false;	
Fire = true;
Select = false;
}
else{
if (Input.GetMouseButtonDown(0) && clip > 0){
EmptyReload = false;	
AltFire = true;
Select = false;
}
}
}
if (Input.GetMouseButton(0) && clip == 0 && AmmoReserve == 0) { 
		StartCoroutine(StopFireSound());
		Move = false;
		Fire = false;
		AltFire = false;
		EmptyReload = false;
		Idle = false;
	    DryFire = true;
    }
else if (Input.GetMouseButtonUp(0)){
	EmptyReload = false;
	Idle = true;
}
 if (Input.GetMouseButtonDown(0) && clip == 0 && AmmoReserve == 0) { 
        FireSound.Stop();
		DryFireSound.Play();
		
}
	if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && Player.isCrouching == false && Player.isJumping == false //FastMove
		|| Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A) && Player.isCrouching == false && Player.isJumping == false
	    || Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D) && Player.isCrouching == false && Player.isJumping == false
		|| Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S) && Player.isCrouching == false && Player.isJumping == false
		|| Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.UpArrow) && Player.isCrouching == false && Player.isJumping == false
		|| Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.DownArrow) && Player.isCrouching == false && Player.isJumping == false
		|| Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftArrow) && Player.isCrouching == false && Player.isJumping == false
		|| Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightArrow) && Player.isCrouching == false && Player.isJumping == false){   
		Move = false;
		FastMove = true;
		Reload = false;
        Idle = false;
		Fire = false;
		AltFire = false;
		Select = false;
          }
	else if (Input.GetKeyUp(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) 
		|| Input.GetKeyUp(KeyCode.LeftShift) && Input.GetKey(KeyCode.A)
	    || Input.GetKeyUp(KeyCode.LeftShift) && Input.GetKey(KeyCode.D)
		|| Input.GetKeyUp(KeyCode.LeftShift) && Input.GetKey(KeyCode.S)
		|| Input.GetKeyUp(KeyCode.LeftShift) && Input.GetKey(KeyCode.UpArrow)
		|| Input.GetKeyUp(KeyCode.LeftShift) && Input.GetKey(KeyCode.DownArrow)
		|| Input.GetKeyUp(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftArrow)
		|| Input.GetKeyUp(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightArrow)){  
        Idle = false;
        Move = true;
		FastMove = false;
    }
if (Input.GetMouseButtonDown(1) && FastMove == false && MeleeAttack == false 
    && Reload == false && EmptyReload == false){   //Zoom
	isZoomed = true;
        ZoomIdle = true;
		Idle = false;
		Crosshair.SetActive(false);
		Select = false;
		if (EmptyReload == false){
		ZoomSound.Play();
		}
		
    }
	else  if (Input.GetMouseButtonUp(1) && FastMove == false && MeleeAttack == false
	   && Reload == false && EmptyReload == false){ 
	   isZoomed = false;
         ZoomIdle = false;	
		 Idle = true;
		 Crosshair.SetActive(true);
		 ZoomSound.Play();
    }	
	if (Input.GetMouseButton(0) && Input.GetMouseButton(1) && clip > 0) {   //Zoom Auto Fire
	 if (SwitchToAlt1 == false){
		ZoomFire = true;
		AltZoomFire  = false;
		Fire = false;
		AltFire = false;
        ZoomIdle = false;
		ZoomMove = false;
		Move = false;
		Idle = false;
		Select = false;
		Crosshair.SetActive(false);
	 }
	 else{
		if (Input.GetMouseButtonDown(0) && Input.GetMouseButton(1) && clip > 0){
		ZoomFire = false;
		AltZoomFire  = true;
		Fire = false;
		AltFire = false;
        ZoomIdle = false;
		ZoomMove = false;
		Move = false;
		Idle = false;
		Select = false;
		Crosshair.SetActive(false); 
	 }
	}
    }
	else if (Input.GetMouseButtonUp(0) && Input.GetMouseButton(1)){ 
         ZoomFire = false;
		 AltZoomFire  = false;
		 ZoomIdle = true;	
		 Idle = false;
		 Crosshair.SetActive(false);
    }	
if (Input.GetMouseButton(0) && Input.GetMouseButtonUp(1)){ 
         ZoomFire = false;
		 AltZoomFire = false;
		 ZoomIdle = false;	
		 Idle = true;
		 Crosshair.SetActive(false);
		 }
if (Input.GetMouseButton(0) && Input.GetMouseButton(1) && clip == 0 && AmmoReserve == 0) { 
		StartCoroutine(StopFireSound());
		Move = false;
		ZoomFire = false;
		AltZoomFire = false;
		EmptyReload = false;
		ZoomIdle = true;
    }
if (Input.GetMouseButtonDown(0) && Input.GetMouseButton(1) && clip == 0 && AmmoReserve == 0) { 
        FireSound.Stop();
		DryFireSound.Play();

}
if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.W) //Zoom Move 
		|| Input.GetMouseButton(1) && Input.GetKey(KeyCode.A)
	    || Input.GetMouseButton(1) && Input.GetKey(KeyCode.D)
		|| Input.GetMouseButton(1) && Input.GetKey(KeyCode.S)){   
		ZoomMove = true;
		Fire = false;
		AltFire = false;
		Move = false;
		ZoomFire = false;
		AltZoomFire = false;
        ZoomIdle = false;
		Idle = false;
		Crosshair.SetActive(false);
		Reload = false;
		EmptyReload = false;
		}
else if (Input.GetMouseButton(1) && Input.GetKeyUp(KeyCode.W)
		  || Input.GetMouseButton(1) && Input.GetKeyUp(KeyCode.A)
	      || Input.GetMouseButton(1) && Input.GetKeyUp(KeyCode.D)
	      || Input.GetMouseButton(1) && Input.GetKeyUp(KeyCode.S)){  
	     ZoomMove = false;
         ZoomFire = false;
		 AltZoomFire = false;
		 ZoomIdle = true;	
		 Idle = false;
		 Move = false;
		 Crosshair.SetActive(false);
        }
    else if (Input.GetMouseButtonUp(1) && Input.GetKey(KeyCode.W)
		  || Input.GetMouseButtonUp(1) && Input.GetKey(KeyCode.A)
	      || Input.GetMouseButtonUp(1) && Input.GetKey(KeyCode.D)
		  || Input.GetMouseButtonUp(1) && Input.GetKey(KeyCode.S)){  
	     ZoomMove = false;
         ZoomFire = false;
		 AltZoomFire = false;
		 ZoomIdle = false;	
		 Idle = false;
		 Move = true;
		 Crosshair.SetActive(false);
        }
if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.W) && Input.GetMouseButton(0)
	    || Input.GetMouseButton(1) && Input.GetKey(KeyCode.D) && Input.GetMouseButton(0)
		|| Input.GetMouseButton(1) && Input.GetKey(KeyCode.S) && Input.GetMouseButton(0)
		|| Input.GetMouseButton(1) && Input.GetKey(KeyCode.A) && Input.GetMouseButton(0)){ 
        if (SwitchToAlt1 == false){		
		ZoomMove = false;
		ZoomFire = true;
		Move = false;
        ZoomIdle = false;
		Idle = false;
		Crosshair.SetActive(false);
		}
		else
		{
		ZoomMove = false;
		AltZoomFire = true;
		Move = false;
		ZoomFire = true;
        ZoomIdle = false;
		Idle = false;
		Crosshair.SetActive(false);	
		}
    }
if (Input.GetMouseButton(0) && Input.GetMouseButton(1) && clip == 0 && AmmoReserve > 0) { 
		StartCoroutine(StopFireSound());
		Move = false;
		ZoomFire = false;
		AltZoomFire = false;
		EmptyReload = true;
		Idle = false;
    }	

if (Input.GetKeyDown(KeyCode.LeftAlt)){   //Melee Attack
        MeleeAttack = true;
		Idle = false;
		Select = false;
    }
	else if (Input.GetKeyUp(KeyCode.LeftAlt)){  
        MeleeAttack = false;
		Idle = true;
		}
if (Input.GetKeyDown(KeyCode.G) && GrenadeSlot.GetComponent<GrenadeSlot>().grenadeQuantity > 0){   //Grenade Throw

        GrenadeThrow = true;
		Idle = false;
		Select = false;
    }
	else if (Input.GetKeyUp(KeyCode.G)){  
        GrenadeThrow = false;
		Idle = true;
		}
	if(Idle== true) {
        animator.SetBool("Idle", true);
		animator.SetBool("AltToAndFrom", false);
		animator.SetBool("AlternateTo", false);
		animator.SetBool("AlternateFrom", false);
        animator.SetBool("Move", false);
		animator.SetBool("DryFire", false);
		animator.SetBool("FastMove", false);
        animator.SetBool("Reload", false);
		animator.SetBool("ReloadLoop", false);
		animator.SetBool("EndReload", false);
		animator.SetBool("Fire", false);
		animator.SetBool("AltFire", false);
		animator.SetBool("EmptyReload", false);
		animator.SetBool("ZoomIdle", false);
		animator.SetBool("MeleeAttack", false);
		animator.SetBool("GrenadeThrow", false);
		animator.SetBool("Select", false);
		
    }
if(Move == true) {
        animator.SetBool("Move", true);
        animator.SetBool("Idle", false);
		animator.SetBool("FastMove", false);
        animator.SetBool("Reload", false);
		animator.SetBool("Fire", false);
		animator.SetBool("EmptyReload", false);
		animator.SetBool("MeleeAttack", false);
		animator.SetBool("GrenadeThrow", false);
		animator.SetBool("ZoomMove", false);
		animator.SetBool("ZoomIdle", false);
		animator.SetBool("DryFire", false);
		Crosshair.SetActive(true);
		Collimator.SetActive(false);
    }
if(Reload == true) {
        animator.SetBool("Reload", true);
        animator.SetBool("Move", false);
		animator.SetBool("FastMove", false);
        animator.SetBool("Idle", false);
		animator.SetBool("Fire", false);
		animator.SetBool("EndReload", false);
		animator.SetBool("ReloadLoop", false);
		animator.SetBool("EmptyReload", false);
		animator.SetBool("MeleeAttack", false);
		animator.SetBool("GrenadeThrow", false);
    }
	    if(Fire == true) {
        animator.SetBool("Fire", true);
        animator.SetBool("Move", false);
        animator.SetBool("Idle", false);
		animator.SetBool("Select", false);
		animator.SetBool("FastMove", false);
		animator.SetBool("Reload", false);
		animator.SetBool("EmptyReload", false);
		animator.SetBool("MeleeAttack", false);
		animator.SetBool("GrenadeThrow", false);
    }
		if(EmptyReload == true) {
        animator.SetBool("EmptyReload", true);
        animator.SetBool("Move", false);
        animator.SetBool("Idle", false);
		animator.SetBool("FastMove", false);
		animator.SetBool("EndReload", false);
		animator.SetBool("ReloadLoop", false);
		animator.SetBool("ZoomMove", false);
		animator.SetBool("ZoomIdle", false);
		animator.SetBool("Fire", false);
		animator.SetBool("Reload", false);
		animator.SetBool("MeleeAttack", false);
		animator.SetBool("GrenadeThrow", false);
    }
	    if(FastMove == true) {
		animator.SetBool("FastMove", true);	
        animator.SetBool("EmptyReload", false);
        animator.SetBool("Move", false);
        animator.SetBool("Idle", false);
		animator.SetBool("Fire", false);
		animator.SetBool("Reload", false);
		animator.SetBool("MeleeAttack", false);
		animator.SetBool("GrenadeThrow", false);
		 }
	if(ZoomIdle== true) {
        animator.SetBool("ZoomIdle", true);
		animator.SetBool("FastMove", false);
		animator.SetBool("Idle", false);
		animator.SetBool("ZoomFire", false);
		animator.SetBool("ZoomMove", false);
		animator.SetBool("AltZoomFire", false);
	}
	if(ZoomFire == true) {
		animator.SetBool("ZoomFire", true);
		animator.SetBool("AltZoomFire", false);
        animator.SetBool("ZoomIdle", false);
		animator.SetBool("Idle", false);
		animator.SetBool("ZoomMove", false);
}
	if(ZoomMove == true) {
		animator.SetBool("ZoomFire", false);
		animator.SetBool("AltZoomFire", false);
        animator.SetBool("ZoomIdle", false);
		animator.SetBool("Idle", false);
		animator.SetBool("ZoomMove", true);
		animator.SetBool("Move", false);
}
	if(MeleeAttack == true) {
		animator.SetBool("Idle", false);
		animator.SetBool("MeleeAttack", true);
		}
	if(GrenadeThrow == true) {
		animator.SetBool("Idle", false);
		animator.SetBool("GrenadeThrow", true);
		}
if(Select == true) {
		animator.SetBool("Select", true);
		animator.SetBool("PutAway", false);
		GrenadeSlot.SetActive(true);
		}
if(PutAway == true) {
	    animator.SetBool("PutAway", true);
	    animator.SetBool("Fire", false);
		animator.SetBool("Idle", false);
		animator.SetBool("Select", false);
		animator.SetBool("FastMove", false);
}
if(DryFire == true) {
		animator.SetBool("DryFire", true);
		animator.SetBool("Fire", false);
		animator.SetBool("ZoomFire", false);
		animator.SetBool("Idle", false);
		animator.SetBool("EmptyReload", false);
		}
 if(AlternateTo == true) {
	    animator.SetBool("AlternateTo", true);
		animator.SetBool("AlternateFrom", true);
		animator.SetBool("Idle", false);
		}
 if(AlternateFrom == true) {
	    animator.SetBool("AlternateFrom", true);
	    animator.SetBool("AlternateTo", false);
		animator.SetBool("Idle", false);
		}
 if(AltToAndFrom == true) {
	    animator.SetBool("AltToAndFrom", true);
		animator.SetBool("Idle", false);
		}
if(ReloadLoop == true) {
	    animator.SetBool("ReloadLoop", true);
		animator.SetBool("EndReload", false);
		animator.SetBool("Reload", false);
		animator.SetBool("Idle", false);
		animator.SetBool("Fire", false);
		animator.SetBool("Move", false);
}
if(EndReload == true) {
	    animator.SetBool("EndReload", true);
        animator.SetBool("Reload", false);
        animator.SetBool("Move", false);
		animator.SetBool("FastMove", false);
        animator.SetBool("Idle", false);
		animator.SetBool("Fire", false);
		animator.SetBool("ReloadLoop", false);
		animator.SetBool("EmptyReload", false);
		animator.SetBool("MeleeAttack", false);
		animator.SetBool("GrenadeThrow", false);
}
if(AltFire == true) {
	    animator.SetBool("AltFire", true);
        animator.SetBool("Fire", false);
        animator.SetBool("Move", false);
        animator.SetBool("Idle", false);
		animator.SetBool("Select", false);
		animator.SetBool("FastMove", false);
		animator.SetBool("Reload", false);
		animator.SetBool("EmptyReload", false);
		animator.SetBool("MeleeAttack", false);
		animator.SetBool("GrenadeThrow", false);
    }
if(AltZoomFire == true) {
	    animator.SetBool("AltZoomFire", true);
		animator.SetBool("ZoomFire", false);
        animator.SetBool("ZoomIdle", false);
		animator.SetBool("Idle", false);
		animator.SetBool("ZoomMove", false);
}
		if (gameObject.activeSelf == true)
	{
		GrenadeSlot.SetActive(true);
	}
	else
	{
	GrenadeSlot.SetActive(false);	
	}
	if (isZoomed)
		{
			weaponCamera.fieldOfView = Mathf.Lerp(weaponCamera.fieldOfView, zoom, Time.deltaTime * zoomSmooth);
		}
else
{
	weaponCamera.fieldOfView = Mathf.Lerp(weaponCamera.fieldOfView, normalFOV, Time.deltaTime * zoomSmooth);
}
if (Player.isCrouching == true || Player.isJumping)
{
	animator.SetFloat("Speed", 0.5f);
}
else
{
	animator.SetFloat("Speed", 1.0f);
}	
}
void OnGUI () {
	if (isUnarmed == false)
	{
	GUI.Label (new Rect (20,Screen.height - 40,100,50), clip+"/"+AmmoReserve,mystyle);
	}
	
}
void AddAmmo(){
  int totalAmmo = clip + AmmoReserve;
if (totalAmmo <=AmmoQuantity) {
    StartCoroutine(Wait());
	StartCoroutine(WaitForReserve2());
}
else
{
StartCoroutine(WaitForClip());	
StartCoroutine(WaitForReserve1());
}
}
void AddShotgunAmmo(){
clip +=1;
AmmoReserve -=1;
if(clip == AmmoQuantity || AmmoReserve == 0){
ReloadLoop = false;
EndReload = true;
}
}
IEnumerator WaitForClip(){
	
	yield return new WaitForSeconds (1.0f);
	clip = AmmoQuantity;
	
}
IEnumerator WaitForReserve1(){
    int shotsFired = AmmoQuantity-clip;
	yield return new WaitForSeconds (1.0f);
	AmmoReserve -= shotsFired;
}
IEnumerator WaitForReserve2(){
	yield return new WaitForSeconds (1.0f);
	AmmoReserve = 0;
}
void PlayFireSound()
{
	FireSound.Play();
}
IEnumerator StopFireSound()
{
yield return new WaitForSeconds(1);
FireSound.Stop();
}
void PlayReloadPart1Sound()
{
	ReloadPart1Sound.Play();
}
void PlayReloadPart2Sound()
{
	ReloadPart2Sound.Play();
}
void PlayReloadPart3Sound()
{
	ReloadPart3Sound.Play();
}
void PlayReloadPart4Sound()
{
	ReloadPart4Sound.Play();
}
void PlayReloadPart5Sound()
{
	ReloadPart5Sound.Play();
}
void PlayReloadPart6Sound()
{
	ReloadPart6Sound.Play();
}
void PlayReloadPart7Sound()
{
	ReloadPart7Sound.Play();
}
void PlayReloadPart8Sound()
{
	ReloadPart8Sound.Play();
}
void PlayCockSound()
{
	CockSound.Play();
}
void PlayMeleeSound()
{
	MeleeSound.Play();
	
}
IEnumerator Wait(){
    int totalAmmo = clip + AmmoReserve;
    yield return new WaitForSeconds (1.0f);
	clip = totalAmmo;
}
void PlayPutawaySound()
{
	PutawaySound.Play();
	
}
void PlayRetrieveSound()
{
	RetrieveSound.Play();
	
}
void PlaySwitchSound(){
	SwitchSound.Play();
}
void PlayBoltPart1Sound(){
	BoltPart1Sound.Play();
}
void PlayBoltPart2Sound(){
	BoltPart2Sound.Play();
}
void PlayFlapPart1Sound(){
	FlapPart1Sound.Play();
}
void PlayFlapPart2Sound(){
	FlapPart2Sound.Play();
}
void PlaySlideSound(){
	SlideSound.Play();
}
void PlayRemovingSafetyPin(){
	RemovingSafetyPin.Play();
}
void PlayThrow(){
	Throw.Play();
}
IEnumerator AltToAndFromDelay(){
	yield return new WaitForSeconds (0.2f);
	AltToAndFrom = false;
	AlternateTo = false;
}
void DisableSelectAnim(){
	Select = false;
	PutAway = false;
	Idle = true;
	Crosshair.SetActive (true);
}
void AltFireToIdle(){
	AltFire = false;
	Fire = false;
	Idle = true;
    }
void AltZoomFireToIdle(){
	AltZoomFire = false;
	ZoomIdle = true;
}
void ReloadToIdle(){
Reload = false;
Idle = true;
}
IEnumerator AddSingleFireEffects(){
MuzzleFlash1.SetActive(true);
MuzzleFlash2.SetActive(true);
heatDistortion.SetActive(true);
MuzzleFlashLight.SetActive(true);
ShellEjecting.SetActive(true);
yield return new WaitForSeconds (0.05f);
MuzzleFlash1.SetActive(false);
MuzzleFlash2.SetActive(false);
heatDistortion.SetActive(false);
MuzzleFlashLight.SetActive(false);
ShellEjecting.SetActive(false);
Smoke.Emit(1);
HitATarget();
}
IEnumerator ThrowGrenade(){
Grenade.SetActive(true);
GrenadeSlot.GetComponent<GrenadeSlot>().grenadeQuantity -= 1;
yield return new WaitForSeconds (0.01f);
Grenade.SetActive(false);
}
IEnumerator AddProjectile(){
Projectile.SetActive(true);
yield return new WaitForSeconds (0.01f);
Projectile.SetActive(false);
}
IEnumerator HitTargetWhenZoomed(){
mainCamera.canShake = true;
yield return new WaitForSeconds (0.05f);
mainCamera.canShake = false;
HitATarget();
}
void AutoFireAmmoCounter(){
if (Input.GetMouseButton(0) && clip >0){
	CounterSpeed -=1;
	if (CounterSpeed == 0) {
	clip -= 1;
	CounterSpeed = 2;
	}	
}
}
void SingleFireAmmoCounter(){
if (Input.GetMouseButton(0) && clip >0){
	clip -= 1;	
}
}
void ShowCollimator(){
Collimator.SetActive(true);
}
void HideCollimator(){
Collimator.SetActive(false);
}
void PlayReloadLoop(){
Reload = false;
ReloadLoop = true;
}
void EndReloadToIdle(){
EndReload = false;
Idle = true;
}
void AltToChangeLayerWeight(){
animator.SetLayerWeight(1, 1);
AlternateTo = false;
Idle = true;
}
void AltFromChangeLayerWeight(){
animator.SetLayerWeight(1, 0);
AlternateFrom = false;
Idle = true;
}
void PlayFootsteps()
{
	Player.Footsteps();
}
void HitATarget ()
	{
		
		RaycastHit hit;
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range))
		{
			DestructibleObject target = hit.transform.GetComponent<DestructibleObject>();
			if (target != null)
			{
				target.TakeDamage(damage);
			}
			
			if (hit.rigidbody != null)
			{
				hit.rigidbody.AddForce(-hit.normal * impactForce);
			}
			
			GameObject impactObject = Instantiate(Impact, hit.point, Quaternion.LookRotation(hit.normal));
			Destroy(impactObject, 2f);
		}
		
	}
public void Deactivation(){
AmmoIcon1. SetActive(true);
AmmoIcon2. SetActive(false);
Collimator.SetActive(false);
isZoomed = false;
PutAway = false;
Idle = false;
Move = false;
Reload = false;
Fire = false;
EmptyReload = false;
FastMove = false;
ZoomIdle = false;
ZoomMove = false;
ZoomFire = false;
MeleeAttack = false;
GrenadeThrow = false;
Select = false;
DryFire = false;
AltToAndFrom = false;
AltFire = false;
AltZoomFire = false;
ReloadLoop = false;
EndReload = false;
AlternateTo = false;
AlternateFrom = false;
SwitchToAlt1 = false;
SwitchToAlt2 = false;
Crosshair.SetActive (false);
gameObject.SetActive (false);
GrenadeSlot.SetActive(false);
}
}