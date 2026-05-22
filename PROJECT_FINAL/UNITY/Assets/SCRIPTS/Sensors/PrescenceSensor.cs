using UnityEngine;

public class PrescenceSensor : MonoBehaviour
{
    public bool isActivated = false;
    [Header("Visual Indicator")]
    public LEDIndicator led; 
    private int objectsInside = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {   
            objectsInside++;
            isActivated = true;
            led.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            objectsInside--;
            if (objectsInside <= 0)
            {
                objectsInside = 0; 
                isActivated = false;
                led.SetActive(false);
            }
        }
    }
}    
