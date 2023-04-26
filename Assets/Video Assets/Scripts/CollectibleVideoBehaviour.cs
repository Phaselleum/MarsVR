using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CollectibleVideoBehaviour : MonoBehaviour
{
    /// <summary> List of Collectibles in the video </summary>
    public List<CollectibleObject> collectibleObjects;
    /// <summary> List of GameObjects associated with Collectibles in the video </summary>
    List<GameObject> gameObjects;

    private int counter = 0;

    public static CollectibleVideoBehaviour Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        collectibleObjects = new List<CollectibleObject>();
    }

    void Start()
    {
        //Setup Collectibles
        gameObjects = new List<GameObject>();
        collectibleObjects.AddRange(DataHolderBehaviour.Instance.video.collectibleObjects);
        List<CollectibleObject> removeObjs = new List<CollectibleObject>();
        foreach(CollectibleObject co in collectibleObjects)
        {
            if(PlayerPrefs.HasKey("Collectible" + co.objectId) && PlayerPrefs.GetInt("Collectible" + co.objectId) == 1)
            {
                removeObjs.Add(co);
                continue;
            }
            gameObjects.Add(Instantiate(co.gameObject, transform));
            gameObjects[gameObjects.Count - 1].SetActive(false);
        }
        foreach(CollectibleObject co in removeObjs)
        {
            collectibleObjects.Remove(co);
        }
    }

    void Update()
    {
        counter++;

        if(counter % 16 == 0)
        {
            //Display Collectibles at given times
            for (int i = 0; i < collectibleObjects.Count; i++)
            {
                if (gameObjects[i])
                {
                    if (!gameObjects[i].activeSelf
                        && VideoManager.Instance.videoplayer.time > collectibleObjects[i].videoDisplayStart
                        && VideoManager.Instance.videoplayer.time < collectibleObjects[i].videoDisplayEnd)
                    {
                        gameObjects[i].SetActive(true);
                    }
                    else if (gameObjects[i].activeSelf && VideoManager.Instance.videoplayer.time > collectibleObjects[i].videoDisplayEnd)
                    {
                        gameObjects[i].SetActive(false);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Add to advancements and hide on grab
    /// </summary>
    /// <param name="id">ID of Collectible grabbed</param>
    public void Grab(string id)
    {
        CollectibleObject co = null;
        for (int i = 0; i < collectibleObjects.Count; i++)
        {
            if(collectibleObjects[i].objectId == id)
            {
                gameObjects[i].SetActive(false);
                PlayerPrefs.SetInt("Collectible" + id, 1);
                AdvancementBehaviour.Instance.CollectibleFound(collectibleObjects[i].objectId);
                co = collectibleObjects[i];
            }
        }
        if (co != null) collectibleObjects.Remove(co);
    }
}
