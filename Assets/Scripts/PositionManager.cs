using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    GameObject[] persistentGameObjects;

    // Start is called before the first frame update
    void Start()
    {
        persistentGameObjects = GameObject.FindGameObjectsWithTag("PersistentGO");
        if (persistentGameObjects.Length > 0)
        {
            if (persistentGameObjects[0].GetComponent<PersistentGOManager>().GetPosition().Equals(new Vector3(1000, 1000, 1000)))
            {
                persistentGameObjects[0].GetComponent<PersistentGOManager>().SetPosition(transform.position);
            }
            else
            {
                transform.position = persistentGameObjects[0].GetComponent<PersistentGOManager>().GetPosition();
            }
        }
    }

    // Update is called once per frame
        public void UpdatePosition()
    {
        persistentGameObjects[0].GetComponent<PersistentGOManager>().SetPosition(transform.position);
    }
}
