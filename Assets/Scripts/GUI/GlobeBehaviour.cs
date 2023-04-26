using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GlobeBehaviour : MonoBehaviour
{
    public GameObject mainEarth;
    Vector3 startPosition;
    bool isControlled;
    XRBaseController xrbc;
    
    void Update()
    {
        if (isControlled)
        {
            mainEarth.transform.eulerAngles = new Vector3(0, -transform.parent.localEulerAngles.y, 0);
            mainEarth.transform.parent.rotation = Quaternion.Euler(-90, 0, 0) * transform.parent.parent.localRotation;
        } else
        {
            transform.parent.localRotation = Quaternion.Euler(0, 0, -mainEarth.transform.eulerAngles.y);
            transform.parent.parent.localRotation = 
                mainEarth.transform.parent.rotation * Quaternion.Euler(90, 0, 0);
        }
    }

    public void EnterHover(HoverEnterEventArgs heea)
    {
        XRBaseControllerInteractor xrbci = (XRBaseControllerInteractor)heea.interactorObject;
        xrbci.SendHapticImpulse(.5f, .2f);
    }

    public void EnterSelect(SelectEnterEventArgs seea)
    {
        XRBaseControllerInteractor xrbci = (XRBaseControllerInteractor)seea.interactorObject;
        xrbc = xrbci.xrController;
        startPosition = xrbci.xrController.transform.position;
        isControlled = true;
    }

    public void ExitSelect(SelectExitEventArgs seea)
    {
        isControlled = false;
    }
}
