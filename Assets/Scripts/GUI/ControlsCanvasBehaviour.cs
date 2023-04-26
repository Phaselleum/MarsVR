using UnityEngine;

public class ControlsCanvasBehaviour : MonoBehaviour
{
    void Start()
    {
        //Controls are only shown on initial startup
        if (DataHolderBehaviour.Instance.showControlsCanvas)
        {
            DataHolderBehaviour.Instance.showControlsCanvas = false;
        } else
        {
            gameObject.SetActive(false);
        }
    }
}
