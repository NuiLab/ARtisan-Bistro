using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StudyInstructionsManager : MonoBehaviour
{
    [SerializeField] GameObject mainText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetMainText()
    {
        return mainText;
    }
}
