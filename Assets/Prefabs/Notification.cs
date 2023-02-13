using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Google.MaterialDesign.Icons;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Microsoft.MixedReality.Toolkit.Utilities;


public class Notification : MonoBehaviour
{

    public bool shouldDock;
    public float lifetime;
    [Tooltip("Calculates Notification Duration based on character count")]
    public bool calcLifetime;
    public MaterialIcon icon;
    public TextMeshPro voiceNumber;
    public GameObject manager;
    public MeshRenderer notificationBackground;
    public MeshRenderer buttonBackground;
    public GameObject dockObj;
    public GridObjectCollection dockGrid;
    bool isInHand;
    bool dockExists = false;
    [SerializeField] myEnum InteractionType = new myEnum();

    [Header("References")]
    public TextMeshPro title;
    public TextMeshPro content;
    public GameObject button;
    public MeshRenderer buttonText;
    public List<GameObject> closeButtons = new List<GameObject>(3);
    int stage = 1;
    public enum myEnum { Eye = 0, Touch = 1, Voice = 2 };
    public GameObject viewportGrid;
    public GameObject customer;

    void Update()
    {
        if (customer == null)
            Dismiss();
    }
    void Start()
    {
        setType();
        manager = GameObject.FindGameObjectsWithTag("Global Records")[0];
        dockObj = manager.GetComponent<Records>().dockObject; //needs to be the grid gameobject and not the actual dock
        viewportGrid = GameObject.FindGameObjectsWithTag("SubtitleGrid")[0];
        this.transform.SetParent(viewportGrid.transform);

        if (dockObj != null)
        {
            dockExists = true;
            if (dockObj.name == "HandGrid")
                isInHand = true;

            dockGrid = dockObj.GetComponent<GridObjectCollection>();
        }

        notificationBackground = this.gameObject.GetComponent<MeshRenderer>();
        notificationBackground.material = new Material(notificationBackground.material);

        SetButtonBackground(this.gameObject);
        buttonBackground.material = new Material(buttonBackground.material);

        if (calcLifetime) calculateLifetime();
        manager.GetComponent<Records>().notificationInViewport = true;

        viewportGrid.GetComponent<GridObjectCollection>().UpdateCollection();
        if (voiceNumber != null) viewportGrid.GetComponent<NotificationDock>().updateLetters();
        StartCoroutine(FadeInObject());
        StartCoroutine(Disappear());
    }

