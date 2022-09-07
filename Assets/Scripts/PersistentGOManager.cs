using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class PersistentGOManager : MonoBehaviour
{
    [SerializeField] bool showNotification = false;

    Vector3 position = new Vector3(1000, 1000, 1000);
    GameObject currGlobalRecordsGO;
    IMixedRealitySceneSystem sceneSystem;
    string unloadSceneName;
    bool notificationSound = true;

    int participantNumber = 0;
    string filePath;
    StreamWriter writer;
    float time_s = 0;

    // Start is called before the first frame update
    void Start()
    {
        sceneSystem = MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>();
        filePath = Application.persistentDataPath + "/Records";
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        time_s += Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetSceneNamesAndLoad("Instructions Scene");
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            showNotification = true;
            notificationSound = true;
            SetSceneNamesAndLoad("NoD_WS Scene");

        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            showNotification = true;
            notificationSound = false;
            SetSceneNamesAndLoad("NoD_WOS Scene");
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            showNotification = true;
            notificationSound = true;
            SetSceneNamesAndLoad("NoO_WS Scene");
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha4))
        {
            showNotification = true;
            notificationSound = false;
            SetSceneNamesAndLoad("NoO_WOS Scene");
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha5))
        {
            showNotification = true;
            notificationSound = true;
            SetSceneNamesAndLoad("NoV_WS Scene");
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha6))
        {
            showNotification = true;
            notificationSound = false;
            SetSceneNamesAndLoad("NoV_WOS Scene");
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha7))
        {
            showNotification = false;
            notificationSound = false;
            SetSceneNamesAndLoad("Control Scene");
        }


        if (Input.GetKeyDown(KeyCode.M))
        {
            SetParticipantNumber(0);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            writer.Close();
        }
    }

    void SetSceneNamesAndLoad(string newSceneName)
    {
        if (sceneSystem.IsContentLoaded("Instructions Scene"))
            unloadSceneName = "Instructions Scene";
        else if (sceneSystem.IsContentLoaded("NoD_WS Scene"))
            unloadSceneName = "NoD_WS Scene";
        else if (sceneSystem.IsContentLoaded("NoD_WOS Scene"))
            unloadSceneName = "NoD_WOS Scene";
        else if (sceneSystem.IsContentLoaded("NoO_WS Scene"))
            unloadSceneName = "NoO_WS Scene";
        else if (sceneSystem.IsContentLoaded("NoO_WOS Scene"))
            unloadSceneName = "NoO_WOS Scene";
        else if (sceneSystem.IsContentLoaded("NoV_WS Scene"))
            unloadSceneName = "NoV_WS Scene";
        else if (sceneSystem.IsContentLoaded("NoV_WOS Scene"))
            unloadSceneName = "NoV_WOS Scene";
        else if (sceneSystem.IsContentLoaded("Control Scene"))
            unloadSceneName = "Control Scene";

        var task = LoadNextLevel(newSceneName);
    }

    public async Task LoadNextLevel(string sceneName)
    {
        await sceneSystem.UnloadContent(unloadSceneName);
        await sceneSystem.LoadContent(sceneName);
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public void SetPosition(Vector3 pos)
    {
        position = pos;
    }

    public bool GetShowNotification()
    {
        return showNotification;
    }

    public void SetShowNotification(bool sNotifi)
    {
        showNotification = sNotifi;
    }

    public void SetCurrGlobalRecordsGO(GameObject currObject)
    {
        currGlobalRecordsGO = currObject;
    }

    public bool GetNotificationSound()
    {
        return notificationSound;
    }

    public void SetNotificationSound(bool nSound)
    {
        notificationSound = nSound;
    }

    //============================== Study ==============================//

    public void SetParticipantNumber(int pNum)
    {
        participantNumber = pNum;
        filePath = filePath + "/Participant" + participantNumber.ToString()+ ".csv";
        using (writer = File.CreateText(filePath))
        {
            writer.WriteLine("Participant_Number, Notification_Type, Notification_Sound, Category, Action, Status, Time_s");
        }
    }

    public int GetParticipantNumber()
    {
        return participantNumber;
    }

    public void AddData(string category="n/a", string action="n/a", int status=0)
    {
        /*
         * status (0=n/a; 1=start; 2=end)
         */
        using (writer = File.AppendText(filePath))
        {
            writer.WriteLine(participantNumber + "," + currGlobalRecordsGO.GetComponent<Records>().GetNotificationType() + "," + notificationSound + "," + category + "," + action + "," + status + "," + time_s);
        }
    }
}
