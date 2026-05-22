using UnityEngine;

public class LightSystem : MonoBehaviour
{
    public GameObject greenLight;
    public GameObject redLight;
    public GameObject yellowLight;
    public bool emergency = false;
    public bool isOk = false;
    public bool isManual = false;
    float timer = 0f;
    public float blinkSpeed = 2f;

    void Update()
    {
        if (isOk) greenLight.SetActive(true);
        else greenLight.SetActive(false);
        if (emergency){
            timer += Time.deltaTime * blinkSpeed;
            bool on = Mathf.FloorToInt(timer) % 2 == 0;
            redLight.SetActive(on);
        }
        else redLight.SetActive(false);
        if (isManual) yellowLight.SetActive(true);
        else yellowLight.SetActive(false);
    }
}
