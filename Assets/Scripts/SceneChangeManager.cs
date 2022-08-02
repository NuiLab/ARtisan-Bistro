using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SceneChangeManager : MonoBehaviour
{
    IMixedRealitySceneSystem sceneSystem;
    string unloadSceneName;

    // Start is called before the first frame update
    void Start()
    {
        sceneSystem = MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>();
    }

    // Update is called once per frame
  

    public void OnButtonClick(string sceneName)
    {
        // Debug.Log(sceneSystem.SourceName);
        if (sceneSystem.IsContentLoaded("Instructions Scene"))
            unloadSceneName = "Instructions Scene";
        else if (sceneSystem.IsContentLoaded("NoD Scene"))
            unloadSceneName = "NoD Scene";
        else if (sceneSystem.IsContentLoaded("NoO Scene"))
            unloadSceneName = "NoO Scene";
        else if (sceneSystem.IsContentLoaded("NoV Scene"))
            unloadSceneName = "NoV Scene";

        var task = LoadNextLevel(sceneName);
    }

    public async Task LoadNextLevel(string sceneName)
    {
        await sceneSystem.UnloadContent(unloadSceneName);
        await sceneSystem.LoadContent(sceneName);
    }
}
