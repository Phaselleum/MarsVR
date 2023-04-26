using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class MailManager : MonoBehaviour
{

    public void SendMail(string contents)
    {
        StartCoroutine(Upload(contents));
    }

    IEnumerator Upload(string contents)
    {
        WWWForm form = new WWWForm();
        form.AddField("data", contents);

        using (UnityWebRequest www = UnityWebRequest.Post("http://81.169.211.124:8088/send-email/" + PlayerPrefs.GetString("email"), form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Email requested for: " + PlayerPrefs.GetString("email"));
            }
        }
    }
}