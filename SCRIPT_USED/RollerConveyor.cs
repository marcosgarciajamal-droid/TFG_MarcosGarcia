using UnityEngine;
public class RollerConveyor : MonoBehaviour
{
    public float speed = 2f; 
    public bool forward = true; 
    public bool isActive = true;
    public int state = 0; 
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Box")) return;
                if (isActive)
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
            {
                Vector3 direction = forward ? transform.forward : -transform.forward;
                Vector3 velocity = direction * speed;
                rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);             }
        }
    }
    void Update(){
        if (!isActive) state = 0;
        else if (forward) state = -1;
        else state = 1;
    }
    public void Active_and_forward(){
        forward = true;
        isActive = true;
    } 
    public void Active_and_not_forward(){
        forward = false;
        isActive = true;
    } 
    public void Stop(){isActive = false;}
}
