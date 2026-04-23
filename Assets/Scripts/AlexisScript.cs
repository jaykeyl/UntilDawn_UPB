using UnityEngine;
using UnityEngine.AI;

public class AlexisScript : MonoBehaviour
{
    private NavMeshAgent agent;
    //private PlayerMovement player;
    //private DialogueManager dialogueManager;
    //private Animator animator;

    private float baseSpeed = 0.5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();

        //player = FindAnyObjectByType<PlayerMovement>();
        //dialogueManager = FindAnyObjectByType<DialogueManager>();

        agent.speed = baseSpeed;
    }

    //void Update()
    //{
    //    if (dialogueManager != null && dialogueManager.IsDialogueActive)
    //    {
    //        agent.isStopped = true;

    //        if (animator != null)
    //            animator.SetBool("isWalking", false);

    //        return;
    //    }

    //    agent.isStopped = false;

    //    if (agent.enabled && player != null)
    //    {
    //        agent.destination = player.transform.position;

    //        if (animator != null)
    //            animator.SetBool("isWalking", true);
    //    }
    //}
}