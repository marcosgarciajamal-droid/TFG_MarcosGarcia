using UnityEngine;

public class RollerVisual : MonoBehaviour
{
    public float textureSpeed = 1f;
    public RollerConveyor Control;
    private Renderer rend;
    private float offset;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    void Update()
    {
        if (Control == null || !Control.isActive) return;
        float direction = Control.forward ? -1f : 1f;
        offset += Time.deltaTime * textureSpeed * direction;
        rend.material.mainTextureOffset = new Vector2(offset, 0f);
    }
}