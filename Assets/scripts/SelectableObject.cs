using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    private static SelectableObject selectedObject;
    private Renderer objectRenderer;
    private Color originalColor;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
    }

    void OnMouseDown()
    {
        if (selectedObject != null)
        {
            selectedObject.Deselect();
        }
        selectedObject = this;
        Select();
    }

    public void Select()
    {
        objectRenderer.material.color = Color.cyan;
    }

    public void Deselect()
    {
        objectRenderer.material.color = originalColor;
    }

    public static SelectableObject GetSelectedObject()
    {
        return selectedObject;
    }
}