using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationSetManager : MonoBehaviour
{
    [SerializeField] GameObject notificationDock;
    [SerializeField] GameObject notificationBillboard;
    [SerializeField] int notificationType;
    /*
     * 0: Notification on Object
     * 1: Notification on Dock
     * 2: Notification on Head
     */
    [SerializeField] GameObject[] notificationPrefabs;

    GameObject tempNotification;
    List<string> notifications = new List<string>();
    bool waitTimeStarted = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tempNotification == null && notifications.Count > 0 && !waitTimeStarted)
        {
            waitTimeStarted = true;
            StartCoroutine(CreateNotification(0.5f));
        }
    }

    public int GetNotificationType()
    {
        return notificationType;
    }

    public void SetNotificationType(int notifiType)
    {
        notificationType = notifiType;
    }

    public GameObject GetNotificationPrefab()
    {
        return notificationPrefabs[notificationType];
    }

    public void AddNotificationOnDock(string text)
    {
        notificationDock.GetComponent<NotificationDockManager>().AddNotification(text);
    }

    public void AddNotificationOnViewport(string text)
    {
        if (text == "test")
        {
            notifications.Add(notifications.Count.ToString());
        }
        else
        {
            notifications.Add(text);
        }
    }

    IEnumerator CreateNotification(float duration)
    {
        yield return new WaitForSeconds(duration);
        tempNotification = Instantiate(notificationPrefabs[2]);
        tempNotification.transform.parent = notificationBillboard.transform;
        tempNotification.transform.localPosition = new Vector3(0, 0.15f, 0);
        tempNotification.transform.localRotation = Quaternion.identity;
        tempNotification.transform.GetComponentInChildren<TextMeshPro>().text = notifications[0];
        notifications.RemoveAt(0);
        waitTimeStarted = false;
    }
}
