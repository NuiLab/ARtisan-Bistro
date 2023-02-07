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
    [SerializeField] GameObject statusLight;
    [SerializeField] Material off;
    [SerializeField] Material on;

    bool cooking = false;
    float cookingProgress = 0;
    int prevQuotient = 0;
    GameObject food;
    GameObject notification_GO;

    bool notificationSent = false;


    // Start is called before the first frame update
    void Start()
    {
        globalRecords_GO = GameObject.FindWithTag("Global Records");
    }

    // Update is called once per frame
    void Update()
    {
        if (cooking && food.GetComponent<IngredientProperties>().GetCookingStatus() != "Verbrannt")
        {
            cookingProgress += Time.deltaTime;
            if (cookingProgress / cookingSpeed > prevQuotient + 1)
            {
                prevQuotient = (int)Math.Floor(cookingProgress / cookingSpeed);
                food.GetComponent<IngredientProperties>().SetCookingStatus(prevQuotient, "Pizza");
                switch (prevQuotient)
                {
                    case 0:
                        progressText_GO.GetComponent<TextMesh>().text = "Roh";
                        break;
                    case 1:
                        progressText_GO.GetComponent<TextMesh>().text = "Fertig";
                        if (!notificationSent)
                        {
                            GameObject tempNoti = globalRecords_GO.GetComponent<Records>().addIngredientNotification("Pizza", progressText_GO.GetComponent<TextMesh>().text, transform.GetInstanceID());
                            tempNoti.GetComponent<Notification>().ReceiveInput("Ofen", "Fertig", "f204");
                            tempNoti.GetComponent<Notification>().customer = globalRecords_GO;
                            notificationSent = true;
                        }

                        break;
                    case 2:
                        progressText_GO.GetComponent<TextMesh>().text = "Verbrannt";
                        cooking = false;
                        break;
                }
                /*
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
                }*/


                //globalRecords_GO.GetComponent<Records>().AddNotificationOnDock("Pizza", progressText_GO.GetComponent<TextMesh>().text, transform.GetInstanceID());

            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ingredient_Base") && !other.gameObject.GetComponent<ObjectManager>().isGrabbed && !cooking)
        {
            statusLight.GetComponent<StatusLight>().setOn();
            food = other.gameObject;
            other.transform.position = pizzaPos.transform.position;
            other.transform.rotation = Quaternion.identity;
            cooking = true;
            progressText_GO.GetComponent<TextMesh>().text = other.GetComponent<IngredientProperties>().GetCookingStatus();
            switch (other.GetComponent<IngredientProperties>().GetCookingStatus())
            {
                case "Roh":
                    cookingProgress = 0;
                    prevQuotient = 0;
                    break;
                case "Fertig":
                    cookingProgress = cookingSpeed * 1;
                    break;
                case "Verbrannt":
                    cookingProgress = cookingSpeed * 2;
                    break;
            }
            /*
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
            }*/
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
            statusLight.GetComponent<StatusLight>().setOff();
            other.transform.GetChild(1).gameObject.SetActive(false);
            progressText_GO.GetComponent<TextMesh>().text = "Place Food";
            if (notification_GO != null)
                Destroy(notification_GO);
        }
    }
}
