using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BotHealth : MonoBehaviour
{
    private NavMeshAgent agent;

    public int maxHealth = 100;
    public int currentHealth;

    public Animator animator;

    public BotAttack botAttack;

    void Start()
    {
        botAttack = GetComponent<BotAttack>();

        agent = GetComponent<NavMeshAgent>();

        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal, GameObject attacker)
    {
        // Friendly fire kontrolü
        if (attacker != null && attacker.CompareTag(gameObject.tag))
        {
            return; // Eðer saldýran ve hedef ayný tag'e sahipse hasar uygulanmaz
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Invoke("GameWin", 3f);

            Die();
        }
    }

    void Die()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            animator.ResetTrigger("Run");
        }

        animator.SetTrigger("Dead");

        if (botAttack != null)
        {
            Debug.Log("StopAttacking is Trigged");

            botAttack.StopAttacking();
        }

        agent.enabled = false;

        Destroy(gameObject, 2f);
    }
}