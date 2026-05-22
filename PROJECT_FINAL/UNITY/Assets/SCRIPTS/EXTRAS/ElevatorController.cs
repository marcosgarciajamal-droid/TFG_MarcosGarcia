using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [Header("References")]
    // Plataforma móvil del elevador
    public Transform platform;

    // Posiciones físicas que representan cada nivel
    public Transform pos0;
    public Transform pos1;
    public Transform pos2;
    public Transform pos3;

    public bool S4_Level0 => Vector3.Distance(rb.position, pos0.position) < 0.01f;
    public bool S5_Level1 => Vector3.Distance(rb.position, pos1.position) < 0.01f;
    public bool S6_Level2 => Vector3.Distance(rb.position, pos2.position) < 0.01f;
    public bool S7_Level3 => Vector3.Distance(rb.position, pos3.position) < 0.01f;

    [Header("Debug Sensors (Read Only)")]
    [SerializeField] private bool s4_Level0_Debug;
    [SerializeField] private bool s5_Level1_Debug;
    [SerializeField] private bool s6_Level2_Debug;
    [SerializeField] private bool s7_Level3_Debug;
    [Header("Parameters")]
    // Velocidad de desplazamiento del elevador
    public float speed = 2f;

    // Rigidbody de la plataforma (movimiento físico)
    private Rigidbody rb;

    // Destino actual del elevador
    private Transform destiny;

    // Indica si el elevador está en movimiento
    private bool isMoving = true;

    void Start()
    {
        // Se obtiene el Rigidbody solo una vez (optimización)
        rb = platform.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Si no hay destino o no debe moverse, salir
        if (!isMoving || destiny == null) return;

        // Movimiento progresivo hacia el destino
        Vector3 newPosition = Vector3.MoveTowards(
            rb.position,
            destiny.position,
            speed * Time.fixedDeltaTime
        );

        // Movimiento físico correcto
        rb.MovePosition(newPosition);

        // Si llega al destino, se ajusta exactamente
        if (Vector3.Distance(rb.position, destiny.position) < 0.001f)
            rb.MovePosition(destiny.position);
    }
    void Update()
    {
        // Actualización de sensores para debug
        s4_Level0_Debug = S4_Level0;
        s5_Level1_Debug = S5_Level1;
        s6_Level2_Debug = S6_Level2;
        s7_Level3_Debug = S7_Level3;
    }
    // Función genérica para mover a un nivel concreto
    public void MoveToLevel(Transform level)
    {
        destiny = level;
    }

    // Vuelve a la base
    public void ReturnToBase()
    {
        MoveToLevel(pos0);
    }

    // Métodos de prueba ejecutables desde el Inspector

    [ContextMenu("Go To Level A")]
    public void Test_LevelA()
    {
        MoveToLevel(pos1);
    }

    [ContextMenu("Go To Level B")]
    public void Test_LevelB()
    {
        MoveToLevel(pos2);
    }

    [ContextMenu("Go To Level C")]
    public void Test_LevelC()
    {
        MoveToLevel(pos3);
    }

    [ContextMenu("Return To Base")]
    public void Test_Return()
    {
        ReturnToBase();
    }
}