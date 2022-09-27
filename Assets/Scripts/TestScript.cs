using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] GameObject dockButton;
    [SerializeField] GameObject test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
  
    public void LogFunction(string logString)
    {
        Debug.Log(logString);
    }

    public void ButtonPress()
    {
        dockButton.GetComponent<NotificationDockManager>().AddNotification("test", "test" + dockButton.GetComponent<NotificationDockManager>().GetNotificationCountGO(), 0);
    }
}
