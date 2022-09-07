using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ParticipantNumManager : MonoBehaviour
{
    [SerializeField] GameObject participantNum_GO;
    [SerializeField] GameObject participantNum_TMP;

    public TouchScreenKeyboard keyboard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (keyboard != null)
            participantNum_GO.GetComponent<TMP_InputField>().text = keyboard.text;
            // participantNum_TMP.GetComponent<>
        // Debug.Log(keyboard.text);
    }

    public void OnClick()
    {
        keyboard = TouchScreenKeyboard.Open("");
        Debug.Log(keyboard.status);
    }
}
