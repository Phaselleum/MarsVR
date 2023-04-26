using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InfoScreenTopManager : MonoBehaviour
{
    /// <summary> Main display Image component </summary>
    public Image infoScreenTopMainImage;
    /// <summary> Other display Image components </summary>
    public Image[] infoScreenTopImages;
    /// <summary> Text component for info about the displayed destination/hotel </summary>
    public Text infoScreenTopText;
    /// <summary> Transform of Gameoject containing max temperature text objects </summary>
    public Transform maxTemps;
    /// <summary> Transform of Gameoject containing min temperature text objects </summary>
    public Transform minTemps;
    /// <summary> Transform of Gameoject containing max temperature line objects </summary>
    public Transform maxTempLines;
    /// <summary> Transform of Gameoject containing min temperature line objects </summary>
    public Transform minTempLines;
    /// <summary> Transform of Gameoject containing precipitation text objects </summary>
    public Transform precipTexts;
    /// <summary> Transform of Gameoject containing precipitation line objects </summary>
    public Transform precipLines;
    /// <summary> Map display Image component </summary>
    public Image mapImage;
    /// <summary> City to display info from </summary>
    public EarthEngineCity city;
    /// <summary> Button to start the selected hotel tour </summary>
    public GameObject tourButton;
    public GameObject selectButton;
    public TextMeshProUGUI mapLegend;
    /// <summary> Image video overlay button </summary>
    public GameObject videoOverlay;
    /// <summary> Text component of the display title </summary>
    public TextMeshProUGUI titleText;

    [SerializeField]
    /// <summary> Hotel to display info from </summary>
    public Hotel hotel { get; private set; }

    public static InfoScreenTopManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Sets info text and images, climate graph and video overlay
    /// </summary>
    /// <param name="city">City to set data from</param>
    public void Setup(EarthEngineCity city)
    {
        this.city = city;
        this.hotel = null;

        //save name of label holder
        DataHolderBehaviour.Instance.cityLabelName = city.Label.name;

        //set text
        infoScreenTopText.text = city.infoText;

        //set images
        Sprite[] sprites = city.images;
        for (int i = 0; i < infoScreenTopImages.Length; i++)
        {
            if (i < sprites.Length)
            {
                infoScreenTopImages[i].color = new Color(1, 1, 1, 1);
                infoScreenTopImages[i].sprite = sprites[i];
            }
            else
            {
                infoScreenTopImages[i].color = new Color(1, 1, 1, 0);
                infoScreenTopImages[i].sprite = null;
            }
        }
        if (sprites.Length > 0)
        {
            infoScreenTopMainImage.color = new Color(1, 1, 1, 1);
            infoScreenTopMainImage.sprite = sprites[0];
        }
        else
        {
            infoScreenTopMainImage.color = new Color(1, 1, 1, 0);
            infoScreenTopMainImage.sprite = null;
        }


        if (city.tours.Length > 0)
        {
            tourButton.SetActive(true);
            DataHolderBehaviour.Instance.slideshow = city.tours[0];
        }
        else
        {
            tourButton.SetActive(false);
            DataHolderBehaviour.Instance.slideshow = null;
        }

        //set climat graph
        /*for (int i = 0; i < city.maxTemp.Length; i++)
        {
            Text maxText = maxTemps.GetChild(i).gameObject.GetComponent<Text>();
            Text minText = minTemps.GetChild(i).gameObject.GetComponent<Text>();
            Text precipText = precipTexts.GetChild(i).gameObject.GetComponent<Text>();
            maxText.text = Mathf.Round(city.maxTemp[i]).ToString();
            minText.text = Mathf.Round(city.minTemp[i]).ToString();
            precipText.text = Mathf.Round(city.precipitation[i]).ToString();
            if ((int)FilterBehaviour.Instance.monthFilter == i)
            {
                maxText.color = new Color(1, 1, 1);
                maxText.fontStyle = FontStyle.Bold;
                minText.color = new Color(1, 1, 1);
                minText.fontStyle = FontStyle.Bold;
                precipText.color = new Color(1, 1, 1);
                precipText.fontStyle = FontStyle.Bold;
            }
            else
            {
                maxText.color = new Color(1, 0.3686275f, 0);
                maxText.fontStyle = FontStyle.Normal;
                minText.color = new Color(0.4431373f, 0.7568628f, 0.9294118f);
                minText.fontStyle = FontStyle.Normal;
                precipText.color = new Color(0.22201f, 0.247611f, 0.745f);
                precipText.fontStyle = FontStyle.Normal;
            }
            LineRenderer precipLR = precipLines.GetChild(i).gameObject.GetComponent<LineRenderer>();
            precipLR.SetPosition(1, new Vector3(city.precipitation[i] / 1000, 0, precipLR.GetPosition(1).z));
            if (i < 11)
            {
                LineRenderer maxLR = maxTempLines.GetChild(i).gameObject.GetComponent<LineRenderer>();
                maxLR.SetPosition(0, new Vector3(city.maxTemp[i] / 100, 0, maxLR.GetPosition(0).z));
                maxLR.SetPosition(1, new Vector3(city.maxTemp[i + 1] / 100, 0, maxLR.GetPosition(1).z));
                LineRenderer minLR = minTempLines.GetChild(i).gameObject.GetComponent<LineRenderer>();
                minLR.SetPosition(0, new Vector3(city.minTemp[i] / 100, 0, minLR.GetPosition(0).z));
                minLR.SetPosition(1, new Vector3(city.minTemp[i + 1] / 100, 0, minLR.GetPosition(1).z));
            }
        }*/

        //setup video images
        GameObject.Find("InfoCenter").GetComponent<InfoScreenTopImageManager>().Setup(city.videos, city);
        
        //hide hotel tour button
        //tourButton.SetActive(false);
        selectButton.SetActive(false);
    }

    /// <summary>
    /// Calls show hotel details method given hotel filter display index
    /// </summary>
    /// <param name="i">hotel filter display index</param>
    public void ShowHotelDetails(int i)
    {
        ShowHotelDetails(HotelFilters.Instance.filteredList[HotelFilters.Instance.hotelPage * 6 + i]);
    }
    
    /// <summary>
    /// Display details of hotels in top display
    /// </summary>
    /// <param name="hotel">Hotel to display details from</param>
    public void ShowHotelDetails(Hotel hotel)
    {
        //display texts
        titleText.text = hotel.hotelName;
        if (hotel.description != null && hotel.description.Length > 0)
        {
            infoScreenTopText.text = hotel.description + "...";
        } else
        {
            infoScreenTopText.text = "This is the " + hotel.hotelName + " hotel. Enjoy the lovely view!";
        }

        //display amenities list
        string extras = "\n\n";
        if (hotel.pool) extras += "pool - ";
        if (hotel.restaurant) extras += "restaurant - ";
        if (hotel.familyFriendly) extras += "family friendly - ";
        if (hotel.gym) extras += "gym - ";
        if (hotel.accessible) extras += "accessible - ";
        if (hotel.wifi) extras += "WiFi - ";
        if (hotel.beachProximity) extras += "beach proximity - ";
        if (hotel.petFriendly) extras += "pet friendly - ";
        if (hotel.spa) extras += "spa - ";
        if (hotel.carPark) extras += "car park - ";
        infoScreenTopText.text += extras;

        //display hotel image and video overlay
        for (int i = 0; i < infoScreenTopImages.Length; i++)
        {
            infoScreenTopImages[i].color = new Color(1, 1, 1, 0);
            infoScreenTopImages[i].sprite = null;
        }
        if (hotel.video)
        {
            videoOverlay.SetActive(true);
            DataHolderBehaviour.Instance.video = hotel.video;
            DataHolderBehaviour.Instance.videoTitle = hotel.name;
            infoScreenTopMainImage.gameObject.GetComponent<Button>().enabled = true;
        } else
        {
            videoOverlay.SetActive(false);
            DataHolderBehaviour.Instance.video = null;
            DataHolderBehaviour.Instance.videoTitle = "NOVIDEO";
            infoScreenTopMainImage.gameObject.GetComponent<Button>().enabled = false;
        }
        infoScreenTopMainImage.color = new Color(1, 1, 1, 1);
        infoScreenTopMainImage.sprite = hotel.hotelImage;
        if(hotel.imageURLs.Length > 1)
        {
            for(int i = 1; i < Mathf.Min(4, hotel.imageURLs.Length); i++)
            {
                StartCoroutine(GetHotelImage((UnityWebRequest req, Hotel hotel, Image img) =>
                {
                    if (req.error != null)
                    {
                        Debug.Log($"{req.error}: {req.downloadHandler.text}");

                        img.color = new Color(1, 1, 1, 0);
                        img.sprite = null;
                    }
                    else
                    {
                        Texture2D tex = new Texture2D(0, 0);
                        tex.LoadImage(req.downloadHandler.data);
                        Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
                        img.sprite = spr;
                        img.color = Color.white;
                    }
                }, hotel, infoScreenTopImages[i], i));
            }
            if(hotel.imageURLs.Length > 0)
            {
                infoScreenTopImages[0].sprite = hotel.hotelImage;
                infoScreenTopImages[0].color = Color.white;
            }
        }
        //display hotel tour button
        if (hotel.slideshow)
        {
            tourButton.SetActive(true);
            DataHolderBehaviour.Instance.slideshow = hotel.slideshow;
        } else {
            tourButton.SetActive(false);
            DataHolderBehaviour.Instance.slideshow = null;
        }

        selectButton.SetActive(true);

        this.hotel = hotel;
        Debug.Log(hotel);
        DataHolderBehaviour.Instance.viewedHotel = hotel;
        //load map centered on the selected hotel
        LoadMap();
    }

    /// <summary>
    /// Loads the hotel tour scene
    /// </summary>
    public void StartTour()
    {
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// Starts the map fetch coroutine
    /// </summary>
    public void LoadMap()
    {
        /*if (city = null) city = HotelFilters.Instance.city;
        StartCoroutine(GetMap((UnityWebRequest req) =>
        {
            if (req.error != null)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            }
            else
            {
                Texture tex = DownloadHandlerTexture.GetContent(req);
                mapImage.sprite = Sprite.Create((Texture2D)tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
            }
        }));*/
    }

    /// <summary>
    /// Sends the webrequest to receive the map image
    /// </summary>
    /// <param name="callback">Callback after the request is completed.</param>
    /// <returns></returns>
    private IEnumerator GetMap(Action<UnityWebRequest> callback)
    {
        /*if (city == null) city = HotelFilters.Instance.city;
        string locations = "";

        //add numbered hotel markers
        for (int i = 0; i < Mathf.Min(6, HotelFilters.Instance.filteredList.Count); i++)
        {
            if (i > 0) locations += "||";
            string lat = HotelFilters.Instance.filteredList[i].latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB"));
            if (lat.Length >= 10) lat = lat.Substring(0, 10);
            string lon = HotelFilters.Instance.filteredList[i].longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB"));
            if (lon.Length >= 10) lon = lon.Substring(0, 10);
            locations += lat + "," + lon;
        }

        //add named attraction markers
        mapLegend.text = "";
        for (int i = 0; i < city.attractions.Length; i++)
        {
            if (locations.Length > 0) locations += "||";
            //Debug.Log(city.attractions[i].attractionName + " - " + city.attractions[i].shortName);
            string attrAlph = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            locations += 
                city.attractions[i].latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")).Substring(0, 10) 
                + "," 
                + city.attractions[i].longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")).Substring(0, 10) 
                + "|flag-00078F-" + attrAlph[i];
            mapLegend.text += "<color=#FF8800><b>" + attrAlph[i] + "</b></color>: " + city.attractions[i].attractionName + " ";
        }

        //set latitude/longitude to city/hotel values
        double latitude = city.latitude;
        double longitude = city.longitude;
        if(hotel)
        {
            latitude = hotel.latitude;
            longitude = hotel.longitude;
        }
        hotel = null;

        string url = "https://open.mapquestapi.com/staticmap/v5/map?key=SQ3iJjigQ304FTreA6zqLL7VaXhk5yQP" +
            "&scalebar=false" +
            "&size=1920,1080" +
            "&type=map" +
            "&zoom=12" +
            "&center=" + latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")).Substring(0, 10) + "," 
            + longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")).Substring(0, 10) +
            "&imagetype=JPEG" + 
            "&locations=" + locations +
            "&defaultMarker=marker-num";
        Debug.Log("connecting to: " + url);*/

        /*GameObject.Find("DebugCanvas").GetComponentInChildren<TextMeshProUGUI>().text = "Debug Canvas\n\ncity: " + city.locationName
            + "\n\nlocation: " + city.latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")).Substring(0, 10) + "," 
            + city.longitude.ToString().ToString(CultureInfo.CreateSpecificCulture("en-GB")).Substring(0, 10)
            + "\n\nurl: " + url;*/

        //send the web request and callback
        /*using UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.downloadHandler = new DownloadHandlerTexture();
        yield return webRequest.SendWebRequest();
        callback(webRequest);*/
        return null;
    }

    /// <summary>
    /// Get hotel image from server coroutine function
    /// </summary>
    /// <param name="callback">Callback after receiving server response</param>
    /// <param name="hotel">Hotel the image is loaded for</param>
    /// <param name="img">Image component to load the image into</param>
    /// <returns></returns>
    private IEnumerator GetHotelImage(Action<UnityWebRequest, Hotel, Image> callback, Hotel hotel, Image img, int index)
    {
        /*string url = hotel.imageURLs[index];
        Debug.Log("connecting to: " + url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            callback(webRequest, hotel, img);
        }*/
        return null;
    }
}
