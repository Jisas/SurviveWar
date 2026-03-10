using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCount : MonoBehaviour
{
    private TextMeshProUGUI fpsText;

    private void Start()
    {
        fpsText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        fpsText.text = (1 / Time.deltaTime).ToString();
    }
}
