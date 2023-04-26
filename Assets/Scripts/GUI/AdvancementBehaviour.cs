using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdvancementBehaviour : MonoBehaviour
{
    public static AdvancementBehaviour Instance { get; private set; }

    public Button explore1;
    public Button explore2;
    public Button explore3;
    public Button collect1;
    public Button collect2;
    public Button collect3;
    public Button duration1;
    public Button duration2;
    public Button duration3;

    public Sprite exploreSprite;
    public Sprite collectSprite;
    public Sprite durateSprite;

    private List<string> exploredDestinations;
    private List<string> collectedObjects;
    private float appDuration;

    private float timer;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        //check for existing advancements
        exploredDestinations = new List<string>();
        if (!PlayerPrefs.HasKey("AdvExplore"))
        {
            PlayerPrefs.SetString("AdvExplore", "");
        } else
        {
            exploredDestinations.AddRange(PlayerPrefs.GetString("AdvExplore").Split(','));
            exploredDestinations.Remove("");
        }

        collectedObjects = new List<string>();
        if (!PlayerPrefs.HasKey("AdvCollect"))
        {
            PlayerPrefs.SetString("AdvCollect", "");
        } else
        {
            collectedObjects.AddRange(PlayerPrefs.GetString("AdvCollect").Split(','));
            collectedObjects.Remove("");
        }

        if (!PlayerPrefs.HasKey("AdvDuration"))
        {
            PlayerPrefs.SetInt("AdvDuration", 0);
        } else
        {
            appDuration = PlayerPrefs.GetInt("AdvDuration");
        }

        int advExplore = exploredDestinations.Count;
        if (advExplore >= 3) explore1.interactable = true;
        if (advExplore >= 15) explore2.interactable = true;
        if (advExplore >= 50) explore3.interactable = true;
        int advCollect = collectedObjects.Count;
        if (advCollect >= 1) collect1.interactable = true;
        if (advCollect >= 5) collect2.interactable = true;
        if (advCollect >= 20) collect3.interactable = true;
        int advDuration = (int)appDuration;
        if (advDuration >= 300) duration1.interactable = true;
        if (advDuration >= 1800) duration2.interactable = true;
        if (advDuration >= 7200) duration3.interactable = true;
    }

    private void Update()
    {
        //periodically check for time-sensitive advancements
        if(Time.deltaTime < 1)
        {
            timer += Time.deltaTime;
            appDuration += Time.deltaTime;
            if(timer > 5)
            {
                timer = 0;
                if (!duration1.interactable && appDuration >= 300)
                {
                    duration1.interactable = true;
                    AdvancementPopupBehaviour.Instance.ShowAdvancementBanner("Test Driver\nUsed the app for 5 minutes", durateSprite);
                }
                if (!duration2.interactable && appDuration >= 1800)
                { 
                    duration2.interactable = true;
                    AdvancementPopupBehaviour.Instance.ShowAdvancementBanner("VR Native\nUsed the app for 30 minutes", durateSprite);
                }
                if (!duration3.interactable && appDuration >= 7200) 
                { 
                    duration3.interactable = true;
                    AdvancementPopupBehaviour.Instance.ShowAdvancementBanner("Metanaut\nUsed the app for 2 hours", durateSprite);
                }
            }
        }
    }

    /// <summary>
    /// Add visited destination to total visited destinations
    /// </summary>
    /// <param name="name">Location name of the visited destination</param>
    public void DestinationVisited(string name)
    {
        if(exploredDestinations.Contains(name))
        {
            exploredDestinations.Add(name);
            PlayerPrefs.SetString("AdvExplore", string.Join(",", exploredDestinations.ToArray()));
            if(!explore1.interactable && exploredDestinations.Count >= 3)
            {
                explore1.interactable = true;
                AdvancementPopupBehaviour.Instance.ShowAdvancementBanner("Novice Explorer\nVisited 3 Destinations", exploreSprite);
            }
            if(!explore2.interactable && exploredDestinations.Count >= 15)
            {
                explore2.interactable = true;
                AdvancementPopupBehaviour.Instance.ShowAdvancementBanner("Seasoned Traveller\nVisited 15 Destinations", exploreSprite);
            }
            if(!explore3.interactable && exploredDestinations.Count >= 50)
            {
                explore3.interactable = true;
                AdvancementPopupBehaviour.Instance.ShowAdvancementBanner("Globetrotter\nVisited 50 Destinations", exploreSprite);
            }
        }
    }

    /// <summary>
    /// Add found collectible to list of found collectibles
    /// </summary>
    /// <param name="name">name of the collectible found</param>
    public void CollectibleFound(string name)
    {
        if(!collectedObjects.Contains(name))
        {
            collectedObjects.Add(name);
            PlayerPrefs.SetString("AdvCollect", string.Join(",", collectedObjects.ToArray()));
        }
        if (!collect1.interactable && collectedObjects.Count >= 1)
        {
            collect1.interactable = true;
            explore3.interactable = true;
            //if (AdvancementPopupBehaviour.Instance == null) Debug.Log("AdvancementPopupBehaviour not instantiated");
            AdvancementPopupBehaviour.Instance.ShowAdvancementBanner("Lucky Charm\nFound a collectible", collectSprite);
            //Debug.Log("collect!");
        }
        if (!collect2.interactable && collectedObjects.Count >= 5)
        {
            collect2.interactable = true;
            explore3.interactable = true;
            AdvancementPopupBehaviour.Instance.ShowAdvancementBanner("Souvenir Collector\nFound 5 collectibles", collectSprite);
        }
        if (!collect3.interactable && collectedObjects.Count >= 20)
        {
            collect3.interactable = true;
            explore3.interactable = true;
            AdvancementPopupBehaviour.Instance.ShowAdvancementBanner("Sherlock Holmes\nFound all 20 collectibles", collectSprite);
        }
    }

}
