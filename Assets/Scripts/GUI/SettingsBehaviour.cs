using UnityEngine;
using UnityEngine.UI;

public class SettingsBehaviour : MonoBehaviour
{
    /// <summary> Text component diplaying the selected video quality </summary>
    public Text VideoQualityValue;

    private void Start()
    {
        //Set default video quality to 1440p
        string quality = PlayerPrefs.GetString("videoQuality");
        if(quality == "")
        {
            quality = "1440";
        }
        VideoQualityValue.text = "(" + quality + "p)";
        DataHolderBehaviour.Instance.videoQuality = quality;
    }

    /// <summary>
    /// Open the settings screen on the main screen on click
    /// </summary>
    public void ClickHandler()
    {
        MainScreenBehaviour.Instance.ChangeDisplay(MainScreenBehaviour.Displays.SETTINGS);
    }

    /// <summary>
    /// Set the video quality to a given value
    /// </summary>
    /// <param name="quality"></param>
    public void SelectButton(string quality)
    {
        DataHolderBehaviour.Instance.videoQuality = quality;
        VideoQualityValue.text = "(" + quality + "p)";
        PlayerPrefs.SetString("videoQuality", quality);
    }
}
