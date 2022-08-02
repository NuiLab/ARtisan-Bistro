using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationDockManager : MonoBehaviour
{
    [SerializeField] GameObject notificationBtnText;
    [SerializeField] GameObject notificationCountGO;
    [SerializeField] GameObject notificationButton;
    [SerializeField] GameObject notificationParent;

    List<GameObject> notificationsList = new List<GameObject>();
    List<string> notificationText = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
  

    public void ShowNotifications()
    {
        if (notificationBtnText.GetComponent<TextMeshPro>().text == "Hide Notifications")
        {
            notificationBtnText.GetComponent<TextMeshPro>().text = "Show Notifications";
            for (int i = 0; i < notificationsList.Count; i++)
            {
                Destroy(notificationsList[i]);
            }
            notificationsList.Clear();
        }
        else
        {
            notificationBtnText.GetComponent<TextMeshPro>().text = "Hide Notifications";
            for (int i = 0; i < notificationText.Count; i++)
            {
                notificationsList.Add(Instantiate(notificationButton, new Vector3(0, 0, 0), Quaternion.identity));
                float y = -1 * (float)i / 10;
                notificationsList[i].GetComponent<NotificationManager>().SetNotificationProperties(notificationText[i], notificationParent, new Vector3(0, y, 0), Quaternion.identity, new Vector3(3, 3, 1));
            }
        }
    }

    public void AddNotification(string text)
    {
        if (notificationBtnText.GetComponent<TextMeshPro>().text == "Hide Notifications")
        {
            notificationsList.Add(Instantiate(notificationButton, new Vector3(0, 0, 0), Quaternion.identity));
            float y = -1 * (float)notificationText.Count / 10;
            notificationsList[notificationText.Count].GetComponent<NotificationManager>().SetNotificationProperties(text, notificationParent, new Vector3(0, y, 0), Quaternion.identity, new Vector3(3, 3, 1));
        }
        notificationCountGO.GetComponentInChildren<TextMeshPro>().text = (int.Parse(notificationCountGO.GetComponentInChildren<TextMeshPro>().text) + 1).ToString();
        notificationText.Add(text);
    }
    public void ManageNotificationLayout(GameObject notificationGO)
    {
        int index = notificationsList.IndexOf(notificationGO);
        if (index != -1)
        {
            GameObject GOtoDestroy = notificationsList[index];
            notificationText.RemoveAt(index);
            notificationsList.RemoveAt(index);
            Destroy(GOtoDestroy);
            if (notificationBtnText.GetComponent<TextMeshPro>().text == "Hide Notifications")
            {
                for (int i = 0; i < notificationText.Count; i++)
                {
                    float y = -1 * (float)i / 10;
                    notificationsList[i].GetComponent<NotificationManager>().SetNotificationProperties(notificationText[i], notificationParent, new Vector3(0, y, 0), Quaternion.identity, new Vector3(3, 3, 1));
                }
            }
            notificationCountGO.GetComponentInChildren<TextMeshPro>().text = (int.Parse(notificationCountGO.GetComponentInChildren<TextMeshPro>().text) - 1).ToString();
        }
    }

    public int GetNotificationCountGO()
    {
        return int.Parse(notificationCountGO.GetComponentInChildren<TextMeshPro>().text);
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
    }
}
