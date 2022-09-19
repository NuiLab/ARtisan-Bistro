using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PersistentGOManager : MonoBehaviour
{
    #region Consts to modify
    private const int FlushAfter = 40;
    #endregion

    [SerializeField] bool showNotification = false;
    [SerializeField] GameObject StudyBillboard;

    Vector3 position = new Vector3(1000, 1000, 1000);
    GameObject currGlobalRecordsGO;
    IMixedRealitySceneSystem sceneSystem;
    string unloadSceneName;
    bool notificationSound = false;
    bool sceneChanged = false;

    int participantNumber = 0;
    string filePath;
    StreamWriter writer;
    float time_s = 0;
    List<string> independentCSVData = new List<string>();
    private StringBuilder csvData;

    List<string> instructions = new List<string>();
    int instructionsNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        sceneSystem = MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>();
        filePath = Application.persistentDataPath + "/Records";
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);
        InitializeInstructions();
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        time_s += Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha0))
        {
            showNotification = false;
            notificationSound = false;
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
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.I))
        {
            ShowInstructions(instructionsNum++);
        }
    }

    public void SetSceneNamesAndLoad(string newSceneName)
    {
        sceneChanged = true;
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
        sceneChanged = false;
        foreach (var independentData in independentCSVData)
        {
            csvData.AppendLine(participantNumber + "," + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + "," + time_s + "," + currGlobalRecordsGO.GetComponent<Records>().GetNotificationType() + "," + notificationSound + "," + independentData);
        }
        independentCSVData.Clear();
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
        filePath = filePath + "/Participant" + participantNumber.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + ".csv";
        using (writer = File.CreateText(filePath))
        {
            writer.WriteLine("Participant_Number,Timestamp,Time_s,Notification_Type,Notification_Sound,Category,Action,Status,Ingredients");
        }
        csvData = new StringBuilder();
    }

    public int GetParticipantNumber()
    {
        return participantNumber;
    }

    public void AddData(string category="n/a", string action="n/a", int status=0, string ingredients="n/a")
    {
        /*
         * status (0=n/a; 1=start; 2=end)
         */
        if (sceneChanged)
            independentCSVData.Add(category + "," + action + "," + status + "," + ingredients);
        else
            csvData.AppendLine(participantNumber + "," + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + "," + time_s + "," + currGlobalRecordsGO.GetComponent<Records>().GetNotificationType() + "," + notificationSound + "," + category + "," + action + "," + status + "," + ingredients);
        if (csvData.Length >= FlushAfter)
        {
            FlushData();
        }
    }

    void FlushData()
    {
        using (var csvWriter = new StreamWriter(filePath, true))
        {
            csvWriter.Write(csvData.ToString());
        }
        csvData.Clear();
    }

    public void EndCSV()
    {
        if (csvData == null)
        {
            return;
        }
        using (var csvWriter = new StreamWriter(filePath, true))
        {
            csvWriter.Write(csvData.ToString());
        }
        csvData = null;
    }

    private void OnDestroy()
    {
        EndCSV();
    }

    void InitializeInstructions()
    {
        instructions.Add("Welcome to study titled \"Notifications in Pervasive Augmented Reality Scenario\"");
        instructions.Add("Testing 1");
        instructions.Add("Testing 2");
        instructions.Add("Testing 3");
        instructions.Add("Testing 4");
    }

    void ShowInstructions(int instructionNum)
    {
        if (!StudyBillboard.activeSelf)
        {
            StudyBillboard.SetActive(true);
        }
        StudyBillboard.GetComponentInChildren<TextMeshProUGUI>().text = instructions[instructionNum];
    }
}
