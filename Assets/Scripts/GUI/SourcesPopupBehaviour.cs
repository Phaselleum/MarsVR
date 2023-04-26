using UnityEngine;

public class SourcesPopupBehaviour : MonoBehaviour
{
    /// <summary> Popup Canvas object </summary>
    public GameObject popupPlane;

    /// <summary>
    /// Display the Popup Canvas Object
    /// </summary>
    public void OpenPopup()
    {
        popupPlane.SetActive(true);
    }

    /// <summary>
    /// Hide the Popup Canvas Object
    /// </summary>
    public void ClosePopUp()
    {
        popupPlane.SetActive(false);
    }
}
