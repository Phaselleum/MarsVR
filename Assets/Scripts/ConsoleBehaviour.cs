using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ConsoleBehaviour : MonoBehaviour
{
    private void Awake()
    {
        //handle logs for the option to display them ingame
        Application.logMessageReceived += HandleLog;
    }

    /////////////////////////////////////

    /// <summary> String of the collected logs </summary>
    private string error;

    /// <summary>
    /// Collect logs and, if activated, display on the Debug Screen
    /// </summary>
    /// <param name="logString">Log entry</param>
    /// <param name="stackTrace">Stack trace of log entry</param>
    /// <param name="type">Log entry type</param>
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        error = error + "\n" + logString/* + "\n" + stackTrace*/;
        //GameObject.Find("DebugCanvas").GetComponentInChildren<TextMeshProUGUI>().text = "Debug Canvas\n\n" + error;
    }

    /////////////////////////////////////

    private void LateUpdate()
    {
        //Reset y rotation position in case the Console was moved
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public void EnterHover(HoverEnterEventArgs heea)
    {
        //Send haptic impulse upon entering the grab Collider
        XRBaseControllerInteractor xrbci = (XRBaseControllerInteractor)heea.interactorObject;
        xrbci.SendHapticImpulse(.5f, .2f);
    }
}