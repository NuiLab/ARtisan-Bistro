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
    bool notificationSent = false;

    // Start is called before the first frame update
    void Start()
    {
        globalRecords_GO = GameObject.FindWithTag("Global Records");
    }

    private void OnDestroy()
    {
        if (globalRecords_GO != null)
        {
            if (globalRecords_GO.GetComponent<Records>().GetNotificationType().Equals(1))
                globalRecords_GO.GetComponent<Records>().RemoveNotificationOnDock(transform.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetComponent<IngredientProperties>().GetCookingStatus() != "Verbrannt" && cookingState)
        {
            cookingProgress += Time.deltaTime;
            if (cookingProgress / cookingSpeed > prevQuotient + 1)
            {
                prevQuotient = (int)Math.Floor(cookingProgress / cookingSpeed);
                transform.GetComponent<IngredientProperties>().SetCookingStatus(prevQuotient, "Burger");
                string notifiText = "";
                switch (prevQuotient)
                {
                    case 0:
                        notifiText = "Roh";
                        break;
                    case 1:
                        notifiText = "Fertig";
                        if (!notificationSent)
                        {
                            GameObject tempNoti = globalRecords_GO.GetComponent<Records>().addIngredientNotification("Burgergrill", "Patty gekocht", transform.GetInstanceID());
                            tempNoti.GetComponent<Notification>().ReceiveInput("Grill", "Fertig", "ea47");
                            notificationSent = true;
                        }
                        break;
                    case 2:
                        notifiText = "Verbrannt";
                        break;
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
        if (status)
            this.GetComponent<AudioSource>().Play();
    }
}
