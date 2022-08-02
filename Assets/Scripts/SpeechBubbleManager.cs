using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeechBubbleManager : MonoBehaviour
{

    [SerializeField] GameObject contentArea;
    [SerializeField] GameObject contentText;
    [SerializeField] GameObject[] contentImage;

    List<string> contentArray;
    float scaleFactor = 0.37f;
    float posFactor = 0.7f;
    GameObject globalRecords_GO;
    string foodItemRequested;

    private void Awake()
    {
        globalRecords_GO = GameObject.FindWithTag("Global Records");
    }


   

    void UpdateShape()
    {
        if (foodItemRequested == "Coffee")
            contentArea.transform.localScale = new Vector3(1, 0.9f, 1);
        else
            contentArea.transform.localScale = new Vector3(1, 0.11f + contentArray.Count * scaleFactor, 1);
    }

    void UpdateContent()
    {
        // contentText.GetComponent<TextMeshPro>().text = "";
        for (int i = 0; i < contentArray.Count; i++)
        {
            GameObject tmpContentImage;
            // contentText.GetComponent<TextMeshPro>().text += ("\n" + s);
            switch (foodItemRequested)
            {
                case "Coffee":
                    tmpContentImage = Instantiate(contentImage[1]);
                    tmpContentImage.transform.parent = transform;
                    tmpContentImage.transform.localPosition = new Vector3(2.154f, 1.706f, -0.02f);
                    tmpContentImage.transform.localRotation = new Quaternion(0, -0.707106829f, 0.707106829f, 0);
                    tmpContentImage.transform.localScale = new Vector3(0.2f, 0.1f, 0.123f);
                    break;
                case "Pizza":
                    tmpContentImage = Instantiate(contentImage[0]);
                    tmpContentImage.GetComponent<Renderer>().material = globalRecords_GO.GetComponent<Records>().GetPizzaIngredientMaterial(contentArray[i]);
                    tmpContentImage.transform.parent = transform;
                    tmpContentImage.transform.localPosition = new Vector3(2.154f, 0.6f + (i + 1) * posFactor, -0.02f);
                    tmpContentImage.transform.localRotation = new Quaternion(0, -0.707106829f, 0.707106829f, 0);
                    tmpContentImage.transform.localScale = new Vector3(0.2f, 0.1f, 0.1f);
                    break;
                case "Burger":
                    tmpContentImage = Instantiate(contentImage[0]);
                    tmpContentImage.GetComponent<Renderer>().material = globalRecords_GO.GetComponent<Records>().GetBurgerIngredientMaterial(contentArray[i]);
                    tmpContentImage.transform.parent = transform;
                    tmpContentImage.transform.localPosition = new Vector3(2.154f, 0.6f + (i + 1) * posFactor, -0.02f);
                    tmpContentImage.transform.localRotation = new Quaternion(0, -0.707106829f, 0.707106829f, 0);
                    tmpContentImage.transform.localScale = new Vector3(0.2f, 0.1f, 0.1f);
                    break;
            }
        }
    }

    public void SetContent(List<string> newContentArray, string foodItem)
    {
        contentArray = newContentArray;
        foodItemRequested = foodItem;
        UpdateShape();
        UpdateContent();
    }
}
