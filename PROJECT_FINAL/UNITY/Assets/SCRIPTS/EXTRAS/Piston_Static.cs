using UnityEngine;

public class Piston_Static : MonoBehaviour
{
    [Header("References")]
    // Referencia a la parte móvil del pistón (el vástago)
    public Transform rod;

    [Header("Motion Settings")]
    // Distancia total que recorrerá el pistón al extenderse
    public float strokeDistancia = 7f;

    // Velocidad de desplazamiento del pistón
    public float strokeSpeed = 2f;

    [Header("External Control")]
    // Señal externa de control (pensada para MQTT en el futuro)
    // true = extender, false = retraer
    public bool extendCommand = false;

    public bool isRetracted =>
    Vector3.Distance(rod.position, initialRodPosition) < 0.001f;
    [Header("Debug Sensor")]
    [SerializeField] private bool isRetracted_Debug;
    // Referencia al Rigidbody del rod (necesario para moverlo físicamente)
    private Rigidbody rb;

    // Posición inicial del rod (posición retraída)
    private Vector3 initialRodPosition;

    // Estado interno del pistón (extendido o retraído)
    private bool isExtending = false;

    // Indica si el pistón está actualmente en movimiento
    private bool isMoving = false;

    void Start()
    {
        // Obtenemos el Rigidbody del rod
        rb = rod.GetComponent<Rigidbody>();

        // Guardamos la posición inicial como referencia base
        initialRodPosition = rod.position;
    }

    void FixedUpdate()
    {

        // Detecta si ha cambiado la orden externa
        if (extendCommand != isExtending)
        {
            isExtending = extendCommand;
            isMoving = true;
        }

        // Si no debe moverse, salimos
        if (!isMoving) return;

        // Calculamos la posición destino según estado
        Vector3 targetPosition = isExtending
            ? initialRodPosition + Vector3.forward * strokeDistancia
            : initialRodPosition;

        // Calculamos nueva posición progresiva
        Vector3 newPosition = Vector3.MoveTowards(
            rod.position,
            targetPosition,
            strokeSpeed * Time.deltaTime
        );

        // Movimiento físico correcto usando Rigidbody
        rb.MovePosition(newPosition);

        // Comprobación de llegada al destino
        if (Vector3.Distance(rod.position, targetPosition) < 0.001f)
        {
            rb.MovePosition(targetPosition);
            isMoving = false;
        }
    }

    void Update()
{
    isRetracted_Debug = isRetracted;
}
    // Función pública para extender el pistón
    public void ActivatePiston()
    {
        extendCommand = true;
    }

    // Función pública para retraer el pistón
    public void DeactivatePiston()
    {
        extendCommand = false;
    }
}