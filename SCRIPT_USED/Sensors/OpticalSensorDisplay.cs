using UnityEngine;
using TMPro;

public class OpticalSensorDisplay : MonoBehaviour
{
    public OpticalSensor sensor;    
    public TMP_Text displayText;    
    void Update()
    {
        if (!sensor.detected)
        {
            displayText.SetText("No piece");
            return;
        }
        displayText.SetText(sensor.detectedShape.ToString());
    }
}