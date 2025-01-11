using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    public Transform[] waypoints; // Hedef noktalar�n listesi
    public float stopDistance = 1f; // Hedef noktaya ula�t���nda durma mesafesi
    public float targetDetectionRange = 10f; // Hedef alg�lama mesafesi
    public NavMeshAgent agent;

    private Transform currentTarget; // �u an takip edilen hedef

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            SetRandomWaypoint(); // �lk rastgele hedef noktay� ayarla
        }
    }

    void Update()
    {
        // E�er agent aktif de�ilse veya NavMesh �zerinde de�ilse i�lem yapma
        if (agent == null || !agent.isOnNavMesh || !agent.enabled)
            return;

        // Hedef tespit et ve pe�ine tak�l
        DetectTarget();

        if (currentTarget != null)
        {
            // E�er bir hedef tespit edilmi�se ona do�ru hareket et
            agent.SetDestination(currentTarget.position);

            // E�er hedef mesafesi "stopDistance" alt�na d��erse hedefi b�rak
            if (Vector3.Distance(transform.position, currentTarget.position) <= stopDistance)
            {
                currentTarget = null; // Hedefi b�rak
                SetRandomWaypoint(); // Waypoint'lere geri d�n
            }
        }
        else
        {
            // E�er hedef yoksa waypoint hareketine devam et
            if (waypoints.Length > 0 && agent.remainingDistance <= stopDistance && !agent.pathPending)
            {
                SetRandomWaypoint(); // Yeni bir rastgele hedef noktaya ge�
            }
        }
    }

    void SetRandomWaypoint()
    {
        // Rastgele bir hedef se�
        int randomIndex = Random.Range(0, waypoints.Length);
        agent.SetDestination(waypoints[randomIndex].position);
    }

    void DetectTarget()
    {
        // �evredeki objeleri tespit et
        Collider[] colliders = Physics.OverlapSphere(transform.position, targetDetectionRange);

        // E�er tespit edilen bir hedef varsa
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider collider in colliders)
        {
            // Kendi tag'ine g�re do�ru hedefi belirle
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

        // E�er uygun bir hedef bulunduysa takip etmeye ba�la
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

    // Gizmos ile alg�lama alan�n� g�rselle�tirmek i�in
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);
    }
}
