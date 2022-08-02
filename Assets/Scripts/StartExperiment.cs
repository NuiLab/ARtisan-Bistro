using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StartExperiment : MonoBehaviour
{
    IMixedRealitySceneSystem sceneSystem;

    // Start is called before the first frame update
    void Start()
    {
        sceneSystem = MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>();
    }

    // Update is called once per frame


    public void ButtonPress()
    {
        var task = LoadNextLevel();
        Destroy(transform.gameObject);
    }

    public async Task LoadNextLevel()
    {
        await sceneSystem.LoadContent("Instructions Scene");
    }
}
