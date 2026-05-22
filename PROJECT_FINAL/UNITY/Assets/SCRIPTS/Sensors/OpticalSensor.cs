using UnityEngine;

 public class OpticalSensor : MonoBehaviour
{
    public PieceShape detectedShape;
    public bool detected = false;
    private bool detecting = false;
    private PieceData data;
    public LEDIndicator led;
    Timer T_Det;
    [Header("Failure Settings")]
    [Range(0f, 1f)]
    public float failProbability = 0.2f; 
    public bool faultActive = false;
    private bool alreadyProcessed = false; 
    private void OnTriggerEnter(Collider other)
    {
        data = other.GetComponent<PieceData>();
        if (data != null) {
            detecting = true; 
            alreadyProcessed = false; 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        detecting = false; 
        detected = false;
        led.SetActive(false);
        data = null;
    }

    void Start(){T_Det = new Timer { preset = 2f }; }
    void Update()
    {
        T_Det.Update(detecting);
        if (T_Det.done && data != null && !alreadyProcessed)
        {
            alreadyProcessed = true;
            float rand = Random.value;
            if (rand < failProbability)
            {
                faultActive = true;
                detected = false;
                Debug.Log("FALLO SENSOR OPTICO");
            }
            else{
                detectedShape = data.shape;
                detected = true;
                led.SetActive(true);
            }
        }
    }
}