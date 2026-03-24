using UnityEngine;
using UnityEngine.AI; 
using UnityEngine.SceneManagement;
public class EnemyAI : MonoBehaviour
{
    [Header("Referencias de Movimiento")]
    public Transform player; 
    public bool isActive = false; 

    [Header("Animación y Sonido")]
    public Animator animator;       // Referencia al componente Animator
    public AudioSource pasosAudio;  // Referencia al componente AudioSource

    private NavMeshAgent agent;
    private string gameOverSceneName = "GameOver";
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Nos aseguramos de que el sonido esté muteado al empezar si la IA no está activa
        if (pasosAudio != null && !isActive)
        {
            pasosAudio.mute = true;
        }
    }

    void Update()
    {
        if (player != null && isActive)
        {
            agent.SetDestination(player.position);
            animator.SetFloat("Vertical", 1f); 
            // Nota: Aquí también podrías usar la velocidad del agente para la animación
            // animator.SetFloat("Velocidad", agent.velocity.magnitude);
        }
    }

    // Método para activar la IA desde otros scripts
    public void ActivateAI()
    {
        isActive = true;

        // 1. Activar la animación de caminar
        if (animator != null)
        {
            // Asume que tienes un parámetro booleano llamado "isWalking" en tu Animator
            animator.SetFloat("Vertical", 1f); 
        }

        // 2. Desmutear el sonido de los pasos
        if (pasosAudio != null)
        {
            pasosAudio.mute = false;
            
            // Si el sonido no estaba configurado para reproducirse automáticamente, usa esto en su lugar:
            // if (!pasosAudio.isPlaying) pasosAudio.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprobamos si el objeto que acaba de entrar en el Trigger es el Jugador
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡El enemigo ha atrapado al jugador! Cambiando a la escena de Game Over...");
            SceneManager.LoadScene(gameOverSceneName);       
        }
    }
}