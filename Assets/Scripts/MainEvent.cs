using UnityEngine;
using UnityEngine.Events; // IMPORTANTE: Necesario para usar UnityEvent

public class MainEvent : MonoBehaviour
{
    [Header("Configuración")]
    public float segundosDeEspera = 10f;
    
    [Header("¿Qué pasa al terminar el tiempo?")]
    public UnityEvent eventoAlTerminar; // Esto crea la cajita en el inspector

    void Start()
    {
        // Iniciamos la cuenta regresiva apenas empieza el juego
        StartCoroutine(CuentaRegresiva());
    }

    System.Collections.IEnumerator CuentaRegresiva()
    {
        yield return new WaitForSeconds(segundosDeEspera);
        
        // ¡Disparar el evento!
        eventoAlTerminar.Invoke();
    }
}