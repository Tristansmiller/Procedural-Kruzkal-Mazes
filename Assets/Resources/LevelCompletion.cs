using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletion : MonoBehaviour {
    MazeGen controller;
	// Use this for initialization
    MazeGen findController()
    {
        var gameObjects = FindObjectsOfType<GameObject>();
        foreach(GameObject gameObj in gameObjects)
        {
            if(gameObj.name == "Main Camera")
            {
                return gameObj.GetComponent<MazeGen>();
            }
        }
        
        return null;
        
    }
	void Start () {
        controller = findController();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Destroy(collision.gameObject.GetComponent < StormWall > ());
            controller.Invoke("CreateNewMaze", 0);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
