using UnityEngine;
using UnityEngine.UI;

public class GunControllerDeagle : MonoBehaviour
{
    public int damage = 10;
    public int maxAmmo = 6;
    public int currentAmmo;
    public int totalAmmo = 24;

    public float range = 100f;
    public float fireRate = 0.5f; // Deagle için ateþ hýzý (yarý otomatik)
    private float nextFireTime = 0f;

    public Camera playerCamera;
    public ParticleSystem muzzleFlashPrefab;
    public Transform muzzleEffectSpawnPoint;
    public Animator animator;

    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioSource audioSource;

    public Text ammoText;

    private bool canShoot = true;
    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();
        UpdateAmmoUI();
    }

    void Update()
    {
        // Fire1'e basýldýðýnda ateþ et
        if (Input.GetButtonDown("Fire1") && canShoot && !isReloading && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                Reload();
            }
        }

        // Yeniden yükleme iþlemi (R tuþu)
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && totalAmmo > 0 && !isReloading)
        {
            Reload();
        }
    }

    void Shoot()
    {
        PlayMuzzleEffect(); // MuzzleEffect'i spawn et ve oynat
        audioSource.PlayOneShot(shootSound);

        animator.SetTrigger("Fire"); // Animasyonu tetikle

        currentAmmo--;
        UpdateAmmoUI();

        nextFireTime = Time.time + fireRate; // Bir sonraki ateþ zamaný

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            BotHealth botHealth = hit.transform.GetComponent<BotHealth>();
            if (botHealth != null)
            {
                botHealth.TakeDamage(damage, hit.point, hit.normal, gameObject);
            }
        }

        if (currentAmmo <= 0 && totalAmmo > 0)
        {
            Reload();
        }
    }

    void PlayMuzzleEffect()
    {
        if (muzzleFlashPrefab != null && muzzleEffectSpawnPoint != null)
        {
            // Muzzle Effect'i oluþtur ve silahýn namlusuna (spawn point) sabitle
            ParticleSystem muzzleEffect = Instantiate(muzzleFlashPrefab, muzzleEffectSpawnPoint.position, muzzleEffectSpawnPoint.rotation, muzzleEffectSpawnPoint);

            muzzleEffect.Play();

            // Efekti sahneden 1 saniye sonra kaldýr
            Destroy(muzzleEffect.gameObject, 0.1f);
        }
    }

    void Reload()
    {
        if (totalAmmo <= 0) return;

        isReloading = true;
        animator.SetTrigger("Reload"); // Animasyonu tetikle
        audioSource.PlayOneShot(reloadSound);

        int ammoToReload = maxAmmo - currentAmmo;
        if (totalAmmo >= ammoToReload)
        {
            currentAmmo += ammoToReload;
            totalAmmo -= ammoToReload;
        }
        else
        {
            currentAmmo += totalAmmo;
            totalAmmo = 0;
        }

        UpdateAmmoUI();

        canShoot = false;
        Invoke("ResetShoot", 3.2f);
        Invoke("FinishReload", 3.2f);
    }

    void ResetShoot()
    {
        canShoot = true;
    }

    void FinishReload()
    {
        isReloading = false;
    }

    public void UpdateAmmoUI()
    {
        ammoText.text = $"{currentAmmo}/{totalAmmo}";
    }
}