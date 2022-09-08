using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ParticipantNumManager : MonoBehaviour
{
    // [SerializeField] GameObject participantNum_GO;
    // [SerializeField] GameObject participantNum_TMP;

    public TouchScreenKeyboard keyboard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad);
        keyboard.active = true;
    }
}
