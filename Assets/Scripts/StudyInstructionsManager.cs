using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StudyInstructionsManager : MonoBehaviour
{
    [SerializeField] GameObject mainText;
    [SerializeField] GameObject continueBtn;

    public static StudyInstructionsManager instance;

    IMixedRealitySceneSystem sceneSystem;
    List<string> instructions = new List<string>();
    int instructionsNum = -1;
    Dictionary<string, string> sceneInstructions = new Dictionary<string, string>();
    bool startTasks = false;


    private void Awake()
    {
        instance = this;
        sceneSystem = MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>();
        InitializeInstructions();
        InitializeSceneInstructions();
        GameManager.OnGameStateChanged += DisplayInstructionsScreen;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= DisplayInstructionsScreen;
    }

    public void DisplayInstructionsScreen(GameState state)
    {
        startTasks = false;
        if (!state.Equals(GameState.Setup) && instructionsNum != -1)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }


        switch (state)
        {
            case GameState.Setup:
                instructionsNum = 0;
                break;
            case GameState.Welcome:
                if (instructionsNum == -1)
                {
                    instructionsNum = 0;
                    transform.GetChild(0).gameObject.SetActive(true);
                }
                mainText.GetComponent<TextMeshProUGUI>().text = instructions[instructionsNum++];
                break;
            case GameState.Scene:
                if (GameManager.instance.GetSceneName().Equals("EyeDock"))
                    mainText.GetComponent<TextMeshProUGUI>().text = sceneInstructions["EyeDock"];
                else if (GameManager.instance.GetSceneName().Equals("EyeHand"))
                    mainText.GetComponent<TextMeshProUGUI>().text = sceneInstructions["EyeHand"];
                else if (GameManager.instance.GetSceneName().Equals("TouchDock"))
                    mainText.GetComponent<TextMeshProUGUI>().text = sceneInstructions["TouchDock"];
                else if (GameManager.instance.GetSceneName().Equals("TouchHand"))
                    mainText.GetComponent<TextMeshProUGUI>().text = sceneInstructions["TouchHand"];
                else if (GameManager.instance.GetSceneName().Equals("VoiceDock"))
                    mainText.GetComponent<TextMeshProUGUI>().text = sceneInstructions["VoiceDock"];
                else if (GameManager.instance.GetSceneName().Equals("VoiceHand"))
                    mainText.GetComponent<TextMeshProUGUI>().text = sceneInstructions["VoiceHand"];
                break;
            case GameState.End:
                mainText.GetComponent<TextMeshProUGUI>().text = instructions[instructions.Count - 1];
                continueBtn.SetActive(false);
                break;
        }
    }


    public GameObject GetMainText()
    {
        return mainText;
    }

    void InitializeSceneInstructions()
    {
        /*   sceneInstructions.Add("Instructions Scene", "In this section you will follow a tutorial to learn how to interact with the objects. " +
              "There will be instructions with an arrow pointing to the object used to complete the current task. If the instruction is not in the viewport there will be an arrow guiding you to it. " +
              "After completing each task press the right arrow to go to the next instruction. \n\n" +
              "There will be 3 customers each asking for different food item. You will have all the time you need to present them the food item they requested. " +
              "After preparing the food place it on the tray in front of the customer. If the food item is wrong, there will be a text stating it. If the food item is correct the customer will leave. \n\n" +
              "Once you are done serving all customers, you can press the next button on the last instruction (located at the left of trash can) to start the actual experiment."); */

        sceneInstructions.Add("Instructions Scene", "In diesem Teil werden Sie einer Einführung folgen um zu lernen wie man mit den versch. Objekten interagiert. " +
       "Es werden Ihnen Pfeile eingeblendet die zu den jeweiligen Objekten zeigen, die Sie für die aktuelle Aufgabe benötigen." +
       "Wenn sie die Anweisung befolgt haben, drücken Sie mit Ihrem Finger auf den Pfeil um zur nächsten Anweisung zu kommen. \n\n" +
       "Es werden 3 Kunden warten, die jeweils eine der 3 verschiedenen Essen bestellen. In der Erklärung haben sie so lange Zeit wie Sie brauchen. " +
       "Sobald Sie das Essen zubereitet haben, stellen Sie es auf das Tablett vor dem Kunden. Falls das Essen richtig zubereitet wurde verschwindet der Kunde. \n\n" +
       "Bedienen Sie die 3 Kunden und folgem dem Tutorial bitte bis zum Schluss.");

        sceneInstructions.Add("EyeDock", "In diesem Durchlauf  existiert ein Dock in der Welt in dem alle Benachrichtigungen angezeigt werden sobald Sie nicht mehr im Sichtfeld sind. " +
            "Dieses Dock ist ähnlich wie der Ort wo an Ihrem Smartphone die Benachrichtigungen angezeigt werden " +
            "Sie können, so wie die anderen Objekte im Restaurant auch, das Dock greifen und bewegen.  \n\n " +
            "Jede Benachrichtigung hat einen Knopf. Wenn es sich um eine Bestellung handelt schaltet der Knopf die Bestellung weiter. Bei einer Koch Benachrichtigung schliesst der Knopf die Benachrichtigung.\n " +
            "Der Knopf wird hier betätigt, indem sie den Knopf mit ihren Augen anschauen und ca 1 Sekunde fokussieren.  ");

        sceneInstructions.Add("TouchDock", "In diesem Durchlauf existiert ein Dock in der Welt in dem alle Benachrichtigungen angezeigt werden sobald Sie nicht mehr im Sichtfeld sind. " +
            "Dieses Dock ist ähnlich wie der Ort wo an Ihrem Smartphone die Benachrichtigungen angezeigt werden " +
            "Sie können, so wie die anderen Objekte im Restaurant auch, das Dock greifen und bewegen.  \n\n " +
            "Jede Benachrichtigung hat einen Knopf. Wenn es sich um eine Bestellung handelt schaltet der Knopf die Bestellung weiter. Bei einer Koch Benachrichtigung schliesst der Knopf die Benachrichtigung.\n " +
            "Der Knopf wird hier betätigt, indem sie den Knopf  mit ihrem Zeigefinger drücken.");

        sceneInstructions.Add("VoiceDock", "In diesem Durchlauf existiert ein Dock in der Welt in dem alle Benachrichtigungen angezeigt werden sobald Sie nicht mehr im Sichtfeld sind. " +
       "Dieses Dock ist ähnlich wie der Ort wo an Ihrem Smartphone die Benachrichtigungen angezeigt werden " +
       "Sie können, so wie die anderen Objekte im Restaurant auch, das Dock greifen und bewegen.  \n\n " +
       "Jede Benachrichtigung hat einen Knopf. Wenn es sich um eine Bestellung handelt schaltet der Knopf die Bestellung weiter. Bei einer Koch Benachrichtigung schliesst der Knopf die Benachrichtigung.\n " +
       "Jeder Knopf hat eine Zahl die für die Betätigung notwendig ist \n " +
       "Der Knopf wird hier betätigt, indem sie folgenden Sprachbefehl sagen: Auswahl *Zahl auf dem Knopf*.");
        sceneInstructions.Add("EyeHand", "In diesem Durchlauf  werden ihnen Benachrichtigungen  sobald Sie nicht mehr im Sichtfeld sind neben Ihrer linken Hand angezeigt. " +
                    "Dieses Menü ist ähnlich wie der Ort wo an Ihrem Smartphone die Benachrichtigungen angezeigt werden " +
                    "Sie können dieses Menü aufrufen, indem Sie Ihre linke Handfläche ansehen. \n\n " +
                    "Jede Benachrichtigung hat einen Knopf. Wenn es sich um eine Bestellung handelt schaltet der Knopf die Bestellung weiter. Bei einer Koch Benachrichtigung schliesst der Knopf die Benachrichtigung.\n " +
                    "Der Knopf wird hier betätigt, indem sie den Knopf mit ihren Augen anschauen und ca 1 Sekunde fokussieren.  ");

        sceneInstructions.Add("TouchHand", "In diesem Durchlauf  werden ihnen Benachrichtigungen  sobald Sie nicht mehr im Sichtfeld sind neben Ihrer linken Hand angezeigt. " +
                    "Dieses Menü ist ähnlich wie der Ort wo an Ihrem Smartphone die Benachrichtigungen angezeigt werden " +
                    "Sie können dieses Menü aufrufen, indem Sie Ihre linke Handfläche ansehen. \n\n " +
            "Jede Benachrichtigung hat einen Knopf. Wenn es sich um eine Bestellung handelt schaltet der Knopf die Bestellung weiter. Bei einer Koch Benachrichtigung schliesst der Knopf die Benachrichtigung.\n " +
            "Der Knopf wird hier betätigt, indem sie den Knopf  mit ihrem Zeigefinger drücken.");

        sceneInstructions.Add("VoiceHand", "In diesem Durchlauf  werden ihnen Benachrichtigungen  sobald Sie nicht mehr im Sichtfeld sind neben Ihrer linken Hand angezeigt. " +
                    "Dieses Menü ist ähnlich wie der Ort wo an Ihrem Smartphone die Benachrichtigungen angezeigt werden " +
                    "Sie können dieses Menü aufrufen, indem Sie Ihre linke Handfläche ansehen. \n\n " +
       "Jede Benachrichtigung hat einen Knopf. Wenn es sich um eine Bestellung handelt schaltet der Knopf die Bestellung weiter. Bei einer Koch Benachrichtigung schliesst der Knopf die Benachrichtigung.\n " +
       "Jeder Knopf hat eine Zahl die für die Betätigung notwendig ist \n " +
       "Der Knopf wird hier betätigt, indem sie folgenden Sprachbefehl sagen: Auswahl *Zahl auf dem Knopf*.");



        sceneInstructions.Add("Control", "In diesem Durchlauf gibt es keine Benachrichtigungen " +
            "Die Bestellungen werden Ihnen über dem Tablett der Kunden angezeigt. Passen Sie auf die Geräte auf und kochen Sie alle Bestellungen.");
    }
    void InitializeInstructions()
    {
        instructions.Add("Willkommen zu der Studie \"Notification Interaktion in Augmented Reality\"");

        instructions.Add("Die folgenden Texte werden Ihnen helfen mit den verschienden Elementen in der Umgebung zu interagieren. Die meisten Fragen werden Ihnen durch die Texte erklärt. " +
       "Danach folgt eine Passage in der ihnen interaktiv die Anwendung gezeigt wird. \n\n" +
       "Hinweis: Falls Sie Fragen haben stellen Sie diese bitte sofort oder während der interaktiven Erklärung");

        instructions.Add("Aufgabe\n\n Willkommen im Bistro. In dieser Studie geht es darum wie in einem Fast Food Restaurant Kunden zu bedienen und Essen zuzubereiten. " +
            "Es gibt entweder eine Burger- Pizza- oder Kaffeebestellung. Ihre Aufgabe wird es sein, so schnell wie möglich alle Bestellungen der Kunden innerhalb einer Schicht zu bearbeiten.");


        instructions.Add("Kunden Station\n\nKunden erscheinen an der Theke. Jeder Kunde hat eine eigene Bestellung für ein Gericht. " +
            "Sobald ein Kunde erscheint erhalten Sie eine Benachrichtigung. Um die Bestellung zu starten, betätigen Sie bitte den Knopf auf der Benachrichtigung. Ab dann wird Ihnen die Bestellung angezeigt und die Wartezeit startet. \n\n" +
            "Wenn die Wartezeit abgelaufen ist geht der Kunde ohne seine Bestellung. Wenn die Bestellung zubereitet ist, stellen Sie bitte das Essen auf das Tablett und bestätigen sie die Bestellung, indem Sie den Knopf auf der Bestell-benachrichtigung betätigen");

        instructions.Add("Burger Station\n\nHier sind alle Zutaten die für das Zubereiten eines Burgers notwendig sind. Rechts neben den Zutaten befindet sich ein Grill, auf dem sie das Burger Patty zubereiten müssen. " +
            "Das Patty ist zu Beginn roh, wird dann gekocht, und fängt dann zu brennen an wenn es verkocht ist. Sie erhalten eine Benachrichtigung sobald das Patty fertig ist.\n\nKunden wollen immer ein gekochtes Patty.");

        instructions.Add("Pizza Station\n\nHier sind alle Zutaten die für das Zubereiten einer Pizza notwendig sind. Links neben den Zutaten ist der Ofen, in dem die Pizza gebacken werden muss. " +
            "Belegen Sie immer zuerst die Pizza und legen Sie sie dann in den Ofen. " +
            "Die Pizza ist zunächst roh, ist dann fertig und wenn sie zu lange im Ofen ist verbrannt. Sie erhalten eine Benachrichtigung wenn die Pizza fertig ist. \n\nKunden wollen immer eine fertige Pizza.");

        instructions.Add("Kaffee Station\n\nHier finden sie die Kaffeemaschine und Kaffeebecher. " +
            "Wenn die Kaffeemaschine läuft erscheint neben der Maschine eine Anzeige wie viele Becher in der Kanne sind. " +
            "Um Kaffe einzuschenken, nehmen Sie bitte einen Becher aus dem Spender und schenken erst dann ein. " +
            "Sobald der Becher voll ist erscheint automatisch ein Deckel auf dem Becher und der Kaffee kann ausgegeben werden.");

        instructions.Add("In diesem Teil werden Sie einer Einführung folgen um zu lernen wie man mit den versch. Objekten interagiert. " +
       "Es werden Ihnen Pfeile eingeblendet die zu den jeweiligen Objekten zeigen, die Sie für die aktuelle Aufgabe benötigen." +
       "Wenn sie die Anweisung befolgt haben, drücken Sie mit Ihrem Finger auf den Pfeil um zur nächsten Anweisung zu kommen. \n\n" +
       "Es werden 3 Kunden warten, die jeweils eine der 3 verschiedenen Essen bestellen. In der Erklärung haben sie so lange Zeit wie Sie brauchen. " +
       "Sobald Sie das Essen zubereitet haben, stellen Sie es auf das Tablett vor dem Kunden. Falls das Essen richtig zubereitet wurde verschwindet der Kunde. \n\n" +
       "Bedienen Sie die 3 Kunden und folgem dem Tutorial bitte bis zum Schluss.");
        // NEED NOTIFICATION TUTORIAL
        instructions.Add("Danke für die Teilnahme");
    }

    public void SetNextInstruction(int index)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = instructions[index];
    }

    public void Continue()
    {
        if (instructionsNum < instructions.Count - 1)
            GameManager.instance.UpdateGameState(GameState.Welcome);
        else
        {
            startTasks = true;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public bool GetStartTask()
    {
        return startTasks;
    }

    public void ResetInstructionNumber()
    {
        instructionsNum = -1;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetInstructionsNumer(int instructionNumber)
    {
        instructionsNum = instructionNumber;
    }
}
