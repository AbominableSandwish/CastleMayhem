using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ParticuleSetter : MonoBehaviour
{
    [SerializeField] private Material _material;

    // Start is called before the first frame update
    void Start()
    {
        if(_material != null)
            SetSprite(_material);
    }

    public void SetSprite(Material material)
    {
        ParticleSystemRenderer psr = GetComponent<ParticleSystemRenderer>();
        psr.material = material;
    }
}
