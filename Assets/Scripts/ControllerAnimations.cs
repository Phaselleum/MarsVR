using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerAnimations : MonoBehaviour
{
    /// <summary>Right Oculus controller</summary>
    private InputDevice controller;
    public InputDeviceCharacteristics inputDeviceCharacteristics;
    /// <summary> Animates the hands </summary>
    private Animator handAnimator;
    /// <summary> Prefab of Hand Objects </summary>
    public GameObject handModelPrefab;
    /// <summary> Initiated Hand Objects </summary>
    private GameObject spawnedHandModel;

    private void Start()
    {
        //Instantiate Hand Models
        spawnedHandModel = Instantiate(handModelPrefab, transform);
        handAnimator = spawnedHandModel.GetComponent<Animator>();
        TryInitialise();
    }

    /// <summary>
    /// Attempt to initialise controllers
    /// </summary>
    void TryInitialise()
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics, inputDevices);
        if (inputDevices.Count > 0)
        {
            controller = inputDevices[0];
        }
    }

    void Update()
    {
        //Continuously try to initialise missing controllers
        if (!controller.isValid)
        {
            TryInitialise();
        }

        //Animate the hands
        float triggerValue;
        if (controller.TryGetFeatureValue(CommonUsages.trigger, out triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        if (controller.TryGetFeatureValue(CommonUsages.grip, out triggerValue))
        {
            handAnimator.SetFloat("Grip", triggerValue);
        }
    }
}
