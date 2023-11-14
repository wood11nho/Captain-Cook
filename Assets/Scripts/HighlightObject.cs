using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject : MonoBehaviour
{
    [SerializeField]
    private List<Renderer> renderers;

    private List<Material> materials;

    [SerializeField]
    private Color highlightColor = new Color(0.5f, 0.5f, 0.5f, 0.25f);

    [SerializeField]
    private Color originalColor;

    // Gets all materials for each renderer on awake
    private void Awake()
    {
        materials = new List<Material>();
        foreach (Renderer renderer in renderers)
        {
            materials.AddRange(new List<Material>(renderer.materials));
        }

        if (renderers.Count > 0)
        {
            originalColor = renderers[0].material.color;
        }
    }

    public void ToggleHighlght(bool value)
    {
        if (value)
        {
            foreach (Material material in materials)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", originalColor * highlightColor);
            }
        }
        else
        {
            foreach (Material material in materials)
            {
                material.DisableKeyword("_EMISSION");
            }
        }
    }

}
