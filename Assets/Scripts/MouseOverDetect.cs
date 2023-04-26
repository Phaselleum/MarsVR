using UnityEngine;

/// <summary>
/// Detects if the mouse is hovering over the main earth object
/// </summary>
public class MouseOverDetect : MonoBehaviour
{
    /// <summary>The only instance of this script</summary>
    public static MouseOverDetect Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    /// <summary>true when the mouse is hovering over the earth object. </summary>
    public bool mouseOver = false;

    void LateUpdate()
    {
        //status is reset after regular operations are done
        mouseOver = false;
    }

    void OnMouseOver()
    {
        mouseOver = true;
    }
}
