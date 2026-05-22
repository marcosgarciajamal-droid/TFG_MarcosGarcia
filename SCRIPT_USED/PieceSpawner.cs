using UnityEngine;
public class PieceSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject pieceA;
    public GameObject pieceB;
    public GameObject pieceC;
    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    [Header("Probabilities")]
    [Range(0f,1f)] public float probA = 0.5f;
    [Range(0f,1f)] public float probB = 0.3f;
    [Range(0f,1f)] public float probC = 0.2f;
    [Header("State")]
    public bool SystemRunning = true;
    private Timer spawnTimer;
    void Start()
    {
        spawnTimer = new Timer {preset = spawnInterval};
    }
    void Update()
    {
        spawnTimer.preset = spawnInterval;
        spawnTimer.Update(SystemRunning);
        if (spawnTimer.done)
        {
            SpawnPiece();
            spawnTimer.Update(false); 
            spawnTimer.Update(SystemRunning); 
        }
    }
    public void SpawnPiece()
    {
        float randomValue = Random.value;
        if (randomValue < probA) Instantiate(pieceA, transform.position, Quaternion.Euler(-90,0,0));
        else if (randomValue < probA + probB) Instantiate(pieceB, transform.position, Quaternion.identity);
        else Instantiate(pieceC, transform.position, Quaternion.identity);
    }
}