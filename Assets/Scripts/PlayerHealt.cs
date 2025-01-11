using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Animator animator;

    public bool die;

    void Start()
    {
        die = false;

        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        die = true;

        animator.SetTrigger("Dead");

        gameObject.SetActive(false);
        
        Destroy(gameObject, 2f);
    }
}