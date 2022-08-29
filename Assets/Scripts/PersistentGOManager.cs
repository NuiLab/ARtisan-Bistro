using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PersistentGOManager : MonoBehaviour
{
    [SerializeField] bool showNotification = false;

    Vector3 position = new Vector3(1000, 1000, 1000);
    GameObject currGlobalRecordsGO;
    IMixedRealitySceneSystem sceneSystem;
    string unloadSceneName;

    // Start is called before the first frame update
    void Start()
    {
        sceneSystem = MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>();
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetSceneNamesAndLoad("Instructions Scene");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            showNotification = true;
            SetSceneNamesAndLoad("NoD Scene");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            showNotification = true;
            SetSceneNamesAndLoad("NoO Scene");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            showNotification = true;
            SetSceneNamesAndLoad("NoV Scene");
        }
    }

    void SetSceneNamesAndLoad(string newSceneName)
    {
        if (sceneSystem.IsContentLoaded("Instructions Scene"))
            unloadSceneName = "Instructions Scene";
        else if (sceneSystem.IsContentLoaded("NoD Scene"))
            unloadSceneName = "NoD Scene";
        else if (sceneSystem.IsContentLoaded("NoO Scene"))
            unloadSceneName = "NoO Scene";
        else if (sceneSystem.IsContentLoaded("NoV Scene"))
            unloadSceneName = "NoV Scene";

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
}
