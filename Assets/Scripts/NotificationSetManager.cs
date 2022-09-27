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
     * 2: Notification on Viewport
     */
    [SerializeField] GameObject[] notificationPrefabs;

    GameObject persistentGO;
    GameObject globalRecordsGO;
    GameObject tempNotification;
    List<string> notifications = new List<string>();
    List<string> stations = new List<string>();
    List<int> gameObjectId = new List<int>();
    bool waitTimeStarted = false;


    // Start is called before the first frame update
    void Start()
    {
        globalRecordsGO = GameObject.FindWithTag("Global Records");
        persistentGO = GameObject.FindGameObjectsWithTag("PersistentGO")[0];
        bool nSound = persistentGO.GetComponent<PersistentGOManager>().GetNotificationSound();
        foreach (var notifications in globalRecordsGO.GetComponent<Records>().GetAllNotificationPrefabs())
            notifications.GetComponent<AudioSource>().mute = !nSound;
        if (notificationType == 1)
            notificationDock.GetComponent<AudioSource>().mute = !nSound;
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


    public GameObject AddNotificationOnObject(int objectId)
    {
        gameObjectId.Add(objectId);
        return Instantiate(notificationPrefabs[notificationType]);
    }

    public void AddNotificationOnDock(string stationTxt, string notificationTxt, int objectId)
    {
        notificationDock.GetComponent<NotificationDockManager>().AddNotification(stationTxt, notificationTxt, objectId);
    }

    public void AddNotificationOnViewport(string stationTxt, string notificationTxt, int objectId)
    {
        if (gameObjectId.Contains(objectId))
        {
            int tempIndex = gameObjectId.IndexOf(objectId);
            gameObjectId.Remove(objectId);
            notifications.RemoveAt(tempIndex);
            stations.RemoveAt(tempIndex);
        }
        gameObjectId.Add(objectId);
        notifications.Add(notificationTxt);
        stations.Add(stationTxt);
    }

    IEnumerator CreateNotification(float duration)
    {
        yield return new WaitForSeconds(duration);
        tempNotification = Instantiate(notificationPrefabs[notificationType]);
        tempNotification.transform.parent = notificationBillboard.transform;
        tempNotification.transform.localPosition = new Vector3(0, 0.1f, 0);
        tempNotification.transform.localRotation = Quaternion.identity;
        tempNotification.transform.Find("IconAndText").Find("Station Text").GetComponent<TextMeshPro>().text = stations[0];
        tempNotification.transform.Find("IconAndText").Find("Notification Text").GetComponent<TextMeshPro>().text = notifications[0];
        notifications.RemoveAt(0);
        stations.RemoveAt(0);
        waitTimeStarted = false;
    }
}
