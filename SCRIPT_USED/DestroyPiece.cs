using UnityEngine;
public class DestroyPiece : MonoBehaviour
{
private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Box")){
            Destroy(other.gameObject);
        }
    }
}
