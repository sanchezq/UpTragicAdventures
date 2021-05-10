using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObject : MonoBehaviour
{ 
    private Camera m_camera;
    private MeshRenderer m_mesh;

    [SerializeField]
    private float m_distHidden;
    [SerializeField]
    private float m_speedAlpha = 2.5f;

    private bool m_transparent = false;

    private List<Material> meshMats = new List<Material>();
    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
        m_mesh = GetComponent<MeshRenderer>();
        foreach(Material m in m_mesh.materials)
        {
            meshMats.Add(m);
        }
    }

    public enum BlendMode
    {
        OPAQUE,
        TRANSPARENT
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Material currentMat in meshMats)
        {
            //float distance = Vector3.Distance(m_camera.transform.position, m_mesh.bounds.center);
            float distance = Vector3.Distance(m_camera.transform.position, transform.position);

            float alpha = currentMat.color.a;
            //Debug.Log(distance);
            if (distance <= m_distHidden)
            {
                if (!m_transparent)
                {
                    ChangeRenderMode(currentMat, BlendMode.TRANSPARENT);
                    m_transparent = true;
                }

                if (alpha >= 0)
                {
                    currentMat.color = new Color(currentMat.color.r, currentMat.color.g, currentMat.color.b, (alpha - m_speedAlpha * Time.deltaTime));
                }
                else
                {
                    currentMat.color = new Color(currentMat.color.r, currentMat.color.g, currentMat.color.b, 0f);
                    m_mesh.enabled = false;
                }
            }
            else
            {
                if (m_transparent)
                {
                    if (!m_mesh.enabled)
                    {

                        m_mesh.enabled = true;
                    }
                    if (alpha < 1)
                    {
                        currentMat.color = new Color(currentMat.color.r, currentMat.color.g, currentMat.color.b, (alpha + m_speedAlpha * Time.deltaTime));
                    }
                    else
                    {
                        ChangeRenderMode(currentMat, BlendMode.OPAQUE);
                        m_transparent = false;
                        currentMat.color = new Color(currentMat.color.r, currentMat.color.g, currentMat.color.b, 1);
                    }
                }

            }
        }
    }


    public static void ChangeRenderMode(Material standardShaderMaterial, BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.OPAQUE:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = -1;
                break;
            case BlendMode.TRANSPARENT:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
        }

    }
}
