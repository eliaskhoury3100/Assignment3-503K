using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotWindMill : MonoBehaviour
{
    private Transform rotAxis;
    [SerializeField] private Vector3 rot = new Vector3(1, 0, 1) * 20;
    private int angle = 180;

    private MeshRenderer meshRenderer;

    void Awake()
    {
        // Get the MeshRenderer component
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        rotAxis = GameObject.FindWithTag("RotAxisMill").transform;

        // Forcefully enable the MeshRenderer at runtime to fix the rendering issue
        if (meshRenderer != null)
        {
            //meshRenderer.enabled = false;
            meshRenderer.enabled = true;  // Force re-enable to avoid the lock
        }
    }

    void Update()
    {
        transform.RotateAround(rotAxis.position, rot, angle*Time.deltaTime);
    }
}
