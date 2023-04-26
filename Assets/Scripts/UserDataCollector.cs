using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserDataCollector : MonoBehaviour
{
    /// <summary> Time the session started </summary>
    private DateTime sessionStart;
    /// <summary> List of watched videos </summary>
    public List<Video> videoList;
    /// <summary> List of times spent watching videos </summary>
    public List<double> videoTimesList;
    /// <summary> List of names of destinations visited </summary>
    public List<string> destinationList;
    /// <summary> Number of filter uses </summary>
    public int filterUseCount;
    /// <summary> Number of filter searches </summary>
    public int filterSearchCount;

    /// <summary> Unique session hash </summary>
    public string sessionHash;

    /// <summary> timer until next Data Collection </summary>
    private float timer;

    public static UserDataCollector Instance { get; private set; }

    void Awake()
    {
        //setup user and session hashes
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            sessionStart = System.DateTime.Now;
			if(!PlayerPrefs.HasKey("userID")) {
				Hash128 hash = new Hash128();
                hash.Append(UnityEngine.Random.Range(0, 2147483647).ToString());
				PlayerPrefs.SetString("userID", hash.ToString());
			}
			Hash128 hash2 = new Hash128();
			hash2.Append(UnityEngine.Random.Range(0, 2147483647).ToString());
			sessionHash = hash2.ToString();
        } else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        videoList = new List<Video>();
        videoTimesList = new List<double>();
        destinationList = new List<string>();
    }

    private void Update()
    {
        //send Data every 30s
        timer += Time.deltaTime;
        if(timer >= 30)
        {
            StartCoroutine(SendData());
            timer %= 30;
        }
    }

    /// <summary>
    /// Format and send user data to server
    /// </summary>
    /// <returns></returns>
    IEnumerator SendData()
    {
        Debug.Log("Starting user data send coroutine");
        string sendData = "\"userID\": \"" + PlayerPrefs.GetString("userID") + "\",\n"
            + "\"sessionID\": \"" + sessionHash + "\",\n"
            + "\"sessionStart\": \"" + sessionStart + "\",\n"
            + "\"timeSent\": \"" + System.DateTime.Now + "\",\n"
            + "\"videoCount\": \"" + videoList.Count + "\",\n"
            + "\"videoList\": [\"" + string.Join("\",\"", videoList.ConvertAll(x => x.name.ToString()).ToArray()) + "\"],\n"
            + "\"videoWatchTimes\": [\"" + string.Join("\",\"", videoTimesList.ToArray()) + "\"],\n"
            + "\"destinationCount\": \"" + destinationList.Count + "\",\n"
            + "\"destinationList\": [\"" + string.Join("\",\"", destinationList.ToArray()) + "\"],\n"
            + "\"filterUseCount\": \"" + filterUseCount + "\",\n"
            + "\"filterSearchCount\": \"" + filterSearchCount + "\"";

        WWWForm form = new WWWForm();
        form.AddField("data", sendData);
		
        using UnityWebRequest www = UnityWebRequest.Post("http://" + DataHolderBehaviour.Instance.serverIP + "/user-data/" + PlayerPrefs.GetString("userID") + "/" + sessionHash, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("User data sent for " + PlayerPrefs.GetString("userID"));
        }
    }
}
