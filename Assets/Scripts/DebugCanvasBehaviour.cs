using UnityEngine;
using TMPro;

public class DebugCanvasBehaviour : MonoBehaviour
{
    /// <summary> Displayed text on Debug Canvas </summary>
    public TextMeshProUGUI text;

    /// <summary> Move displayed text up </summary>
    public void Up()
    {
        text.rectTransform.localPosition = text.rectTransform.localPosition - new Vector3(100, 0, 0);
    }

    /// <summary> Move displayed text down </summary>
    public void Down()
    {
        text.rectTransform.localPosition = text.rectTransform.localPosition + new Vector3(100, 0, 0);
    }
}
