using UnityEngine;

public class ShelfSlotBeltAuto : MonoBehaviour
{   

    [Header("Manual Activation")]
    public bool beltEnabled = true;   // ← NUEVA VARIABLE
    [Header("Belt Settings")]
    public float beltSpeed = 2f;

    [Header("Next Slot")]
    public ShelfSlotBeltAuto nextSlot;

    [Header("Visual")]
    private Renderer rend;
    public float textureSpeed = 2f;

    private float textureOffset = 0f;

    [Header("State")]
    public bool blockBelt = false; // ← NUEVA VARIABLE
    public bool sensorBlocked = false;
    public Rigidbody storedBox;

  
void Start()
{
    rend = GetComponent<Renderer>();
}
    void Update()
    {
        bool shouldMove = ShouldMove();

        if (shouldMove)
        {
            textureOffset += Time.deltaTime * textureSpeed;
            rend.material.mainTextureOffset = new Vector2(0f, -textureOffset);
        }
    }

    bool ShouldMove()
    {
        if (blockBelt) return false; // ← NUEVA CONDICIÓN
        if (beltEnabled) return true;          
        if (!sensorBlocked) return false;
        if (nextSlot == null) return false;
        if (nextSlot.sensorBlocked) return false;

        return true;
    }

    void OnTriggerStay(Collider other)
    {
        if (!ShouldMove()) return;

        if (!other.CompareTag("Box")) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        Vector3 moveDirection = -transform.forward * beltSpeed;

        rb.linearVelocity = new Vector3(
            moveDirection.x,
            rb.linearVelocity.y,
            moveDirection.z
        );
    }


    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Box")) return;

        sensorBlocked = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Box")) return;

        sensorBlocked = false;
    }


}

