using UnityEngine;

public class LEDIndicator : MonoBehaviour
{
    [Header("Materials")]
    public Material onMaterial;
    public Material offMaterial;
    private Renderer ledRenderer;
    void Awake()
    {
        ledRenderer = GetComponent<Renderer>();
        SetActive(false);
    }
    public void SetActive(bool active)
    {
        if (ledRenderer == null) return;
        if (active) ledRenderer.material = onMaterial;
        else ledRenderer.material = offMaterial;
    }
}