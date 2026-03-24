using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EventTriggerZone : MonoBehaviour
{
    [Header("Configuración del Evento")]
    public bool triggerOnlyOnce = true;
    public bool triggerPresset = true;

    
    [Space(10)]
    [Tooltip("Coloca aquí lo que quieres que pase cuando el jugador entre.")]
    public UnityEvent onPlayerEnter;

    private bool hasTriggered = false;
    private bool OnTriggerZone = false;

    private void Update()
    {
        // Al presionar la E, alternamos con difuminado
        if (Input.GetKeyDown(KeyCode.E) && triggerPresset && OnTriggerZone)
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (triggerOnlyOnce && hasTriggered) 
        {
            return; // Si ya pasó, no hacemos nada más
        }
        Debug.Log("¡Interacción activada!");
        onPlayerEnter.Invoke();
        hasTriggered = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {

            OnTriggerZone = true;
            if (!triggerPresset)
            {
                Interact();
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTriggerZone = false;
        }
    }
}