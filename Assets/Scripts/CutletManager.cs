using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutletManager : MonoBehaviour
{
    [SerializeField] float cookingSpeed;
    [SerializeField] GameObject globalRecords_GO;

    int prevQuotient;
    bool cookingState = false;
    float cookingProgress = 0;
    GameObject notification_GO;

    // Start is called before the first frame update
    void Start()
    {
        globalRecords_GO = GameObject.FindWithTag("Global Records");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetComponent<IngredientProperties>().GetCookingStatus() != "Burnt" && cookingState)
        {
            cookingProgress += Time.deltaTime;
            if (cookingProgress / cookingSpeed > prevQuotient + 1)
            {
                prevQuotient = (int)Math.Floor(cookingProgress / cookingSpeed);
                transform.GetComponent<IngredientProperties>().SetCookingStatus(prevQuotient, "Burger");
                if (globalRecords_GO.GetComponent<Records>().GetPersistentGO().GetComponent<PersistentGOManager>().GetShowNotification())
                {
                    string notifiText = "";
                    switch (prevQuotient)
                    {
                        case 0:
                            notifiText = "Uncooked";
                            break;
                        case 1:
                            notifiText = "Partially Cooked";
                            break;
                        case 2:
                            notifiText = "Cooked";
                            break;
                        case 3:
                            notifiText = "Over Cooked";
                            break;
                        case 4:
                            notifiText = "Burnt";
                            break;
                    }
                    switch (globalRecords_GO.GetComponent<Records>().GetNotificationType())
                    {
                        case 0:
                            if (notification_GO != null)
                                Destroy(notification_GO);
                            notification_GO = Instantiate(globalRecords_GO.GetComponent<Records>().GetNotificationPrefab());
                            notification_GO.GetComponent<NotificationManager>().SetNotificationProperties(notifiText, transform.gameObject, new Vector3(0, 0.4f, 0));
                            break;
                        case 1:
                            globalRecords_GO.GetComponent<Records>().AddNotificationOnDock(notifiText);
                            break;
                        case 2:
                            globalRecords_GO.GetComponent<Records>().AddNotificationOnViewport(notifiText);
                            break;
                    }
                }
            }
        }
    }

    public bool GetCookingState()
    {
        return cookingState;
    }

    public void SetCookingState(bool status)
    {
        cookingState = status;
    }
}
