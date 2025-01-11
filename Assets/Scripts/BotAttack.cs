using UnityEngine;

public class BotAttack : MonoBehaviour
{
    public float attackRange = 10f; // Botlar�n sald�r� menzili
    public float attackRate = 1f; // Botlar�n sald�r� h�z� (saniyede bir at��)
    public LayerMask targetLayer; // Hedeflerin katman�
    public string enemyTag; // Hedef botun tag'i ("CT" veya "T")

    private float nextAttackTime = 0f;
    private BotController botController; // Bot hareket kontrol� i�in referans
    private Animator animator; // Botun animasyonu i�in referans
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
            RotateTowardsTarget(); // Hedefe do�ru d�n

            // Hedef menzil d���na ��km��sa sald�r�y� durdur
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
        // Botun �evresindeki d��manlar� alg�la
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange, targetLayer);

        if (enemiesInRange.Length > 0)
        {
            foreach (Collider enemy in enemiesInRange)
            {
                if (enemy.CompareTag(enemyTag))
                {
                    currentTarget = enemy.transform; // Mevcut hedefi belirle

                    // Hedef menzil i�indeyse sald�r
                    if (Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
                    {
                        BotHealth botHealth = enemy.GetComponent<BotHealth>();
                        PlayerHealth playerHealth = enemy.GetComponent<PlayerHealth>();

                        Vector3 hitPoint = currentTarget.position; // �rnek bir hitPoint, uygun �ekilde ayarlanmal�
                        Vector3 hitNormal = transform.forward; // �rnek bir hitNormal, uygun �ekilde ayarlanmal�

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
            StopAttacking(); // E�er menzilde d��man yoksa sald�rmay� durdur
        }
    }

    void AttackEnemy(BotHealth enemyHealth, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (enemyHealth != null)
        {
            botController.StopWalking();

            animator.SetBool("Fire", true);

            PlayAttackSound();

            // Rastgele hasar de�eri hesapla (15-30 aras�nda)
            int damage = Random.Range(15, 31);
            enemyHealth.TakeDamage(damage, hitPoint, hitNormal, gameObject);

            Debug.Log($"Attacked {enemyHealth.gameObject.name} for {damage} damage!");

            // Bir sonraki sald�r� i�in zamanlay�c�y� ayarla
            nextAttackTime = Time.time + attackRate;

            // E�er hedef �ld�yse
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

            animator.SetBool("Fire", true); // Ate� animasyonunu ba�lat

            PlayAttackSound();

            // Rastgele hasar de�eri hesapla (15-30 aras�nda)
            int damage = Random.Range(15, 31);
            playerHealth.TakeDamage(damage, hitPoint, hitNormal);

            Debug.Log($"Attacked {playerHealth.gameObject.name} for {damage} damage!");

            // Bir sonraki sald�r� i�in zamanlay�c�y� ayarla
            nextAttackTime = Time.time + attackRate;

            // E�er hedef �ld�yse
            if (playerHealth.currentHealth <= 0)
            {
                botController.EnabledWalking();
                StopAttacking();
                Debug.Log($"{playerHealth.gameObject.name} �ld�r�ld�!");
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
        // Hedef mevcutsa ona do�ru d�n
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        direction.y = 0; // Y eksenindeki fark� s�f�rla, b�ylece bot dikeyde d�nmez

        if (direction.magnitude > 0.1f) // Hedefe do�ru d�nerken gereksiz d�n��� engelle
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Yumu�ak d�n��
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

    // Sald�r� menzilini sahnede g�rselle�tirmek i�in
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}