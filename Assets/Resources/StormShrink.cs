using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StormShrink : MonoBehaviour {
    GameObject time;
    GameObject findTime()
    {
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        foreach(GameObject gameobj in gameObjects)
        {
            if(gameobj.name=="Time Text")
            {
                return gameobj;
            }
        }
        return null;
    }
	// Use this for initialization
	void Start () {
        time = findTime();
	}
    // Update is called once per frame
    void Update () {
        if (transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x - .01f, transform.localScale.y - .01f, transform.localScale.z);
            GetComponent<CircleCollider2D>().radius = (transform.localScale.x / 2);
            time.GetComponent<Text>().text = "Total Collapse in " + GetComponent<CircleCollider2D>().radius / .01f;
        }
	}
}
