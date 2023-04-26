using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR;

public class VideoManager : MonoBehaviour
{
    /// <summary> Primary video player </summary>
    public VideoPlayer videoplayer;
    /// <summary> Audio video player </summary>
    public VideoPlayer audioVP;
    /// <summary> Weezy Logo video player </summary>
    public VideoPlayer weezyPlayer;

    /// <summary> IP of yt-dlp server </summary>
    public string serverIP;
    /// <summary> Url of video to stream </summary>
    public string vidServerAddress;

    /// <summary> Loading text Canvas GameObject </summary>
    public GameObject loadCanvas;
    /// <summary> Video controls Canvas GameObject </summary>
    public GameObject controlCanvas;
    /// <summary> Weezy Logo player panel GameObject </summary>
    public GameObject weezyPlayerPanel;
    /// <summary> Url to the YouTube video </summary>
    public Text ytUrl;
    /// <summary> Video timeline Slider </summary>
    public Slider timeline;
    /// <summary> Current time elapsed and total time displaying Text component </summary>
    public Text timeState;
    /// <summary> Video title </summary>
    public Text titleText;

    /// <summary> Material for 360 videos </summary>
    public Material Mat360;
    /// <summary> Material for 180 videos </summary>
    public Material Mat180;
    /// <summary> Plane for standard videos </summary>
    public GameObject standardVidPlane;

    /// <summary> fade timer for Loading Canvas </summary>
    private float fadeLoadingTimer = 0;
    /// <summary> fade timer for Controls Canvas </summary>
    private float fadeControlsTimer = 0;

    /// <summary>Right Oculus controller</summary>
    private InputDevice rightController;
    /// <summary>Left Oculus controller</summary>
    private InputDevice leftController;
    /// <summary>Left Oculus trigger value</summary>
    private bool lastTriggerL;
    /// <summary>Right Oculus trigger value</summary>
    private bool lastTriggerR;

    /// <summary>Has the main video playback started?</summary>
    private bool playBackStarted;
    /// <summary>Is the Weezy logo video playing?</summary>
    private bool weezyIsPlaying = true;
    /// <summary>Has the main video playback buffered?</summary>
    private bool playbackIsBuffered = false;

    ///<summary>keeps track of seconds (0,1]</summary>
    private float timer;
    ///<summary>time the video player was at last second</summary>
    private double oldVPTime;

    /// <summary> Is the audio separate from the video? </summary>
    private bool separateAudio = false;

    public static VideoManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //Setup data, textures and displays
        videoplayer = GetComponent<VideoPlayer>();
        ytUrl.text = DataHolderBehaviour.Instance.video.url;
        titleText.text = DataHolderBehaviour.Instance.videoTitle;
        TryInitialise();
        fadeControlsTimer = 5;
        RenderTexture rt = (RenderTexture)Mat360.mainTexture;
        rt.Release();

