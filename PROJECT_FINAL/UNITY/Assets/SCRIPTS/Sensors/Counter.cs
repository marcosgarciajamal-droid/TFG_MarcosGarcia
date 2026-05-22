using UnityEngine;

public class Counter : MonoBehaviour
{
    public int pieceCount = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            pieceCount++;
        }
    }
    public void ResetCounter()
    {
        pieceCount = 0;
    }
}