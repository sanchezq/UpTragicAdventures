using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCloud : MonoBehaviour
{
    [SerializeField]
    private float m_speedMov;
    private float m_maxOffset;
    private Renderer m_renderer;
    private float m_offset = 0f;
    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponent<Renderer>();
        m_maxOffset = m_renderer.material.GetTextureScale("_MainTex").x;
    }
    //test
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(m_maxOffset);
        m_renderer.material.SetTextureOffset("_MainTex", new Vector2(m_offset, 0));
        m_offset += Time.deltaTime * m_speedMov;
        if(m_offset >= m_maxOffset)
        {
            m_offset = 0f;
        }
    }
}
