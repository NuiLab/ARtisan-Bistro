using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToPlaceManager : MonoBehaviour
{
    [SerializeField] GameObject playArea;
    [SerializeField] GameObject tablePositioningMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   

    public void placePlayArea()
    {
        playArea.transform.position = transform.position;
        playArea.transform.rotation = transform.rotation;
        playArea.SetActive(true);
        Instantiate(tablePositioningMenu);
        transform.gameObject.SetActive(false);
    }
}
