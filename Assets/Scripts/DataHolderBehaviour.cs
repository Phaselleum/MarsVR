using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DataHolderBehaviour : MonoBehaviour
{
    public static DataHolderBehaviour Instance;

    /// <summary>The video to be played on entering the video scene</summary>
    public Video video = null;
    /// <summary>Title of the video to be played on entering the video scene</summary>
    public string videoTitle = "";
    /// <summary>Array of videos attached to the current selection of hotels</summary>
    public Video[] YThotel;
    /// <summary>Array of video titles attached to the current selection of hotels</summary>
    public string[] hotelVideoTitles;
    /// <summary>The slideshow to be played on entering the video scene</summary>
    public Slideshow slideshow;

    public int lastScene = 0;

    public Hotel viewedHotel;
    public Hotel selectedHotel;

    /// <summary>Current playback time of the radio music</summary>
    public float radioTime;
    /// <summary>Selected radio track id</summary>
    public int radioChannel;
    /// <summary>Ensures the controls canvas is shown only on startup</summary>
    public bool showControlsCanvas = true;

    /// <summary>IP of the data server</summary>
    public string serverIP;

    /// <summary>Split csv of flight prices</summary>
    public float[][] flightPrices;
    /// <summary>Array of departure airport IATA codes</summary>
    public string[] departureAirports;
    /// <summary>Array of destination airport IATA codes</summary>
    public string[] destinationAirports;
    /// <summary>Backup csv with flight prices</summary>
    public TextAsset flightsCSV;
    /// <summary>True once the data has been fetched from server or loaded from local file</summary>
    public bool flightPricesLoaded = false;

    /// <summary> Default video quality  (height in pixels) </summary>
    public string videoQuality = "1440";
    /// <summary> Name of the last viewed destination; used when changing scenes </summary>
    public string cityLabelName;

    public List<Hotel> allHotels;
    public List<string> destinationssWithHotelsLoaded;
    public List<string> hotelsLoadingFrom;
    public bool hotelFavouritesLoaded;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }

        //max 6 hotels per page/selection
        YThotel = new Video[6];
        hotelVideoTitles = new string[6];
        allHotels = new List<Hotel>();
        destinationssWithHotelsLoaded = new List<string>();
        hotelsLoadingFrom = new List<string>();

        //get flight data from server as csv; deal with request failure
        /*StartCoroutine(GetFlightData((UnityWebRequest req) =>
        {
            if (req.error != null)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");

                /*GameObject messagePopup = null;
                Transform[] trs = GameObject.Find("Main Camera").GetComponentsInChildren<Transform>(true);
                foreach (Transform t in trs)
                {
                    if (t.name == "MessagePopupCanvas")
                    {
                        messagePopup = t.gameObject;
                    }
                }
                messagePopup.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Error retrieving flight data";
                messagePopup.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Could not connect to server\n(" + req.error + ")";
                messagePopup.SetActive(true);*/

                /*LoadFlightData(flightsCSV.text);
            }
            else
            {
                LoadFlightData(req.downloadHandler.text);
            }
        }));*/
    }

    /// <summary>
    /// Server request for flight prices csv
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator GetFlightData(Action<UnityWebRequest> callback)
    {
        string url = "http://" + serverIP.Trim() + "/get-flight-prices/";
        Debug.Log("connecting to: " + url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            callback(webRequest);
        }
    }

    private float PrivParseFloat(string s)
    {
        float f = 0;
        float.TryParse(s, out f);
        return f;
    }

    /// <summary>
    /// Fill related arrays with given flight data.
    /// </summary>
    /// <param name="data">csv with airport IATAs in first line and column.</param>
    /// <returns></returns>
    private bool LoadFlightData(string data)
    {
        //Debug.Log(data);
        /*try
        {
            string[] flightPricesRows = data.Split("\n"[0]);
            flightPrices = new float[flightPricesRows.Length][];
            destinationAirports = new string[flightPricesRows.Length - 2];
            departureAirports = flightPricesRows[0].Substring(1).Split(','); //skip first comma
            for (int i = 1; i < flightPricesRows.Length - 1; i++)
            {
                string[] flightPricesRow = flightPricesRows[i].Split(',');
                destinationAirports[i - 1] = flightPricesRow[0];
                string[] flightPricesData = new string[flightPricesRow.Length - 1];
                Array.Copy(flightPricesRow, 1, flightPricesData, 0, flightPricesRow.Length - 1);
                flightPrices[i - 1] = Array.ConvertAll(flightPricesData, PrivParseFloat);
                //if(i == 115) Debug.Log(i + " - " + flightPricesRows[i]);
            }
            //Debug.Log(flightPrices[85][Array.IndexOf(DataHolderBehaviour.Instance.departureAirports, "HKT")]);
        } catch (Exception e)
        {
            //load from file if data is invalid
            if(e is IndexOutOfRangeException || e is FormatException)
            {
                Debug.Log("Error loading flight data: " + e.Message);

                /*GameObject messagePopup = null;
                Transform[] trs = GameObject.Find("Main Camera").GetComponentsInChildren<Transform>(true);
                foreach (Transform t in trs)
                {
                    if (t.name == "MessagePopupCanvas")
                    {
                        messagePopup = t.gameObject;
                    }
                }
                messagePopup.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Error reading flight data";
                messagePopup.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Fallback data may be outdated or inaccurate.\n(" + e.Message + ")";
                messagePopup.SetActive(true);*/

                //return true;
                /*return LoadFlightData(flightsCSV.text);
            }

            throw;
        } 
        Debug.Log("Flight data loaded!");
        //return false;*/
        return flightPricesLoaded = true;
    }
}
