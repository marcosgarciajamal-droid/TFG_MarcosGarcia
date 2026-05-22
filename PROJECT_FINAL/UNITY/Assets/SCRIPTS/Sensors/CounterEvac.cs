using UnityEngine;

public class CounterEvac : MonoBehaviour
{
    public int evacuationCountA = 0;
    public int evacuationCountB = 0;
    public int evacuationCountC = 0;
    private int localCount = 0;
    public Counter linkedShelf1;
    public Counter linkedShelf2;
    public Counter linkedShelf3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            localCount++;
            if (localCount == 9 && other.GetComponent<PieceData>().shape == PieceShape.Square)
            {
                evacuationCountA++;
                localCount = 0;
                if (linkedShelf1 != null)
                linkedShelf1.ResetCounter();
            }
            if (localCount == 9 && other.GetComponent<PieceData>().shape == PieceShape.Cylinder)
            {
                evacuationCountB++;
                localCount = 0;
                if (linkedShelf2 != null)
                linkedShelf2.ResetCounter();
            }
            if (localCount == 9 && other.GetComponent<PieceData>().shape == PieceShape.Triangle)
            {
                evacuationCountC++;
                localCount = 0;
                if (linkedShelf3 != null)
                linkedShelf3.ResetCounter();

            }

       }
    }
}