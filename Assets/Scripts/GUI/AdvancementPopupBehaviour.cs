using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvancementPopupBehaviour : MonoBehaviour
{
    public static AdvancementPopupBehaviour Instance { get; private set; }

    public Text description;
    public Image image;
    public GameObject banner;

    private float timer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = 0;
                banner.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Display the advancement banner for 5 seconds
    /// </summary>
    /// <param name="description">Description to display on the banner</param>
    /// <param name="sprite">Sprite to display with the banner description</param>
    public void ShowAdvancementBanner(string description, Sprite sprite)
    {
        this.description.text = description;
        image.sprite = sprite;
        banner.SetActive(true);
        timer = 5;
        //Debug.Log("display!");
    }
}
