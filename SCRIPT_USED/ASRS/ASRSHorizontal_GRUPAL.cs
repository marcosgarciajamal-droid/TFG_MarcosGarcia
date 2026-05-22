using UnityEngine;

public class ASRSHorizontal : MonoBehaviour
{
    [Header("Positions")]
    public Transform position1;
    public Transform position2;
    public Transform position3;
    public Transform position4;
    [Header("Settings")]
    public float speed = 2f;
    public bool isMoving {get; private set; }
    public LEDIndicator led1;
    public LEDIndicator led2;
    public LEDIndicator led3;
    public LEDIndicator led4;   
    public bool Pos1 => position1 != null && Vector3.Distance(rb.position, position1.position) < 0.01f;
    public bool Pos2 => position2 != null && Vector3.Distance(rb.position, position2.position) < 0.01f;
    public bool Pos3 => position3 != null && Vector3.Distance(rb.position, position3.position) < 0.01f;
    public bool Pos4 => position4 != null && Vector3.Distance(rb.position, position4.position) < 0.01f;  
    private Rigidbody rb;
    public bool IsMoving { get; private set; }
    private Vector3 targetPosition;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = position1.position;
    }
    void FixedUpdate()
    {
       Vector3 newPosition = Vector3.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
        IsMoving = Vector3.Distance(rb.position, targetPosition) > 0.01f;

         }
    public void MoveToPos(int pos)
    {
        switch(pos)
        {
            case 1: if (position1 != null) targetPosition = position1.position; break;
            case 2: if (position2 != null) targetPosition = position2.position; break;
            case 3: if (position3 != null) targetPosition = position3.position; break;
            case 4: if (position4 != null) targetPosition = position4.position; break;
        }
    }
void Update()
    {
        if (led1 != null) led1.SetActive(Pos1);
        if (led2 != null) led2.SetActive(Pos2);
        if (led3 != null) led3.SetActive(Pos3);
        if (led4 != null) led4.SetActive(Pos4);
    }
}

