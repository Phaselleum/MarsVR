using UnityEngine;

public class RadioBehaviour : MonoBehaviour
{
    /// <summary> Radio Canvas GameObject </summary>
    public GameObject canvasObject;
    /// <summary> Currently playing audio </summary>
    private AudioSource currentAudio;
    /// <summary> Available audio choices </summary>
    public AudioClip[] audioChoice;

    public static RadioBehaviour Instance;

    private void Awake()
    {
        currentAudio = gameObject.GetComponent<AudioSource>();
        Instance = this;
        //resume playing after scene change
        if(DataHolderBehaviour.Instance.radioChannel >= 0)
        {
            currentAudio.clip = audioChoice[DataHolderBehaviour.Instance.radioChannel];
            currentAudio.time = DataHolderBehaviour.Instance.radioTime;
            currentAudio.Play();
        }
    }

    private void Update()
    {
        if (currentAudio.isPlaying)
        {
            DataHolderBehaviour.Instance.radioTime = currentAudio.time;
        }
    }

    /// <summary>
    /// Display the radio Canvas
    /// </summary>
    public void Select()
    {
        canvasObject.SetActive(true);
    }

    /// <summary>
    /// Hide the radio Canvas
    /// </summary>
    public void DeSelect()
    {
        canvasObject.SetActive(false);
    }

    /// <summary>
    /// Play the audio track with the given ID
    /// </summary>
    /// <param name="id">ID of the audio track to play</param>
    public void PlayMusic(int id)
    {
        currentAudio.Stop();
        currentAudio.clip = audioChoice[id];
        currentAudio.Play();
        DataHolderBehaviour.Instance.radioChannel = id;
    }

    /// <summary>
    /// Pause the music playback
    /// </summary>
    public void PauseMusic()
    {
        currentAudio.Pause();
        DataHolderBehaviour.Instance.radioChannel = -1;
    }
}
