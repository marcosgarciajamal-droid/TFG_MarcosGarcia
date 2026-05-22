using UnityEngine;

public class ASRSVertical : MonoBehaviour
{
    [Header("Levels")]
    public Transform level0;
    public Transform level1;
    public Transform level2;
    public Transform level3;
    public LEDIndicator led1;
    public LEDIndicator led2;
    public LEDIndicator led3;
    public LEDIndicator led4;
    [Header("Settings")]
    public float speed = 2f;
    private Rigidbody rb;
    private Vector3 targetLocalPosition;
    public bool Level0 => Vector3.Distance(transform.localPosition, level0.localPosition) < 0.1f;
    public bool Level1 => Vector3.Distance(transform.localPosition, level1.localPosition) < 0.1f;
    public bool Level2 => Vector3.Distance(transform.localPosition, level2.localPosition) < 0.1f;
    public bool Level3 => Vector3.Distance(transform.localPosition, level3.localPosition) < 0.1f;
    public bool IsMoving { get; private set; }
    void Start() {
        rb = GetComponent<Rigidbody>();
        targetLocalPosition = level0.localPosition; 
    }

    void FixedUpdate(){
        Vector3 newLocalPos = Vector3.MoveTowards(transform.localPosition,targetLocalPosition, speed * Time.fixedDeltaTime);
        Vector3 newGlobalPos = transform.parent.TransformPoint(newLocalPos);
        rb.MovePosition(newGlobalPos);
        IsMoving = Vector3.Distance(transform.localPosition, targetLocalPosition) > 0.01f;
    }
    public void MoveToLevel(int level)
    {
        switch(level)
        {
            case 0: targetLocalPosition = level0.localPosition; break;
            case 1: targetLocalPosition = level1.localPosition; break;
            case 2: targetLocalPosition = level2.localPosition; break;
            case 3: targetLocalPosition = level3.localPosition; break;
        }
    }

    void Update()
    {
        if (led1 != null) led1.SetActive(Level0);
        if (led2 != null) led2.SetActive(Level1);
        if (led3 != null) led3.SetActive(Level2);
        if (led4 != null) led4.SetActive(Level3);
    }

}

