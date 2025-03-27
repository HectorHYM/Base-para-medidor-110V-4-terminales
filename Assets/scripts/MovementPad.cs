using UnityEngine;
using UnityEngine.EventSystems;

public class MovementPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float moveDirection; // 1 para adelante, -1 para atrás
    public float moveSpeed = 0.5f; // Velocidad reducida para un movimiento más preciso

    private bool isButtonPressed = false;

    void Update()
    {
        if (isButtonPressed)
        {
            MoveSelectedObjectInDepth(moveDirection);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonPressed = false;
    }

    private void MoveSelectedObjectInDepth(float direction)
    {
        SelectableObject selectedObject = SelectableObject.GetSelectedObject();
        if (selectedObject == null) return;

        dragdrop dragComponent = selectedObject.GetComponent<dragdrop>();
        if (dragComponent == null || dragComponent.isPlaced) return;

        dragComponent.MoveBy(Vector3.forward * direction * moveSpeed * Time.deltaTime);
    }
}