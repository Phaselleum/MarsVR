using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CollectibleInteractions : XRGrabInteractable
{
    /// <summary> ID of the Collectible </summary>
    public string id;

    [System.Obsolete]
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        //Remove Object on collection
        CollectibleVideoBehaviour.Instance.Grab(id);
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnMouseOver()
    {
        //Remove Object on collection
        if (Input.GetMouseButtonDown(0)) {
            CollectibleVideoBehaviour.Instance.Grab(id);
            Destroy(gameObject);
        }
    }
}
