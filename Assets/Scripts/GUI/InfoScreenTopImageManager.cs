using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InfoScreenTopImageManager : MonoBehaviour
{
    /// <summary> Videos associated with the displayed destination </summary>
    private Video[] videos;
    /// <summary> Index of the image currently displayed as the main image </summary>
    private int index = 0;
    /// <summary> City currently displayed </summary>
    public EarthEngineCity city;
    /// <summary> GameObject containing the Image component of the main image </summary>
    public GameObject mainImage;

    /// <summary>
    /// Handles clicks on miniturised image. Displays them as the main image, toggles 360 video overlay.
    /// </summary>
    /// <param name="number">index of the image clicked on</param>
    public void ClickImage(int number)
    {
        mainImage.GetComponent<Image>().sprite = transform.GetChild(number + 1).GetChild(0).gameObject.GetComponent<Image>().sprite;
        if (InfoScreenTopManager.Instance.hotel != null && number < videos.Length)
        {
            mainImage.GetComponent<Button>().enabled = true;
            mainImage.transform.GetChild(0).gameObject.SetActive(true);
        } else
        {
            mainImage.GetComponent<Button>().enabled = false;
            mainImage.transform.GetChild(0).gameObject.SetActive(false);
        }
        index = number;
        /*if(index < videos.Length)
        {
            DataHolderBehaviour.Instance.video = videos[index];
            DataHolderBehaviour.Instance.videoTitle = city.locationName;
        }*/
    }

    /// <summary>
    ///     Loads the video scene
    /// </summary>
    public void VideoScene()
    {
        if(DataHolderBehaviour.Instance.video)
            SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Setup videos and image video overlay
    /// </summary>
    /// <param name="videos">Array of video URLs</param>
    /// <param name="city">City referenced</param>
    public void Setup(Video[] videos, EarthEngineCity city)
    {
        this.videos = new Video[videos.Length];
        Array.Copy(videos, this.videos, videos.Length);
        if (videos.Length > 0)
        {
            mainImage.GetComponent<Button>().enabled = true;
            mainImage.transform.GetChild(0).gameObject.SetActive(true);
        } else
        {
            mainImage.GetComponent<Button>().enabled = false;
            mainImage.transform.GetChild(0).gameObject.SetActive(false);
        }
        this.city = city;
        if (index < videos.Length)
        {
            DataHolderBehaviour.Instance.video = videos[index];
            DataHolderBehaviour.Instance.videoTitle = city.locationName;
        }
    }
}
