using UnityEngine;

public class BotAttack : MonoBehaviour
{
    public float attackRange = 10f; // Botlarýn saldýrý menzili
    public float attackRate = 1f; // Botlarýn saldýrý hýzý (saniyede bir atýþ)
    public LayerMask targetLayer; // Hedeflerin katmaný
    public string enemyTag; // Hedef botun tag'i ("CT" veya "T")

    private float nextAttackTime = 0f;
    private BotController botController; // Bot hareket kontrolü için referans
    private Animator animator; // Botun animasyonu için referans
    private Transform currentTarget; // Mevcut hedef

    public AudioSource audioSource;
    public AudioClip attackSound;

    void Start()
    {
        botController = GetComponent<BotController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (currentTarget != null)
        {
            RotateTowardsTarget(); // Hedefe doðru dön

            // Hedef menzil dýþýna çýkmýþsa saldýrýyý durdur
            if (Vector3.Distance(transform.position, currentTarget.position) > attackRange)
            {
                StopAttacking();
            }
        }

        if (Time.time >= nextAttackTime)
        {
            CheckForEnemyInRange();
        }
    }

    void CheckForEnemyInRange()
    {
        // Botun çevresindeki düþmanlarý algýla
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange, targetLayer);

        if (enemiesInRange.Length > 0)
        {
            foreach (Collider enemy in enemiesInRange)
            {
                if (enemy.CompareTag(enemyTag))
                {
                    currentTarget = enemy.transform; // Mevcut hedefi belirle

                    // Hedef menzil içindeyse saldýr
                    if (Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
                    {
                        BotHealth botHealth = enemy.GetComponent<BotHealth>();
                        PlayerHealth playerHealth = enemy.GetComponent<PlayerHealth>();

                        Vector3 hitPoint = currentTarget.position; // Örnek bir hitPoint, uygun þekilde ayarlanmalý
                        Vector3 hitNormal = transform.forward; // Örnek bir hitNormal, uygun þekilde ayarlanmalý

                        if (botHealth != null)
                        {
                            AttackEnemy(botHealth, hitPoint, hitNormal);
                        }
                        else if (playerHealth != null)
                        {
                            AttackPlayer(playerHealth, hitPoint, hitNormal);
                        }
                    }
                    return;
                }
            }
        }
        else
        {
            StopAttacking(); // Eðer menzilde düþman yoksa saldýrmayý durdur
        }
    }

    void AttackEnemy(BotHealth enemyHealth, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (enemyHealth != null)
        {
            botController.StopWalking();

            animator.SetBool("Fire", true);

            PlayAttackSound();

            // Rastgele hasar deðeri hesapla (15-30 arasýnda)
            int damage = Random.Range(15, 31);
            enemyHealth.TakeDamage(damage, hitPoint, hitNormal, gameObject);

            Debug.Log($"Attacked {enemyHealth.gameObject.name} for {damage} damage!");

            // Bir sonraki saldýrý için zamanlayýcýyý ayarla
            nextAttackTime = Time.time + attackRate;

            // Eðer hedef öldüyse
            if (enemyHealth.currentHealth <= 0)
            {
                botController.EnabledWalking();
                StopAttacking();
            }
        }
    }

    void AttackPlayer(PlayerHealth playerHealth, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (playerHealth != null)
        {
            botController.StopWalking();

            animator.SetBool("Fire", true); // Ateþ animasyonunu baþlat

            PlayAttackSound();

            // Rastgele hasar deðeri hesapla (15-30 arasýnda)
            int damage = Random.Range(15, 31);
            playerHealth.TakeDamage(damage, hitPoint, hitNormal);

            Debug.Log($"Attacked {playerHealth.gameObject.name} for {damage} damage!");

            // Bir sonraki saldýrý için zamanlayýcýyý ayarla
            nextAttackTime = Time.time + attackRate;

            // Eðer hedef öldüyse
            if (playerHealth.currentHealth <= 0)
            {
                botController.EnabledWalking();
                StopAttacking();
                Debug.Log($"{playerHealth.gameObject.name} öldürüldü!");
            }
        }
    }

    void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = attackSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    void RotateTowardsTarget()
    {
        // Hedef mevcutsa ona doðru dön
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        direction.y = 0; // Y eksenindeki farký sýfýrla, böylece bot dikeyde dönmez

        if (direction.magnitude > 0.1f) // Hedefe doðru dönerken gereksiz dönüþü engelle
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Yumuþak dönüþ
        }
    }

    public void StopAttacking()
    {
        currentTarget = null;

        if (botController != null)
        {
            botController.enabled = true;
            if (botController.agent != null)
            {
                botController.agent.enabled = true;
            }
        }

        if (animator != null)
        {
            animator.SetBool("Fire", false);
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Saldýrý menzilini sahnede görselleþtirmek için
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}