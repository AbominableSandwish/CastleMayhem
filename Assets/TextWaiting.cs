using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextWaiting : MonoBehaviour
{
    const float TimeToRefresh = 0.5f;
    float CounterTime = 0.0f;
    TextMeshProUGUI m_text;
    // Start is called before the first frame update
    void Start()
    {
        m_text = GetComponent<TextMeshProUGUI>();
        m_text.text = "";
        CounterTime = TimeToRefresh;
    }

    // Update is called once per frame
    void Update()
    {
        CounterTime -= Time.deltaTime;
        if(CounterTime <= 0.0f)
        {
            m_text.text += '.';
            if (m_text.text.Length >= 4)
                m_text.text = "";
            CounterTime = TimeToRefresh;
        }
    }
}
