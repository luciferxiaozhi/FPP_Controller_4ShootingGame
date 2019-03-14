using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [Header("Properties")]
    public Vector3 aimPosition;
    public int bulletsPerMag = 30; // Bullets per each magazine
    public int bulletsLeft = 200; // total bullets we have
    public int currentBullets; // The current bullets in our magazine
    public float range = 100f;
    public float fireRate = 0.1f;
    public float damage = 20f;
    public float adsSpeed = 8f;
    [SerializeField] public float recoilSmooth = 0.1f;
    public float spreadFactorX = 0.01f;
    public float spreadFactorY = 0.01f;
    public float spreadFactorZ = 0.01f;
    [SerializeField] private float aimView = 50f;
    [SerializeField] private float recoilYValueMin = 9f;
    [SerializeField] private float recoilYValueMax = 12f;
    [SerializeField] private float recoilXValue = 3f;
    private bool isReloading;
    private bool shoutInput;
    private bool isAiming;
    private float normalView;
    private float _fireTimer = 0f;
    private Vector3 originalPosition;

    public enum ShootMode {Auto, Semi};
    public ShootMode shootingMode;

    [Header("Sound Effects")]
    private AudioSource _audioSource;
    public AudioClip shootSound;

    [Header("Setup")]
    public GameObject hitParticles;
    public GameObject bulletImpact;
    public GameObject shootPoint;   
    public ParticleSystem muzzleFlash; // muzzelFlash
    [SerializeField] private Camera _FPPCamera;
    private Animator _anim;
    private Player _player;

    [Header("UI")]
    public Text ammoText;

    private void OnEnable()
    {
        // Update when active state is changed.
        UpdateAmmoText();
    }

    // Use this for initialization
    void Start () {
        _player = GameObject.Find("FPS_Controller").GetComponent<Player>();
        normalView = _FPPCamera.fieldOfView;
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        currentBullets = bulletsPerMag;
        shootingMode = ShootMode.Auto; // initialize shoot mode
        originalPosition = transform.localPosition;

        UpdateAmmoText();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.B))
        {
            ChangeShootMode(); // change shoot mode
        }

        switch (shootingMode)
        {
            case ShootMode.Auto:
                shoutInput = Input.GetMouseButton(0);
                break;
            case ShootMode.Semi:
                shoutInput = Input.GetMouseButtonDown(0);
                break;
        }

		if (shoutInput) // press mouse button 0 and fire
        {
            if (currentBullets > 0)
                Fire();
            else
                DoReload();
        }

        if (Input.GetKeyDown(KeyCode.R)) // Reload
        {
            if (currentBullets < bulletsPerMag)
                DoReload();
        }

        if (_fireTimer < fireRate)
        {
            _fireTimer += Time.deltaTime; // add into time counter
        }

        AimDownSights();
	}

    private void FixedUpdate()
    {
        AnimatorStateInfo info = _anim.GetCurrentAnimatorStateInfo(0); // base layer

        isReloading = info.IsName("Reload");
        _anim.SetBool("Aim", isAiming);
        //if (info.IsName("Fire")) _anim.SetBool("Fire", false); // reset fire _animation to unfire state
    }

    private void Fire()
    {
        if (_fireTimer < fireRate || currentBullets <= 0 || isReloading) return;

        RaycastHit hitInfo;

        Vector3 shootDirection = shootPoint.transform.forward;
        shootDirection.x += Random.Range(-spreadFactorX, spreadFactorX);
        shootDirection.y += Random.Range(-spreadFactorY, spreadFactorY);
        shootDirection.z += Random.Range(-spreadFactorZ, spreadFactorZ);

        if (Physics.Raycast(shootPoint.transform.position, shootDirection, out hitInfo, range)) // hit "hitInfo"
        {
            Debug.Log(hitInfo.transform.name + "found");
            // Spawn a hit partical effect and the bullet effect position
            GameObject hitParticalEffect = Instantiate(hitParticles, hitInfo.point, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
            // Spawn a bullet hole decal at the bullet impact position
            GameObject bulletHole = Instantiate(bulletImpact, hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));

            Destroy(hitParticalEffect, 1.0f);
            Destroy(bulletHole, 2.0f);

            if (hitInfo.transform.GetComponent<objectHealthController>())
            {
                hitInfo.transform.GetComponent<objectHealthController>().ApplyDamage(damage);
            }
        }

        Recoil(); // recoil

        _anim.CrossFadeInFixedTime("Fire", 0.01f); // play fire ainimation
        //_anim.SetBool("Fire", true);
        muzzleFlash.Play(); // Show the muzzle flash
        PlayShootSound(); // Play the shooting sound effects

        currentBullets--; // Deduct one bullet
        UpdateAmmoText(); // Update ammoText
        _fireTimer = 0.0f; // reset fire timer
    }

    private void Recoil()
    {
        _player.changeRotationY(recoilYValueMin, recoilYValueMax, recoilSmooth);
        _player.changeRotationX(recoilXValue, recoilSmooth);
    }

    public void Reload()
    {
        if (bulletsLeft <= 0) return;

        int bulletsToLoad = bulletsPerMag - currentBullets;
        int bulletsToDeduct = bulletsLeft >= bulletsToLoad ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToDeduct;
        currentBullets += bulletsToDeduct;
        UpdateAmmoText();
    }

    private void DoReload()
    {
        // AnimatorStateInfo info = _anim.GetCurrentAnimatorStateInfo(0);
        if (isReloading || bulletsLeft <= 0) return;

        // play reload audio 

        _anim.CrossFadeInFixedTime("Reload", 0.01f);
    }

    private void PlayShootSound()
    {
        _audioSource.PlayOneShot(shootSound);
    }

    private void ChangeShootMode()
    {
        if (shootingMode == ShootMode.Auto)
        {
            shootingMode = ShootMode.Semi;
        }
        else
        {
            shootingMode = ShootMode.Auto;
        }
    }

    private void AimDownSights()
    {
        if (Input.GetMouseButton(1) && !isReloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * adsSpeed);
            _FPPCamera.fieldOfView = aimView;
            isAiming = true;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * adsSpeed);
            _FPPCamera.fieldOfView = normalView;
            isAiming = false;
        }
    }

    private void UpdateAmmoText()
    {
        ammoText.text = currentBullets.ToString() + " / " + bulletsLeft.ToString();
    }
}
