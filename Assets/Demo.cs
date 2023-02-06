using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    public GameObject notificationPrefab;
    public float interval;
    public GameObject demoObject;
    public GameObject demoNotification;
    // Start is called before the first frame update
    void Start()
    {
        demoFunction();
    }
    private void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            demoNotification.GetComponent<Notification>().setTransparent();
        }
         if (Input.GetKeyDown("n"))
        {
            demoNotification.GetComponent<Notification>().FadeInObject();
        }
    }

    // Update is called once per frame
    void demoFunction()
    {
        StartCoroutine("demoCR");
    }
    IEnumerator demoCR()
    {

        demoNotification = Instantiate(notificationPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        yield return new WaitForSeconds(interval);
        demoFunction();
    }
}
