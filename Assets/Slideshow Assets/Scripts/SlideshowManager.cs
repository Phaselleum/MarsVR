using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SlideshowManager : MonoBehaviour
{
    /// <summary> Tour displayed </summary>
    public Slideshow slideshow;
    /// <summary> Scene skybox </summary>
    public Material skybox;
    /// <summary> Title of displayed tour </summary>
    public Text title;
    /// <summary> Buttons of available categories </summary>
    public Button[] categoryButtons;
    /// <summary> Gameobject of menu Canvas </summary>
    public GameObject menuCanvasObject;

    /// <summary> Array of List of avilable categories </summary>
    private List<int>[] categories;

    /// <summary> Currently displayed 360 Image </summary>
    private int currentImage;
    /// <summary> Currently selected category </summary>
    private int currentCategory;

    private void Start()
    {
        //Load selected tour
        slideshow = DataHolderBehaviour.Instance.slideshow;
        categories = new List<int>[slideshow.categoryNames.Length];

        //Setup categories menu
        for(int i = 0; i < categories.Length; i++)
        {
            categories[i] = new List<int>(Array.ConvertAll(slideshow.categoryImageIds[i].Split(','), int.Parse));
        }
        for (int i = 0; i < categoryButtons.Length; i++)
        {
            if(i < categories.Length)
            {
                categoryButtons[i].gameObject.SetActive(true);
                categoryButtons[i].GetComponentInChildren<Text>().text = slideshow.categoryNames[i];
            } else
            {
                categoryButtons[i].gameObject.SetActive(false);
            }
        }
        currentImage = 0;
        currentCategory = 0;
        SetTexture();
        title.text = slideshow.slideshowName;
    }

    /// <summary>
    /// Switch to next image in tour
    /// </summary>
    public void Next()
    {
        currentImage = (currentImage + 1) % categories[currentCategory].Count;
        SetTexture();
    }

    /// <summary>
    /// Switch to previous image in tour
    /// </summary>
    public void Previous()
    {
        currentImage = (categories[currentCategory].Count + currentImage - 1) % categories[currentCategory].Count;
        SetTexture();
    }

    /// <summary>
    /// Return to globe scene
    /// </summary>
    public void Exit()
    {
        SceneManager.LoadScene(DataHolderBehaviour.Instance.lastScene);
    }

    /// <summary>
    /// Switch to given category
    /// </summary>
    /// <param name="id">ID of category to switch to</param>
    public void SetCategory(int id)
    {
        currentCategory = id;
        currentImage = 0;
        SetTexture();
    }

    /// <summary>
    /// Set the currnet image as the skybox texture
    /// </summary>
    private void SetTexture()
    {
        skybox.mainTexture = slideshow.images[categories[currentCategory][currentImage]];
    }

    /// <summary>
    /// Toggle the display of the menu
    /// </summary>
    public void ToggleMenu()
    {
        menuCanvasObject.SetActive(!menuCanvasObject.activeSelf);
    }
}
