using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] ConversationTemplate conversation;

    public InputActionReference interactAction;

    private void OnEnable()
    {
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.Disable();
    }

    private void Update()
    {
        if (!interactAction.action.WasPressedThisFrame())
            return;

        if (!dialogueManager.IsDialogueActive)
        {
            dialogueManager.GetConversation(conversation);
        }
        else
        {
            dialogueManager.NextLine();
        }
    }
}