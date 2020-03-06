using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour {
    private AudioSource audioSource;
    private bool fireLock = false;
    private Vector3 originalPosition;
    private HolographicSight holographicSight;
    private LaserSight laserSight;
    private Silencer silencer;

    [Header("Weapon")]
    public float damage = 20f;
    public float fireRate = 0.2f;
    public float range = 100f;

    [Header("Iron Sight")]
	public Vector3 ironSightOffset;
	public float ironSightSpeed = 10f;

    [Header("Object References")]
	public ParticleSystem muzzleFlash;

    [Header("Audio References")]
	public AudioClip fireSound;

    void Start() {
        originalPosition = transform.localPosition;

        audioSource = GetComponent<AudioSource>();
        holographicSight = GetComponent<HolographicSight>();
        laserSight = GetComponent<LaserSight>();
        silencer = GetComponent<Silencer>();

        bool hologramSightAttached = PlayerPrefs.GetInt("HologramSight", 0) == 1;
        bool laserSightAttached = PlayerPrefs.GetInt("LaserSight", 0) == 1;
        bool silencerAttached = PlayerPrefs.GetInt("Silencer", 0) == 1;

        if (hologramSightAttached) {
            holographicSight.Enable();
        }
        if (laserSightAttached) {
            laserSight.Enable();
        }
        if (silencerAttached) {
            silencer.Enable();
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Fire();
        }
        
        HandleAim();
    }

    void Fire() {
        if (fireLock) return;
        
        fireLock = true;
        Invoke("ResetFireLock", fireRate);

        muzzleFlash.Stop();
        muzzleFlash.Play();

        if (silencer.active) {
            audioSource.PlayOneShot(silencer.fireSound);    
        }
        else {
            audioSource.PlayOneShot(fireSound);
        }

        float finalDamage = damage;

        if (silencer.active) {
            finalDamage += silencer.value;
        }

        float finalRange = range;

        if (holographicSight.active) {
            finalRange += holographicSight.value;
        }
        if (laserSight.active) {
            finalRange += laserSight.value;
        }

        print("Damage: " + finalDamage);
        print("Range: " + finalRange);
    }

    void ResetFireLock() {
        fireLock = false;
    }

    void HandleAim() {
        bool shouldAim = Input.GetMouseButton(1);
        Vector3 targetPosition;

        if (shouldAim) {
            if (holographicSight.active) {
                targetPosition = holographicSight.overrideOffset;
            }
            else {
                targetPosition = ironSightOffset;
            }
        }
        else {
            targetPosition = originalPosition;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, ironSightSpeed * Time.deltaTime);
    }
}
