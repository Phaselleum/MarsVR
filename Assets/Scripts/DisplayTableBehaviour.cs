using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTableBehaviour : MonoBehaviour
{
    /// <summary> Array of Collectibles available on the Display Table </summary>
    public GameObject[] collectibles;

    /// <summary> Array of IDs of Collectibles available on the Display Table </summary>
    private string[] collectibleIDs;

    /// <summary> Button to display the next Collectible </summary>
    public Button next;
    /// <summary> Button to display the previous Collectible </summary>
    public Button previous;

    /// <summary> List of collected Collectibles </summary>
    private List<GameObject> collectiblesDisplayed;

    /// <summary> index of currently displayed Collectible </summary>
    private int index = 0;

    private void Start()
    {
        //populate list of collected Collectibles
        collectibleIDs = new string[collectibles.Length];
        collectiblesDisplayed = new List<GameObject>();
        for (int i = 0; i < collectibles.Length; i++)
        {
            collectibleIDs[i] = collectibles[i].GetComponent<CollectibleBehaviour>().id;
            if(PlayerPrefs.HasKey("Collectible" + collectibleIDs[i]))
            {
                if(PlayerPrefs.GetInt("Collectible" + collectibleIDs[i]) > 0)
                {
                    collectiblesDisplayed.Add(collectibles[i]);
                }
            }
        }
        //Activate display and buttons according to count of collected Collectibles
        if (collectiblesDisplayed.Count > 0)
        {
            collectiblesDisplayed[0].SetActive(true);
            previous.interactable = collectiblesDisplayed.Count > 1;
            next.interactable = collectiblesDisplayed.Count > 1;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.C))
        {
            for (int i = 0; i < collectibles.Length; i++)
            {
                collectibleIDs[i] = collectibles[i].GetComponent<CollectibleBehaviour>().id;
                PlayerPrefs.SetInt("Collectible" + collectibleIDs[i], 1);
            }
            Start();
        }
    }

    /// <summary>
    /// Display the next collected Collectible on the table
    /// </summary>
    public void Next()
    {
        collectiblesDisplayed[index].SetActive(false);
        index = (index + 1) % collectiblesDisplayed.Count;
        collectiblesDisplayed[index].SetActive(true);
        collectiblesDisplayed[index].GetComponent<CollectibleBehaviour>().ResetTransform();
    }

    /// <summary>
    /// Display the previous collected Collectible on the table
    /// </summary>
    public void Previous()
    {
        collectiblesDisplayed[index].SetActive(false);
        index = (index - 1) % collectiblesDisplayed.Count;
        if (index < 0) index = collectiblesDisplayed.Count - 1;
        collectiblesDisplayed[index].SetActive(true);
        collectiblesDisplayed[index].GetComponent<CollectibleBehaviour>().ResetTransform();
    }
}
