using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenManager : MonoBehaviour
{
    [SerializeField] GameObject pizzaPos;
    [SerializeField] float cookingSpeed;
    [SerializeField] GameObject progressText_GO;
    [SerializeField] GameObject globalRecords_GO;

    bool cooking = false;
    float cookingProgress = 0;
    int prevQuotient = 0;
    GameObject food;
    GameObject notification_GO;

    // Start is called before the first frame update
    void Start()
    {
        globalRecords_GO = GameObject.FindWithTag("Global Records");
    }

    // Update is called once per frame
    void Update()
    {
        if (cooking && food.GetComponent<IngredientProperties>().GetCookingStatus() != "Burnt")
        {
            cookingProgress += Time.deltaTime;
            if (cookingProgress / cookingSpeed > prevQuotient + 1)
            {
                prevQuotient = (int)Math.Floor(cookingProgress / cookingSpeed);
                food.GetComponent<IngredientProperties>().SetCookingStatus(prevQuotient, "Pizza");
                switch (prevQuotient)
                {
                    case 0:
                        progressText_GO.GetComponent<TextMesh>().text = "Uncooked";
                        break;
                    case 1:
                        progressText_GO.GetComponent<TextMesh>().text = "Partially Cooked";
                        break;
                    case 2:
                        progressText_GO.GetComponent<TextMesh>().text = "Cooked";
                        break;
                    case 3:
                        progressText_GO.GetComponent<TextMesh>().text = "Over Cooked";
                        break;
                    case 4:
                        progressText_GO.GetComponent<TextMesh>().text = "Burnt";
                        cooking = false;
                        break;
                }
                if (globalRecords_GO.GetComponent<Records>().GetPersistentGO().GetComponent<PersistentGOManager>().GetShowNotification())
                {
                    switch (globalRecords_GO.GetComponent<Records>().GetNotificationType())
                    {
                        case 0:
                            if (notification_GO != null)
                                Destroy(notification_GO);
                            notification_GO = Instantiate(globalRecords_GO.GetComponent<Records>().GetNotificationPrefab());
                            notification_GO.GetComponent<NotificationManager>().SetNotificationProperties(progressText_GO.GetComponent<TextMesh>().text, transform.gameObject, new Vector3(-0.7f, 1.5f, 0), new Quaternion(0, 0.707106829f, 0, 0.707106829f));
                            break;
                        case 1:
                            globalRecords_GO.GetComponent<Records>().AddNotificationOnDock(progressText_GO.GetComponent<TextMesh>().text);
                            break;
                        case 2:
                            globalRecords_GO.GetComponent<Records>().AddNotificationOnViewport(progressText_GO.GetComponent<TextMesh>().text);
                            break;
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ingredient_Base") && !other.gameObject.GetComponent<ObjectManager>().isGrabbed && !cooking)
        {
            food = other.gameObject;
            other.transform.position = pizzaPos.transform.position;
            other.transform.rotation = Quaternion.identity;
            cooking = true;
            progressText_GO.GetComponent<TextMesh>().text = other.GetComponent<IngredientProperties>().GetCookingStatus();
            switch (other.GetComponent<IngredientProperties>().GetCookingStatus())
            {
                case "Uncooked":
                    cookingProgress = 0;
                    break;
                case "Partially Cooked":
                    cookingProgress = cookingSpeed * 1;
                    break;
                case "Cooked":
                    cookingProgress = cookingSpeed * 2;
                    break;
                case "Over Cooked":
                    cookingProgress = cookingSpeed * 3;
                    break;
                case "Burnt":
                    cookingProgress = cookingSpeed * 4;
                    break;
            }
        }
        else if (other.CompareTag("Ingredient_Base") && other.gameObject.GetComponent<ObjectManager>().isGrabbed)
        {
            cooking = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ingredient_Base") && other.gameObject.GetComponent<ObjectManager>().isGrabbed)
        {
            other.transform.GetChild(1).gameObject.SetActive(false);
            progressText_GO.GetComponent<TextMesh>().text = "Place Food";
            if (notification_GO != null)
                Destroy(notification_GO);
        }
    }
}
