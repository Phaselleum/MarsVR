using UnityEngine;
using UnityEngine.XR;

public class VRRenderScale : MonoBehaviour
{
    private void Awake()
    {
        /*OVRPlugin.SystemHeadset headset = OVRPlugin.GetSystemHeadsetType();
        if (headset == OVRPlugin.SystemHeadset.Oculus_Quest)
        {
            QualitySettings.masterTextureLimit = 2;
        }
        else if (headset == (OVRPlugin.SystemHeadset.Oculus_Quest + 1))
        {
            QualitySettings.masterTextureLimit = 0;
        }*/
    }

    void Start()
    {
        //helps with graphical glitches, apparently
        //XRSettings.eyeTextureResolutionScale = 1.5f;
    }
}