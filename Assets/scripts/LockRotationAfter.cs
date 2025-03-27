using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class LockRotationAfter : MonoBehaviour
{
    private ObserverBehaviour mObserverBehaviour;
    private bool isLocked = false;

    // Variables para almacenar la rotación y posición fijas
    public Vector3 fixedPosition; // La posición fija que se quiere
    public Vector3 fixedRotation; // La rotación fija que se quiere

    void Start()
    {
        mObserverBehaviour = GetComponent<ObserverBehaviour>();
        if (mObserverBehaviour)
        {
            mObserverBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnDestroy()
    {
        if (mObserverBehaviour)
        {
            mObserverBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        if ((targetStatus.Status == Status.TRACKED || targetStatus.Status == Status.EXTENDED_TRACKED) && !isLocked)
        {
            LockPositionAndRotation();
        }
    }

    private void LockPositionAndRotation()
    {
        isLocked = true;
        transform.position = fixedPosition;
        transform.rotation = Quaternion.Euler(fixedRotation); // Solo se establece una vez
    }
}