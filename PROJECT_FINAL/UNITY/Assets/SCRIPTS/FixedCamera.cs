using UnityEngine;
using System.Collections;

public class CameraViewManager : MonoBehaviour
{
    [Header("Transition Settings")]
    public float transitionSpeed = 2f;
    public bool smoothTransition = true;

    [Header("Views")]
    public Transform vistaGeneral;
    public Transform mainCinta;
    public Transform elevador;

    public Transform shelfA;
    public Transform shelfB;
    public Transform shelfC;

    public Transform evacA;
    public Transform evacB;
    public Transform evacC;

    private Coroutine currentTransition;

    // ===============================
    // FUNCIÓN CENTRAL
    // ===============================
    public void GoToView(Transform target)
    {
        if (target == null)
        {
            Debug.LogWarning("Target view is NULL.");
            return;
        }

        if (currentTransition != null)
            StopCoroutine(currentTransition);

        if (smoothTransition)
            currentTransition = StartCoroutine(SmoothMove(target));
        else
            InstantMove(target);
    }

    // ===============================
    // TRANSICIÓN SUAVE
    // ===============================
    IEnumerator SmoothMove(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                target.position,
                Time.deltaTime * transitionSpeed
            );

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                target.rotation,
                Time.deltaTime * transitionSpeed
            );

            yield return null;
        }

        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    // ===============================
    // TRANSICIÓN INSTANTÁNEA
    // ===============================
    void InstantMove(Transform target)
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    // ===============================
    // FUNCIONES PARA BOTONES UI
    // ===============================
    [ContextMenu("Go To Vista General")]

    public void ViewGeneral() => GoToView(vistaGeneral);


    [ContextMenu("Go To Main Cinta")]
    public void ViewMainCinta() => GoToView(mainCinta);
    [ContextMenu("Go To Elevador")]

    public void ViewElevador() => GoToView(elevador);
        [ContextMenu("Go To Level A")]

    public void ViewShelfA() => GoToView(shelfA);
        [ContextMenu("Go To Level B")]

    public void ViewShelfB() => GoToView(shelfB);

            [ContextMenu("Go To Level C")]

    public void ViewShelfC() => GoToView(shelfC);
    [ContextMenu("Go To Evac A")]

    public void ViewEvacA() => GoToView(evacA);
        [ContextMenu("Go To Evac B")]

    public void ViewEvacB() => GoToView(evacB);
        [ContextMenu("Go To Evac C")]

    public void ViewEvacC() => GoToView(evacC);
}