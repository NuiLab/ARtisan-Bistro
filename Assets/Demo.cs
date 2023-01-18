using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    public GameObject notificationPrefab;
    public float interval;
    public GameObject demoObject;
    // Start is called before the first frame update
    void Start()
    {
        demoFunction();
    }
    private void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            demoObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void demoFunction()
    {
        StartCoroutine("demoCR");
    }
    IEnumerator demoCR()
    {

        Instantiate(notificationPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        yield return new WaitForSeconds(interval);
        demoFunction();
    }
}
