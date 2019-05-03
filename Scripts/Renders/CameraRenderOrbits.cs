using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRenderOrbits : MonoBehaviour {

    public SolarSystemManager SolarSystemManager;

    private void OnPostRender()
    {
        // SolarSystemManager.RenderCachedOrbitsGL();
    }
}
