using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TableMenuManager : MonoBehaviour
{
    GameObject playArea;
    GameObject tapToPlace;
    float transformFactor = 1;
    GameObject[] persistentGameObjects;


    // Start is called before the first frame update
    void Start()
    {
        persistentGameObjects = GameObject.FindGameObjectsWithTag("PersistentGO");
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.name.Equals("Play Area"))
                playArea = go;
            if (go.name.Equals("Tap To Place"))
                tapToPlace = go;
        }       
    }

    // Update is called once per frame
   

    public void Push()
    {
        playArea.transform.position += new Vector3(0, 0, 0.01f * transformFactor);
    }

    public void Pull()
    {
        playArea.transform.position += new Vector3(0, 0, -0.01f * transformFactor);
    }

    public void Left()
    {
        playArea.transform.position += new Vector3(-0.01f * transformFactor, 0, 0);
    }

    public void Right()
    {
        playArea.transform.position += new Vector3(0.01f * transformFactor, 0, 0);
    }

    public void Up()
    {
        playArea.transform.position += new Vector3(0, 0.01f * transformFactor, 0);
    }

    public void Down()
    {
        playArea.transform.position += new Vector3(0, -0.01f * transformFactor, 0);
    }

    public void ResetSetup()
    {
        playArea.SetActive(false);
        tapToPlace.SetActive(true);
        Destroy(transform.gameObject);
    }

    public void FinishSetup()
    {
        persistentGameObjects[0].GetComponent<PersistentGOManager>().SetPosition(playArea.transform.position);
        playArea.transform.Find("Instruction Manager").gameObject.SetActive(true);
        Destroy(transform.gameObject);
    }

    public void sliderUpdated(SliderEventData eventData)
    {
        transformFactor = eventData.NewValue * 4 + 1;
    }
}
