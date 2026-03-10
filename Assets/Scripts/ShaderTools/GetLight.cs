using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GetLight : MonoBehaviour
{
    [SerializeField] private Material skyBoxMaterial;

    private void Update()
    {
        skyBoxMaterial.SetVector(name = "_CustomLightDirection", transform.forward);
    }
}
