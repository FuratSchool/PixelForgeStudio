using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private CinemachineFreeLook FreelookCam;
    private CinemachineFreeLook.Orbit[] originalOrbits;
    
    [Range(0.01f,0.5f)]
    [SerializeField] private float minZoom = 0.5f;
    
    [Range(1.0f,5.0f)]
    [SerializeField] private float maxZoom = 2.0f;
    
    [AxisStateProperty]
    public AxisState zAxis = new AxisState(0f, 1f, false, true, 50f, 0.1f, 0.11f, "Mouse ScrollWheel", false);
    void Start()
    {
        FreelookCam = GetComponent<CinemachineFreeLook>();
        if (FreelookCam != null)
        {
            originalOrbits = new CinemachineFreeLook.Orbit[FreelookCam.m_Orbits.Length];
            for (int i =0;i<originalOrbits.Length;i++)
            {
                originalOrbits[i].m_Height = FreelookCam.m_Orbits[i].m_Height;
                originalOrbits[i].m_Radius = FreelookCam.m_Orbits[i].m_Radius;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (originalOrbits != null)
        {
            zAxis.Update(Time.deltaTime);
            float zoomScale = Mathf.Lerp(minZoom, maxZoom, zAxis.Value);
            for (int i = 0; i < originalOrbits.Length; i++)
            {
                FreelookCam.m_Orbits[i].m_Height = originalOrbits[i].m_Height * zoomScale;
                FreelookCam.m_Orbits[i].m_Radius = originalOrbits[i].m_Radius * zoomScale;
            }
        }
    }
}
