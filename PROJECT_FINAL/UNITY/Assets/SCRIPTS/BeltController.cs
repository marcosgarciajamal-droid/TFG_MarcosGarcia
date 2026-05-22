using UnityEngine;

public class MotorBelt : MonoBehaviour
{
    [Header("References")] 
    private Renderer beltRenderer; 
    [Header("Parameters")]
    public float beltSpeed = 0.5f;
    public float speed;
    [Header("State")]
    public bool beltEnabled = false;  
    private float textureOffset = 0f;
    void Start(){
    beltRenderer = GetComponent<Renderer>();
}
    void Update() {
        if (!beltEnabled || beltRenderer == null) return; 
        textureOffset += Time.deltaTime * beltSpeed; 
        beltRenderer.material.mainTextureOffset = new Vector2(0f, -textureOffset); 
    }

    void OnTriggerStay(Collider other) {
        if (!beltEnabled) return;
        if (!other.CompareTag("Box")) return;

        Rigidbody rb = other.attachedRigidbody; 
        if (rb == null) return;

        Vector3 direction = Vector3.forward; 
        Vector3 velocity = direction * beltSpeed;

        rb.linearVelocity = new Vector3(velocity.x,rb.linearVelocity.y,velocity.z); 
        speed = rb.linearVelocity.magnitude; 
    }
    public void SetState(bool state) {beltEnabled = state;}
    public void setSpeed(float speed){beltSpeed = speed;}   
}


