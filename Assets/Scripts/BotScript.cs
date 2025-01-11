using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    public Transform[] waypoints; // Hedef noktalarýn listesi
    public float stopDistance = 1f; // Hedef noktaya ulaþtýðýnda durma mesafesi
    public float targetDetectionRange = 10f; // Hedef algýlama mesafesi
    public NavMeshAgent agent;

    private Transform currentTarget; // Þu an takip edilen hedef

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            SetRandomWaypoint(); // Ýlk rastgele hedef noktayý ayarla
        }
    }

    void Update()
    {
        // Eðer agent aktif deðilse veya NavMesh üzerinde deðilse iþlem yapma
        if (agent == null || !agent.isOnNavMesh || !agent.enabled)
            return;

        // Hedef tespit et ve peþine takýl
        DetectTarget();

        if (currentTarget != null)
        {
            // Eðer bir hedef tespit edilmiþse ona doðru hareket et
            agent.SetDestination(currentTarget.position);

            // Eðer hedef mesafesi "stopDistance" altýna düþerse hedefi býrak
            if (Vector3.Distance(transform.position, currentTarget.position) <= stopDistance)
            {
                currentTarget = null; // Hedefi býrak
                SetRandomWaypoint(); // Waypoint'lere geri dön
            }
        }
        else
        {
            // Eðer hedef yoksa waypoint hareketine devam et
            if (waypoints.Length > 0 && agent.remainingDistance <= stopDistance && !agent.pathPending)
            {
                SetRandomWaypoint(); // Yeni bir rastgele hedef noktaya geç
            }
        }
    }

    void SetRandomWaypoint()
    {
        // Rastgele bir hedef seç
        int randomIndex = Random.Range(0, waypoints.Length);
        agent.SetDestination(waypoints[randomIndex].position);
    }

    void DetectTarget()
    {
        // Çevredeki objeleri tespit et
        Collider[] colliders = Physics.OverlapSphere(transform.position, targetDetectionRange);

        // Eðer tespit edilen bir hedef varsa
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider collider in colliders)
        {
            // Kendi tag'ine göre doðru hedefi belirle
            if ((CompareTag("CT") && collider.CompareTag("T")) || (CompareTag("T") && collider.CompareTag("CT")))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = collider.transform;
                }
            }
        }

        // Eðer uygun bir hedef bulunduysa takip etmeye baþla
        if (closestTarget != null)
        {
            currentTarget = closestTarget;
        }
    }

    public void StopWalking()
    {
        agent.enabled = false;
    }

    public void EnabledWalking()
    {
        agent.enabled = true;
    }

    // Gizmos ile algýlama alanýný görselleþtirmek için
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);
    }
}
