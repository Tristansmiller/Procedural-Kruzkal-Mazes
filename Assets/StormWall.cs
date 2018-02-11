using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormWall : MonoBehaviour {
    GameObject storm;
    bool isFalling;
    bool waiting;
    bool rigidBodyInvoked;
    MazeGen controller;
	// Use this for initialization
	void Start () {
        storm = getStorm();
        if (this.gameObject.name == "Player")
        {
            controller = getController();
        }
        isFalling = false;
        waiting = false;
	}
    MazeGen getController()
    {
        var gameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject gameObj in gameObjects)
        {
            if (gameObj.name == "Main Camera")
            {
                return gameObj.GetComponent<MazeGen>();
            }
        }
        return null;
    }
    void setWait()
    {
        waiting = true;
    }
    void setResume()
    {
        waiting = false;
    }
	GameObject getStorm()
    {
        var gameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject gameObj in gameObjects)
        {
            if (gameObj.name == "Storm")
            {
                return gameObj;
            }
        }
        return null;
    }
    Vector2 getVectorFromStormCenter(Vector2 point)
    {
        return new Vector2(point.x - storm.transform.position.x, point.y - storm.transform.position.y);
    }
    bool isOutsideStorm()
    {
        Vector2 distanceFromCenter = getVectorFromStormCenter(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
        if(distanceFromCenter.magnitude>storm.transform.localScale.x/2){
            return true;
        }
        else
        {
            return false;
        }
    }
    void addRigidBody()
    {
        this.gameObject.AddComponent<Rigidbody2D>();
        isFalling = true;
    }
    void fall()
    {
        if (this.gameObject.name == "Player")
        {
            if (isOutsideStorm())
            {
                controller.Invoke("WaitForRestart", 0);
            }
        }
        else if(!rigidBodyInvoked)
        {
            this.Invoke("addRigidBody", .5f);
            rigidBodyInvoked = true;
        } 
    }
	// Update is called once per frame
	void Update () {
        if (!isFalling && !waiting)
        {
            if (storm != null)
            {
                if (isOutsideStorm())
                {
                    fall();
                }
            }
            else
            {
                storm = getStorm();
            }
        }
        else
        {
            if (this.gameObject.transform.position.y < (-15))
            {
                Destroy(this.gameObject);
            }
        }
	}
}
