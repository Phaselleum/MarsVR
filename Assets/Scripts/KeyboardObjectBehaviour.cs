using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class KeyboardObjectBehaviour : MonoBehaviour
{
    public InputField inputField;

    public static KeyboardObjectBehaviour Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Shift()
    {
        foreach(Transform tf in transform)
        {
            if(tf.childCount > 1)
            {
                foreach(Transform tfc in tf)
                {
                    tfc.gameObject.SetActive(!tfc.gameObject.activeSelf);
                    if(tfc.gameObject.activeSelf)
                    {
                        tf.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                        tf.gameObject.GetComponent<Button>().onClick.AddListener(new UnityAction(() => EnterString(tfc.gameObject.GetComponent<TextMeshProUGUI>().text)));
                        //Debug.Log(tfc.gameObject.GetComponent<TextMeshProUGUI>().text);
                    }
                }
            }
        }
    }

    public void EnterString(string str)
    {
        if (inputField)
            inputField.text += str;
        Debug.Log(str);
    }

    public void DeleteChar()
    {
        if (inputField)
            try { inputField.text = inputField.text.Substring(0, inputField.text.Length - 1); } catch (ArgumentOutOfRangeException e) { return; }
    }
}
