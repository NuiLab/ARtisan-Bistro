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
    [SerializeField] GameObject notificationBtnBackplate;
    [SerializeField] Material[] notificationBtnMaterial;
    [SerializeField] float duration = -1;

    List<GameObject> notificationsList = new List<GameObject>();
    List<string> notificationText = new List<string>();
    List<string> stationText = new List<string>();
    List<int> gameObjectId = new List<int>();
    List<int> notificationNumbers = new List<int>();
    List<float> notificationDurations = new List<float>();
    AudioSource audioSource;
    float timeTracker = 0;

    // Start is called before the first frame update
    void Start()
    {
        ShowNotifications();
    }

    private void Update()
    {
        timeTracker += Time.deltaTime;
        if (duration > 0)
            for (int i = 0; i < notificationDurations.Count; i++)
                if (timeTracker > notificationDurations[i] + duration)
                    RemoveNotification(i);
    }

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
            notificationBtnBackplate.GetComponent<Renderer>().material = notificationBtnMaterial[0];
            for (int i = 0; i < notificationText.Count; i++)
            {
                notificationsList.Add(Instantiate(notificationButton, new Vector3(0, 0, 0), Quaternion.identity));
                float y = -1 * (float)i / 10;
                notificationsList[i].GetComponent<NotificationManager>().SetNotificationProperties(notificationNumbers[i], stationText[i], notificationText[i], notificationParent, new Vector3(0, y, 0), Quaternion.identity, new Vector3(3, 3, 1));
            }
        }
    }

    public void AddNotification(int notifiNum, string stationTxt, string notificationTxt, int objectId)
    {
        if (gameObjectId.Contains(objectId))
        {
            if (notificationBtnText.GetComponent<TextMeshPro>().text == "Hide Notifications")
            {
                ManageNotificationLayout(notificationsList[gameObjectId.IndexOf(objectId)]);
            }
            else
            {
                int index = gameObjectId.IndexOf(objectId);
                gameObjectId.RemoveAt(index);
                notificationText.RemoveAt(index);
                stationText.RemoveAt(index);
                notificationNumbers.RemoveAt(index);
                notificationDurations.RemoveAt(index);
            }
        }
        
        notificationCountGO.GetComponentInChildren<TextMeshPro>().text = (int.Parse(notificationCountGO.GetComponentInChildren<TextMeshPro>().text) + 1).ToString();
        notificationText.Insert(0, notificationTxt);
        stationText.Insert(0, stationTxt);
        notificationNumbers.Insert(0, notifiNum);
        gameObjectId.Insert(0, objectId);
        notificationDurations.Insert(0, timeTracker);
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        ShowNotifications();
        ShowNotifications();

        if (notificationBtnText.GetComponent<TextMeshPro>().text == "Hide Notifications")
        {
            //notificationsList.Add(Instantiate(notificationButton, new Vector3(0, 0, 0), Quaternion.identity));
            //float y = -1 * (float)notificationText.Count / 10;
            //notificationsList[notificationText.Count].GetComponent<NotificationManager>().SetNotificationProperties(stationTxt, notificationTxt, notificationParent, new Vector3(0, y, 0), Quaternion.identity, new Vector3(3, 3, 1));
        }
        else
        {
            notificationBtnBackplate.GetComponent<Renderer>().material = notificationBtnMaterial[1];
        }
    }

    public void RemoveNotification(GameObject cutletGO)
    {
        if (gameObjectId.Contains(cutletGO.GetInstanceID()))
        {
            if (notificationBtnText.GetComponent<TextMeshPro>().text == "Hide Notifications")
            {
                ManageNotificationLayout(notificationsList[gameObjectId.IndexOf(cutletGO.GetInstanceID())]);
            }
            else
            {
                int index = gameObjectId.IndexOf(cutletGO.GetInstanceID());
                gameObjectId.RemoveAt(index);
                notificationText.RemoveAt(index);
                stationText.RemoveAt(index);
                notificationNumbers.RemoveAt(index);
                notificationDurations.RemoveAt(index);
            }
        }
    }

    public void RemoveNotification(int index)
    {
        if (notificationBtnText.GetComponent<TextMeshPro>().text == "Hide Notifications")
        {
            ManageNotificationLayout(notificationsList[index]);
        }
        else
        {
            gameObjectId.RemoveAt(index);
            notificationText.RemoveAt(index);
            stationText.RemoveAt(index);
            notificationNumbers.RemoveAt(index);
            notificationDurations.RemoveAt(index);
        }
        notificationCountGO.GetComponent<TextMeshPro>().text = gameObjectId.Count.ToString();
    }

    public void ManageNotificationLayout(GameObject notificationGO)
    {
        int index = notificationsList.IndexOf(notificationGO);
        if (index != -1)
        {
            GameObject GOtoDestroy = notificationsList[index];
            gameObjectId.RemoveAt(index);
            notificationText.RemoveAt(index);
            stationText.RemoveAt(index);
            notificationNumbers.RemoveAt(index);
            notificationsList.RemoveAt(index);
            notificationDurations.RemoveAt(index);
            Destroy(GOtoDestroy);
            if (notificationBtnText.GetComponent<TextMeshPro>().text == "Hide Notifications")
            {
                for (int i = 0; i < notificationText.Count; i++)
                {
                    float y = -1 * (float)i / 10;
                    notificationsList[i].GetComponent<NotificationManager>().SetNotificationProperties(notificationNumbers[i], stationText[i], notificationText[i], notificationParent, new Vector3(0, y, 0), Quaternion.identity, new Vector3(3, 3, 1));
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
        PersistentGOManager.instance.AddData("Dock", "Released [" + transform.position.x.ToString() + ";" + transform.position.y.ToString() + ";" + transform.position.z.ToString() + "]", 2);
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
    }

    public void DockGrabbed()
    {
        PersistentGOManager.instance.AddData("Dock", "Grabbed [" + transform.position.x.ToString() + ";" + transform.position.y.ToString() + ";" + transform.position.z.ToString() + "]", 1);
    }
}
