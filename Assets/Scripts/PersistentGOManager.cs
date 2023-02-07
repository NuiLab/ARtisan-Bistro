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
using System.Linq;

public class PersistentGOManager : MonoBehaviour
{
    public static PersistentGOManager instance;

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
    List<string> sceneNames;
    int sceneIndex = 0;

    int participantNumber = 0;
    string filePath;
    StreamWriter writer;
    float time_s = 0;
    List<string> independentCSVData = new List<string>();
    private StringBuilder csvData;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneNames = new List<string>() { "EyeDock", "EyeHand", "TouchHand", "TouchDock", "VoiceHand", "VoiceDock" };
        var rnd = new System.Random();
        sceneNames = sceneNames.OrderBy(item => rnd.Next()).ToList();
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
            SetSceneNamesAndLoad("Instructions Scene");
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1))
            SetSceneNamesAndLoad("EyeDock");
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2))
            SetSceneNamesAndLoad("TouchDock");
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha3))
            SetSceneNamesAndLoad("VoiceDock");
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha4))
            SetSceneNamesAndLoad("EyeHand");
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha5))
            SetSceneNamesAndLoad("TouchHand");
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha6))
            SetSceneNamesAndLoad("VoiceHand");

        if (Input.GetKeyDown(KeyCode.N))
        {
            writer.Close();
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.I))
        {
            ShowInstructions(0);
            StudyInstructionsManager.instance.SetInstructionsNumer(1);
            // StudyInstructionsManager.instance.ResetInstructionNumber();
        }
    }

    public void SetSceneNamesAndLoad(string newSceneName)
    {
        sceneChanged = true;
        if (sceneSystem.IsContentLoaded("Instructions Scene"))
            unloadSceneName = "Instructions Scene";
        else if (sceneSystem.IsContentLoaded("EyeDock"))
            unloadSceneName = "EyeDock";
        else if (sceneSystem.IsContentLoaded("TouchDock"))
            unloadSceneName = "TouchDock";
        else if (sceneSystem.IsContentLoaded("VoiceDock"))
            unloadSceneName = "VoiceDock";
        else if (sceneSystem.IsContentLoaded("EyeHand"))
            unloadSceneName = "EyeHand";
        else if (sceneSystem.IsContentLoaded("TouchHand"))
            unloadSceneName = "TouchHand";
        else if (sceneSystem.IsContentLoaded("VoiceHand"))
            unloadSceneName = "VoiceHand";
        switch (newSceneName)
        {
            case "Instructions Scene":
                showNotification = false;
                notificationSound = false;
                StudyInstructionsManager.instance.ResetInstructionNumber();
                break;
            default:
                showNotification = true;
                notificationSound = true;
                break;
        }
        GameManager.instance.SetSceneName(newSceneName);
        StudyInstructionsManager.instance.DisplayInstructionsScreen(GameState.Scene);
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
            csvData.AppendLine(participantNumber + "," + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + "," + time_s + "," + currGlobalRecordsGO.GetComponent<Records>().GetNotificationType() + "," + category + "," + action + "," + status + "," + ingredients);
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

    void ShowInstructions(int instructionNum)
    {
        if (!StudyBillboard.activeSelf)
        {
            StudyBillboard.SetActive(true);
        }
        StudyBillboard.GetComponentInParent<StudyInstructionsManager>().SetNextInstruction(instructionNum);
    }

    public GameObject GetStudyBillboard()
    {
        return StudyBillboard;
    }

    public int GetSceneIndex()
    {
        return sceneIndex;
    }

    public string GetNextScene()
    {
        return sceneNames[sceneIndex++];
    }
}
