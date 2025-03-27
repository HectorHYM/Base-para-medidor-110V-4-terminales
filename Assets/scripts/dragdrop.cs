using System.Collections;
using System.Collections.Generic; // Para usar List<>
using UnityEngine;

public class dragdrop : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 originalScale;
    private Color originalColor;
    private bool isBeingDragged = false;
    public bool isPlaced = false; // Evita mover el objeto después de colocarlo
    private Plane dragPlane;

    [Tooltip("Lista de tags de destinos exitosos para esta pieza.")]
    public List<string> validDestinations; // Lista de tags válidos
    [Tooltip("Lista de tags de destinos erróneos para esta pieza.")]
    public List<string> invalidDestinations; // Lista de tags inválidos

    public float placementAnimationDuration = 0.5f; // Duración de la animación de colocación
    public float shakeIntensity = 0.05f; // Intensidad del temblor
    public float shakeDuration = 0.3f;  // Duración del temblor

    void Start()
    {
        originalScale = transform.localScale;
        originalColor = GetComponent<Renderer>().material.color;
    }

    void OnMouseDown()
    {
        if (isPlaced) return;

        isBeingDragged = true;
        dragPlane = new Plane(Camera.main.transform.forward, transform.position);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        dragPlane.Raycast(ray, out float enter);
        offset = transform.position - ray.GetPoint(enter);
    }

    void OnMouseDrag()
    {
        if (!isBeingDragged || isPlaced) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 point = ray.GetPoint(enter) + offset;
            point.z = transform.position.z;
            transform.position = point;
        }
    }

    void OnMouseUp()
    {
        if (isPlaced) return;

        isBeingDragged = false;

        CheckPlacement();
    }

    public void CheckPlacement()
    {
        if (isPlaced) return;
    
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (var collider in colliders)
        {
            string tag = collider.tag;
    
            // Si el destino es válido y no está ocupado
            if (validDestinations.Contains(tag))
            {
                Destination destination = collider.GetComponent<Destination>();
                if (destination != null && destination.IsOccupied)
                {
                    // El destino está ocupado, no hacer nada
                    return;
                }
    
                // Colocar el objeto
                StartCoroutine(AnimatePlacement(collider.transform.position));
                if (destination != null)
                {
                    destination.IsOccupied = true; // Marcar destino como ocupado
                }
                return;
            }
    
            // Si el destino es explícitamente inválido
            if (invalidDestinations.Contains(tag))
            {
                StartCoroutine(IncorrectPlacementFeedback());
                return;
            }
    
            // Si el destino no está en ninguna lista, no hacer nada
        }
    }

    public IEnumerator AnimatePlacement(Vector3 destination)
    {
        isPlaced = true;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < placementAnimationDuration)
        {
            transform.position = Vector3.Lerp(startPosition, destination, elapsedTime / placementAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
        StartCoroutine(ShowPlacementFeedback());

         //* Se agregan los puntos en el contador llamando a la función del script que maneja el mismo
        ScoreManager.Instance?.AddPoints(10);
    }

    private IEnumerator ShowPlacementFeedback()
    {
        GetComponent<Renderer>().material.color = Color.green;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Renderer>().material.color = originalColor;
    }

    private IEnumerator IncorrectPlacementFeedback()
    {
        GetComponent<Renderer>().material.color = Color.red;

        Vector3 originalPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float offsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float offsetY = Random.Range(-shakeIntensity, shakeIntensity);
            transform.position = originalPosition + new Vector3(offsetX, offsetY, 0);

            yield return null;
        }

        transform.position = originalPosition;
        GetComponent<Renderer>().material.color = originalColor;
    }

    public void MoveBy(Vector3 movement)
    {
        if (isPlaced) return; // Si ya está colocado, no lo muevas
        transform.position += movement; // Desplaza el objeto
        CheckPlacement(); // Verificar si la nueva posición coloca el objeto en un destino
    }
}