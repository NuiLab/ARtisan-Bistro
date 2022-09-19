using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SkipTutorial : MonoBehaviour
{
    GameObject globalRecords_GO;
    IMixedRealitySceneSystem sceneSystem;

    // Start is called before the first frame update
    void Start()
    {
        globalRecords_GO = GameObject.FindWithTag("Global Records");
        sceneSystem = MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>();
    }

    // Update is called once per frame


    public void ButtonPress()
    {
        globalRecords_GO.GetComponent<Records>().GetPersistentGO().GetComponent<PersistentGOManager>().SetShowNotification(true);
        LoadNextLevel();
        Destroy(transform.gameObject);
    }

    public void LoadNextLevel()
    {
        globalRecords_GO.GetComponent<Records>().GetPersistentGO().GetComponent<PersistentGOManager>().SetShowNotification(true);
        globalRecords_GO.GetComponent<Records>().GetPersistentGO().GetComponent<PersistentGOManager>().SetNotificationSound(true);
        globalRecords_GO.GetComponent<Records>().GetPersistentGO().GetComponent<PersistentGOManager>().SetSceneNamesAndLoad("NoO_WS Scene");
        /*
        await sceneSystem.UnloadContent("Instructions Scene");
        await sceneSystem.LoadContent("NoO_WS Scene");
        */
    }
}
