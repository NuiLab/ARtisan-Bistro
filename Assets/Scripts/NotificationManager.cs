using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] GameObject text_GO;
    [SerializeField] float duration;

    // Start is called before the first frame update
    void Start()
    {
        if (duration > 0)
            StartCoroutine(NotificationDuration(duration));
    }

 
    public void setText(string text)
    {
        text_GO.GetComponent<TextMeshPro>().text = text;
    }

    public string getText()
    {
        return text_GO.GetComponent<TextMeshPro>().text;
    }

    IEnumerator PauseForSound()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(transform.gameObject);
    }

    public void OnNotificationClick()
    {
        transform.parent.parent.GetComponent<NotificationDockManager>().ManageNotificationLayout(gameObject);
        StartCoroutine(PauseForSound());
    }

    public void SetNotificationProperties(string text, GameObject parent, Vector3? pos = null, Quaternion? rot = null, Vector3? scale = null)
    {
        if (pos == null)
            pos = Vector3.zero;
        if (rot == null)
            rot = Quaternion.identity;
        if (scale == null)
            scale = transform.localScale;
        setText(text);
        transform.SetParent(parent.transform);
        transform.localPosition = pos.Value;
        transform.localRotation = rot.Value;
        transform.localScale = scale.Value;
    }

    IEnumerator NotificationDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(transform.gameObject);
    }
}
