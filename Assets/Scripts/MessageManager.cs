using UnityEngine;
using System.Collections.Generic; 
using System.Collections;
using TMPro; 

public class MessageManager : MonoBehaviour
{
    [Header("Configuración del Mensaje")]
    [SerializeField] public List<string> messages;
    public TextMeshProUGUI messageText;
    public GameObject messagePanel;
    
    [Header("Difuminado y Tiempo")]
    [Tooltip("El componente CanvasGroup que debe estar en el MessagePanel")]
    public CanvasGroup panelCanvasGroup; 
    public float fadeDuration = 0.5f; // Tiempo que tarda en aparecer/desaparecer
    public float displayDuration = 3f;

    private int currentMessageIndex = 0;
    private Coroutine currentFadeCoroutine; // Para controlar que no se solapen los difuminados

    private void Update()
    {
        // Al presionar la F, alternamos con difuminado
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (messagePanel.activeSelf && panelCanvasGroup.alpha > 0)
            {
                // Si está visible, lo ocultamos
                Transicion(0f, false);
            }
            else
            {
                // Si está oculto, lo mostramos
                Transicion(1f, true);
            }
        }
    }

    public void NextMessage()
    {
        if (currentMessageIndex >= messages.Count)
        {
            return; 
        }
        if (messagePanel.activeSelf && panelCanvasGroup.alpha > 0)
        {
            // Si está visible, lo ocultamos
            Transicion(0f, false);
        }
        else
        {
            // Si está oculto, lo mostramos
            Transicion(1f, true);
        }
        messageText.text = messages[currentMessageIndex];
        currentMessageIndex++;
        
        // Detenemos cualquier difuminado previo para que no parpadee
        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        
        // Iniciamos la secuencia automática: Aparecer -> Esperar -> Desaparecer -> Apagar Script
        currentFadeCoroutine = StartCoroutine(SecuenciaMensajeAutomatico());
    }

    // --- CORRUTINAS DE DIFUMINADO ---

    private IEnumerator SecuenciaMensajeAutomatico()
    {
        messagePanel.SetActive(true);

        // 1. Difuminado de entrada (Fade In)
        yield return StartCoroutine(HacerFade(1f));

        // 2. Esperamos el tiempo de lectura
        yield return new WaitForSeconds(displayDuration);

        // 3. Difuminado de salida (Fade Out)
        yield return StartCoroutine(HacerFade(0f)); 

    }

    // Función auxiliar para el botón F
    private void Transicion(float targetAlpha, bool encenderPanel)
    {
        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        
        if (encenderPanel) messagePanel.SetActive(true);
        
        currentFadeCoroutine = StartCoroutine(HacerFade(targetAlpha, !encenderPanel));
    }

    // La corrutina que hace el trabajo matemático de cambiar la opacidad poco a poco
    private IEnumerator HacerFade(float alphaObjetivo, bool apagarPanelAlTerminar = false)
    {
        float alphaInicial = panelCanvasGroup.alpha;
        float tiempo = 0f;

        while (tiempo < fadeDuration)
        {
            tiempo += Time.deltaTime;
            // Lerp calcula la transición suave entre el alpha inicial y el objetivo
            panelCanvasGroup.alpha = Mathf.Lerp(alphaInicial, alphaObjetivo, tiempo / fadeDuration);
            yield return null;
        }

        // Aseguramos que llegue exactamente al valor final
        panelCanvasGroup.alpha = alphaObjetivo;

        // Si estábamos ocultando con la F, apagamos el panel para que no estorbe los clics
        if (apagarPanelAlTerminar)
        {
            messagePanel.SetActive(false);
        }
    }
}