        //Decide which display/material to stream to
        switch (DataHolderBehaviour.Instance.video.format) {
            case Video.Format.full_360:
                RenderSettings.skybox = Mat360;
                break;
            case Video.Format.half_180:
                RenderSettings.skybox = Mat180;
                break;
            case Video.Format.standard:
            default:
                RenderSettings.skybox = null;
                standardVidPlane.SetActive(true);
                break;
        }
        LoadResources();
    }

    private void Update()
    {
        //End playback at the end of the video
        if(weezyIsPlaying && weezyPlayer.length - weezyPlayer.time < .5)
        {
            weezyIsPlaying = false;
            weezyPlayerPanel.SetActive(false);
        }

        //Start playback when main video is buffered and Weezy Logo playback has ended
        if (!weezyIsPlaying && playbackIsBuffered)
        {
            Play();
            playbackIsBuffered = false;
        }

        //Fade out controls when video starts
        if(!playBackStarted && videoplayer.time > 0)
        {
            fadeLoadingTimer = 1;
            playBackStarted = true;
        }

        //fading canvases
        if (fadeLoadingTimer > 0)
        {
            loadCanvas.GetComponent<CanvasGroup>().alpha = Mathf.Min(fadeControlsTimer, 1);
            fadeLoadingTimer = Mathf.Max(fadeLoadingTimer - Time.deltaTime, 0);
            if (fadeLoadingTimer == 0)
            {
                loadCanvas.SetActive(false);
            }
        }
        if (fadeControlsTimer > 0)
        {
            controlCanvas.GetComponent<CanvasGroup>().alpha = Mathf.Min(fadeControlsTimer, 1);
            fadeControlsTimer = Mathf.Max(fadeControlsTimer - Time.deltaTime, 0);
            if (fadeControlsTimer == 0)
            {
                controlCanvas.SetActive(false);
            }
        }

        //controller input
        if (!weezyIsPlaying)
        {
            if (videoplayer.isPlaying && videoplayer.length - videoplayer.time < .5)
            {
                Exit();
            }

            bool triggerButtonValue = false;
            if (rightController != null)
            {
                rightController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonValue);
            }
            if (triggerButtonValue && (triggerButtonValue ^ lastTriggerR))
            {
                controlCanvas.SetActive(true);
                fadeControlsTimer = 5;
            }
            lastTriggerR = triggerButtonValue;
            triggerButtonValue = false;
            if (leftController != null)
            {
                leftController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonValue);
            }
            if (triggerButtonValue && (triggerButtonValue ^ lastTriggerL))
            {
                controlCanvas.SetActive(true);
                fadeControlsTimer = 5;
            }
            lastTriggerL = triggerButtonValue;
        }

        //timeline stuff
        if(videoplayer.isPlaying)
            timeline.value = (float)(Mathf.Max((float)videoplayer.time, float.Epsilon) / videoplayer.length);

        timer += Time.deltaTime;
        if(separateAudio && timer > 4)
        {
            timer = timer % 4;
            if(videoplayer.time == oldVPTime)
            {
                audioVP.Pause();
            } else
            {
                if(Math.Abs(videoplayer.time - oldVPTime) > .2f)
                {
                    audioVP.time = videoplayer.time;
                }
                if (!audioVP.isPlaying)
                {
                    audioVP.Play();
                }
            }
            oldVPTime = videoplayer.time;
        }

        //Display animated text on Loading Canvas
        if (loadCanvas.activeSelf)
        {
            int time2 = (int)timer % 4;
            if (time2 == 0)
            {
                loadCanvas.GetComponentInChildren<Text>().text = "Loading";
            }
            else if (time2 == 1)
            {
                loadCanvas.GetComponentInChildren<Text>().text = "Loading.";
            }
            else if (time2 == 2)
            {
                loadCanvas.GetComponentInChildren<Text>().text = "Loading..";
            }
            else
            {
                loadCanvas.GetComponentInChildren<Text>().text = "Loading...";
            }
        }

        //Exit the player after the video ends
        if (videoplayer.length != 0 && videoplayer.time == videoplayer.length)
        {
            Exit();
        }
    }

    /// <summary>
    /// Play the main video
    /// </summary>
    public void Play()
    {
        videoplayer.Play();
        if (separateAudio) audioVP.Play();
    }

    /// <summary>
    /// Pause the main video
    /// </summary>
    public void Pause()
    {
        videoplayer.Pause();
        if (separateAudio) audioVP.Pause();
    }

    /// <summary>
    /// Return to the main scene
    /// </summary>
    public void Exit()
    {
        SceneManager.LoadScene(DataHolderBehaviour.Instance.lastScene);
    }

    /// <summary>
    /// Scrub to point in main video player timeline
    /// </summary>
    public void SearchTimeline()
    {
        if (videoplayer.isPaused)
        {
            videoplayer.time = videoplayer.length * timeline.value;
            if (separateAudio) audioVP.time = videoplayer.length * timeline.value;
        }
        timeState.text = (int)(videoplayer.time / 60) + ":" + ((int)(videoplayer.time % 60)).ToString("D2") + "/"
            + (int)(videoplayer.length / 60) + ":" + ((int)(videoplayer.length % 60)).ToString("D2");
    }

    /// <summary>
    /// Handle the video/audio URL requests
    /// </summary>
    private void LoadResources()
    {
        if (DataHolderBehaviour.Instance.video.localVideo)
        {
            videoplayer.source = VideoSource.VideoClip;
            videoplayer.clip = DataHolderBehaviour.Instance.video.localVideo;
            videoplayer.Prepare();
            playbackIsBuffered = true;
        }
        else if(DataHolderBehaviour.Instance.video.proprietary)
        {
            videoplayer.source = VideoSource.Url;
            videoplayer.url = "http://" + vidServerAddress + DataHolderBehaviour.Instance.video.proprietaryFileName;
            videoplayer.Prepare();
        } else
        {
            separateAudio = true;
            StartCoroutine(GetAudioURL((UnityWebRequest req) =>
            {
                if (req.error != null)
                {
                    Debug.Log($"{req.error}: {req.downloadHandler.text}");
                }
                else
                {
                    LoadAudioURL(req.downloadHandler.text);
                }
            }));
            StartCoroutine(GetVideoURL((UnityWebRequest req) =>
            {
                if (req.error != null)
                {
                    Debug.Log($"{req.error}: {req.downloadHandler.text}");
                }
                else
                {
                    LoadURL(req.downloadHandler.text);
                }
            }));
        }
    }

    /// <summary>
    /// Request the audio video URL
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator GetAudioURL(Action<UnityWebRequest> callback)
    {
        string url = "http://" + serverIP.Trim() + "/get-audio/" 
            + UnityWebRequest.EscapeURL(DataHolderBehaviour.Instance.video.url.Split('=')[1]);
        Debug.Log("connecting to: " + url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            callback(webRequest);
        }
    }

    /// <summary>
    /// Request the video video URL
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator GetVideoURL(Action<UnityWebRequest> callback)
    {
        string url = "http://" + serverIP.Trim() + "/get-video/" + DataHolderBehaviour.Instance.videoQuality 
            + "/" + UnityWebRequest.EscapeURL(DataHolderBehaviour.Instance.video.url.Split('=')[1]);
        Debug.Log("connecting to: " + url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            callback(webRequest);
        }
    }

    /// <summary>
    /// Load a URL into the video video player
    /// </summary>
    /// <param name="vURL"></param>
    public void LoadURL(string vURL)
    {

        videoplayer.source = VideoSource.Url;
        videoplayer.url = vURL;
        videoplayer.Prepare();
        videoplayer.prepareCompleted += PrepareComplete;
        
    }

    /// <summary>
    /// Load a URL into the audio video player
    /// </summary>
    /// <param name="vURL"></param>
    public void LoadAudioURL(string vURL)
    {
        audioVP.source = VideoSource.Url;
        audioVP.url = vURL;
        audioVP.Prepare();
        audioVP.prepareCompleted += PrepareAudioComplete;
    }

    /// <summary>
    /// Play the video as soon as it's loaded
    /// </summary>
    /// <param name="source"></param>
    private void PrepareComplete(VideoPlayer source)
    {
        Debug.Log("Playback is buffered");
        playbackIsBuffered = true;
    }

    /// <summary>
    /// Debug for audio video preparedness
    /// </summary>
    /// <param name="source"></param>
    private void PrepareAudioComplete(VideoPlayer source)
    {
        Debug.Log("Audio Playback is buffered");
    }

    /// <summary>
    ///     Initialise VR headset controller setup
    /// </summary>
    private void TryInitialise()
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, inputDevices);
        if (inputDevices.Count > 0)
        {
            rightController = inputDevices[0];
        }
        inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, inputDevices);
        if (inputDevices.Count > 0)
        {
            leftController = inputDevices[0];
        }
    }
}