    // Update is called once per frame
    public void Dismiss(float delay = 0f)
    {
        StartCoroutine(DismissCR(delay));
    }
    void SetButtonBackground(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag("ButtonBackground"))
            {
                buttonBackground = child.gameObject.GetComponent<MeshRenderer>();
            }
            SetButtonBackground(child.gameObject);
        }
    }
    void SetVoiceText(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag("IconNumber"))
            {
                voiceNumber = child.gameObject.GetComponent<TextMeshPro>();
            }
            SetVoiceText(child.gameObject);
        }
    }
    public void buttonDismiss(float delay = 0f)
    {
        notificationDataLog("Food notification", "dismissed");
        StartCoroutine(DismissCR(delay));
    }
    public string GetNotificationType()
    {
        return this.InteractionType.ToString();
    }
    public IEnumerator DismissCR(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(FadeOutObject());
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
        if (dockGrid != null)
        {
            dockGrid.UpdateCollection();
            dockObj.GetComponent<NotificationDock>().updateNumbers();
        }
    }
    IEnumerator Disappear()
    {

        yield return new WaitForSeconds(lifetime - 1f);
        if (shouldDock)
        {
            goToDock();
            manager.GetComponent<Records>().notificationInViewport = false;
        }
        else
        {
            StartCoroutine(FadeOutObject());
            yield return new WaitForSeconds(1f);
            Destroy(this.gameObject);
        }
    }

    void notificationDataLog(string notificationCat, string action)
    {
        manager.GetComponent<Records>().GetPersistentGO().GetComponent<PersistentGOManager>().AddData(notificationCat, action + ":" + this.GetInstanceID().ToString());
    }
    void calculateLifetime()
    {
        lifetime = content.text.Length * 0.1f;
    }

    public void ReceiveInput(string titleText = null, string contentText = null, string iconText = null)
    {
        if (titleText != null) title.text = titleText;
        if (contentText != null) content.text = contentText;
        if (iconText != null) icon.iconUnicode = iconText;
        // iconText is like "\uE84D". Person is "\e7df", burger is e57a, pizza e552, coffee efef, cofee maker eff0, grill ea47, oven e843
    }
    public void setTransparent()
    {
        Color color = notificationBackground.materials[0].color;
        Color colorB = buttonBackground.materials[0].color;
        Color colorButtonX = buttonText.materials[0].color;
        Color iconColor = icon.color;

        color.a = 0f;
        colorB.a = 0f;
        iconColor.a = 0f;
        title.alpha = 0f;
        content.alpha = 0f;
        colorButtonX.a = 0f;

        notificationBackground.materials[0].color = color;
        buttonBackground.materials[0].color = colorB;
        icon.color = iconColor;
        buttonText.materials[0].color = colorButtonX;
    }

    public IEnumerator FadeOutObject()
    {
        float speed = 0.03f;
        // Get the mesh renderer of the object

        Color color = notificationBackground.materials[0].color;
        Color colorB = buttonBackground.materials[0].color;
        Color colorButtonX = buttonText.materials[0].color;
        Color iconColor = icon.color;
        // While the color's alpha value is above 0
        while (color.a > 0)
        {
            // Reduce the color's alpha value
            color.a -= speed;
            colorB.a -= speed;
            colorButtonX.a -= speed;
            title.alpha -= speed;
            content.alpha -= speed;
            iconColor.a -= speed;

            icon.color = iconColor;
            notificationBackground.materials[0].color = color;
            buttonBackground.materials[0].color = colorB;
            buttonText.materials[0].color = colorButtonX;
            // Wait for the frame to update
            yield return new WaitForEndOfFrame();
        }

        // If the material's color's alpha value is less than or equal to 0, end the coroutine
        yield return new WaitUntil(() => notificationBackground.materials[0].color.a <= 0f);
    }
    public IEnumerator FadeInObject()
    {
        setTransparent();
        float speed = 0.01f;

        Color color = notificationBackground.materials[0].color;
        Color colorB = buttonBackground.materials[0].color;
        Color iconColor = icon.color;
        Color colorButtonX = buttonText.materials[0].color;
        // While the color's alpha value is above 0
        while (color.a < 1)
        {
            // Reduce the color's alpha value
            color.a += speed;
            colorB.a += speed;
            colorButtonX.a += speed;
            iconColor.a += speed;
            title.alpha += speed;
            content.alpha += speed;

            // Apply the modified color to the object's mesh renderer
            icon.color = iconColor;
            notificationBackground.materials[0].color = color;
            buttonBackground.materials[0].color = colorB;
            buttonText.materials[0].color = colorButtonX;
            // Wait for the frame to update
            yield return new WaitForEndOfFrame();
        }

        // If the material's color's alpha value is less than or equal to 0, end the coroutine
        yield return new WaitUntil(() => notificationBackground.materials[0].color.a == 1f);
    }
    public void goToDock()
    {
        this.gameObject.GetComponent<Orbital>().enabled = false;
        if (isInHand && dockObj.transform.parent.gameObject.active == false)
        {
            dockObj.transform.parent.gameObject.SetActive(true);
        }
        this.gameObject.transform.SetParent(dockObj.transform);

        if (isInHand)
        {
            Vector3 newScale = new Vector3(0.095f, 0.03f, 0.1f);
            this.gameObject.transform.localScale = newScale;

        }
        dockGrid.UpdateCollection();
        if (GetNotificationType() == "Voice") dockObj.GetComponent<NotificationDock>().updateNumbers();
        this.gameObject.transform.localRotation = Quaternion.identity;
        if (isInHand && manager.GetComponent<Records>().handMenuOpen == false)
            dockObj.transform.parent.gameObject.SetActive(false);

    }
    void setType()
    {
        switch (InteractionType.ToString())
        {
            case "Eye":
                closeButtons[0].SetActive(true);
                button = closeButtons[0];
                break;
            case "Touch":
                closeButtons[1].SetActive(true);
                button = closeButtons[1];
                break;
            case "Voice":
                closeButtons[2].SetActive(true);
                button = closeButtons[2];
                SetVoiceText(this.gameObject);
                break;
        }
    }
    public void nextStage()
    {
        switch (stage)
        {
            case 1:
                notificationDataLog("Order Notification", "accepted");
                stage++;
                customer.GetComponent<CustomerManager>().startTimer();
                string tempContent = customer.GetComponent<CustomerManager>().translateIngredients();
                ReceiveInput(null, tempContent, getFoodIcon());
                break;
            case 2:
                notificationDataLog("Order Notification", "complete");
                customer.GetComponent<CustomerManager>().completedOrder = true;
                ReceiveInput(this.title.text, "Bestellung fertig", "e5ca");
                Dismiss(2f);
                break;
            case 3:
                Destroy(this.transform.gameObject);
                break;
            default:
                break;
        }
    }
    string getFoodIcon()
    {
        switch (customer.GetComponent<CustomerManager>().foodCategory)
        {
            case "Pizza":
                return "e552";

            case "Burger":
                return "e57a";

            case "Coffee":
                return "efef";

            default:
                return "";

        }
    }
}


