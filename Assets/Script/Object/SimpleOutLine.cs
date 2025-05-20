using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SimpleOutLine : MonoBehaviour
{
    private Renderer _renderer;
    private Material _originalMaterial;

    public Material outlineMaterial;  // 에디터에서 할당

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _originalMaterial = _renderer.material;
    }

    public void EnableOutline()
    {
        if (outlineMaterial != null)
            _renderer.material = outlineMaterial;
    }

    public void DisableOutline()
    {
        _renderer.material = _originalMaterial;
    }
}