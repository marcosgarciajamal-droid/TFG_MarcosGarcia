using UnityEngine;

public class PneumaticPiston : MonoBehaviour
{
    [Header("References")]
    public Transform rod;   // El vástago (lo que se mueve)

    [Header("Settings")]
    public float strokeDistance = 0.3f;
    public float speed = 2f;

    [Header("Control")]
    public bool extendCommand = false;

    public bool isRetracted =>
    Vector3.Distance(rod.localPosition, initialLocalPos) < 0.001f;
    [Header("Debug Sensor")]
    [SerializeField] private bool isRetracted_Debug;
    private Vector3 initialLocalPos;
    private Vector3 targetLocalPos;

    void Start()
    {
        initialLocalPos = rod.localPosition;
        targetLocalPos = initialLocalPos;
    }

    void Update()
    {
        rod.localPosition = Vector3.MoveTowards(
            rod.localPosition,
            targetLocalPos,
            speed * Time.deltaTime
        );
        isRetracted_Debug = isRetracted;

    }

    void LateUpdate()
    {
        // Detectar cambio de orden
        if (extendCommand)
            targetLocalPos = initialLocalPos + transform.up * strokeDistance;
        else
            targetLocalPos = initialLocalPos;
    }

    public void Extend()
    {
        extendCommand = true;
    }

    public void Retract()
    {
        extendCommand = false;
    }
}