using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Load the video scnee
    /// </summary>
    public void VideoScene()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Load the video scene with a given video
    /// </summary>
    public void VideoScene(Video video)
    {
        DataHolderBehaviour.Instance.video = video;
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Load the video scene with the video from the current Hotel display with the given ID
    /// </summary>
    /// <param name="i">ID of the Hotel currently displayed to play the video from</param>
    public void VideoSceneHotel(int i)
    {
        DataHolderBehaviour.Instance.video = DataHolderBehaviour.Instance.YThotel[i];
        SceneManager.LoadScene(1);
    }

    public void LoadMars()
    {
        DataHolderBehaviour.Instance.lastScene = 0;
        SceneManager.LoadScene(0);
    }

    public void LoadMoon()
    {
        DataHolderBehaviour.Instance.lastScene = 3;
        SceneManager.LoadScene(3);
    }

    public void LoadVenus()
    {
        DataHolderBehaviour.Instance.lastScene = 4;
        SceneManager.LoadScene(4);
    }
}
