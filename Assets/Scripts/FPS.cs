using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FPS : MonoBehaviour
{

    public Text m_fpsText = null;

    private int m_updateRate = 4;  // 4 updates per sec.
    private int m_frameCount = 0;
    private float m_deltaTime = 0f;
    private float m_fps = 0f;

    // Update is called once per frame
    void Update()
    {
        m_deltaTime += Time.unscaledDeltaTime;

        m_frameCount++;

        // Only update texts 'm_updateRate' times per second

        if (m_deltaTime > 1f / m_updateRate)
        {
            m_fps = m_frameCount / m_deltaTime;

            // Update fps and ms

            m_fpsText.text = Mathf.RoundToInt(m_fps).ToString();
        }
        m_deltaTime = 0f;
        m_frameCount = 0;
    }
}
