using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationDock : MonoBehaviour
{
    private List<GameObject> listOfChildren;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateNumbers()
    {   int i = 1;
        if (null == this)
            return;

        foreach (Transform child in this.transform)
        {
            if (null == child)
                continue;
            //child.gameobject contains the current child you can do whatever you want like add it to an array
            child.GetComponent<Notification>().voiceNumber.text = i.ToString();
            i++;
            
        }
    }
}
