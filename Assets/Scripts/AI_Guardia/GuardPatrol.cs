using UnityEngine;
using UnityEngine.AI;

public class GuardPatrol : MonoBehaviour
{
    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float waitTimeAtPoint = 2f;

    [Header("Player Detection")]
    public Transform player;
    public Transform playerCamera;

    [Tooltip("Si el jugador entra a esta distancia, el guardia lo detecta aunque esté atrás.")]
    public float proximityDetectionRange = 5f;

    [Tooltip("Distancia máxima para verlo de frente.")]
    public float visionDetectionRange = 8f;

    [Tooltip("Ángulo de visión frontal.")]
    public float fieldOfView = 90f;

    [Header("Chase")]
    public float chaseSpeed = 4.5f;
    public float patrolSpeed = 2.5f;
    public float losePlayerDistance = 12f;

    [Header("Flashlight")]
    public Light flashlight;
    public AudioSource alertAudio;

    private NavMeshAgent agent;
    private int currentPointIndex = 0;
    private float waitTimer;
    private bool isWaiting = false;
    private bool isAlerted = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = patrolSpeed;

        if (flashlight != null)
            flashlight.enabled = true; // siempre encendida

        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[currentPointIndex].position);
    }

    void Update()
    {
        if (player == null) return;
        if (patrolPoints.Length == 0) return;

        CheckPlayerDetection();

        if (isAlerted)
            ChaseMode();
        else
            PatrolMode();
    }

    void CheckPlayerDetection()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // 1. Si está muy cerca, lo detecta aunque esté detrás
        if (distanceToPlayer <= proximityDetectionRange)
        {
            isAlerted = true;
            return;
        }

        // 2. Si está demasiado lejos, no lo detecta
        if (distanceToPlayer > visionDetectionRange)
        {
            // solo deja de perseguir si además ya está suficientemente lejos
            if (distanceToPlayer > losePlayerDistance)
                isAlerted = false;

            return;
        }

        // 3. Detección por visión frontal
        Vector3 flatDirection = new Vector3(directionToPlayer.x, 0f, directionToPlayer.z);
        float angle = Vector3.Angle(transform.forward, flatDirection);

        if (angle <= fieldOfView * 0.5f)
        {
            isAlerted = true;
        }
        else
        {
            // si no está cerca ni en el cono frontal, no alerta,
            // pero solo si además ya no está en persecución útil
            if (distanceToPlayer > proximityDetectionRange)
                isAlerted = false;
        }
    }

    void PatrolMode()
    {
        agent.speed = patrolSpeed;

        if (flashlight != null)
            flashlight.enabled = true; // siempre encendida

        // Si estaba sonando la alerta, la detiene porque vuelve a su estado de patrulla.
        if (alertAudio != null && alertAudio.isPlaying)
            alertAudio.Stop();

        // se asegura de que el agente no esté detenido.
        agent.isStopped = false;

        // cuando llega a un punto, espera unos segundos y luego avanza al siguiente.
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = waitTimeAtPoint;
            }

            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPointIndex].position);
                isWaiting = false;
            }
        }
    }

    void ChaseMode()
    {
        agent.speed = chaseSpeed;

        // Se asegura de que pueda seguir moviéndose.
        agent.isStopped = false;

        // persigue al player viendo su posicion acual
        agent.SetDestination(player.position);

        // hace que el guardia se gire para mirar al player
        Vector3 target = player.position;
        target.y = transform.position.y;
        transform.LookAt(target);

        if (flashlight != null)
            flashlight.enabled = true;

        if (alertAudio != null && !alertAudio.isPlaying)
            alertAudio.Play();

        // si el jugador se aleja demasiado, el guardia deja de perseguirlo y vuelve a patrullar
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > losePlayerDistance)
        {
            isAlerted = false;
            agent.speed = patrolSpeed;
            agent.SetDestination(patrolPoints[currentPointIndex].position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // radio de detección cercana
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, proximityDetectionRange);

        // radio de visión
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionDetectionRange);

        Vector3 left = Quaternion.Euler(0, -fieldOfView / 2f, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, fieldOfView / 2f, 0) * transform.forward;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, left * visionDetectionRange);
        Gizmos.DrawRay(transform.position, right * visionDetectionRange);
    }
}