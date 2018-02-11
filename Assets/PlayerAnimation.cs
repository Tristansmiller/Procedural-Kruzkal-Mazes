using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerAnimation : MonoBehaviour {

    enum Direction  {North,South,East,West,NotMoving };
    float speed = 6f;
    float points = 0;
    Rigidbody2D rigidBody;
    bool westCollision;
    bool eastCollision;
    bool southCollision;
    bool northCollision;
    GameObject scoretext;
    bool frozen;
    Vector3 addVectors(Vector3 v1, Vector3 v2)
    {
        float xNew = v1.x + v2.x;
        float yNew = v1.y + v2.y;
        float zNew = v1.z + v2.z;
        return new Vector3(xNew, yNew, zNew);
    }

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        frozen = false;
        points = 0;
        scoretext = findScore();

	}
    GameObject findScore()
    {
        GameObject[] gameObjs = FindObjectsOfType<GameObject>();
        foreach(GameObject gameobj in gameObjs)
        {
            if(gameobj.name == "Score Text")
            {
                return gameobj;
            }
        }
        return null;
    }
    void updateScore()
    {
        scoretext.GetComponent<Text>().text = "Score: " + points;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "point" || collision.gameObject.name == "point(Clone)")
        {
            Destroy(collision.gameObject);
            points++;
            updateScore();
        }
    }
    void freeze()
    {
        frozen = true;
    }
    void unfreeze()
    {
        frozen = false;
        points = 0;
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    westCollision = false;
    //    northCollision = false;
    //    southCollision = false;
    //    eastCollision = false;
    //}
    // Update is called once per frame
    void Update() {
        float xForce = 0f;
        float yForce = 0f;
        xForce = speed * Input.GetAxis("Horizontal");
        yForce = speed * Input.GetAxis("Vertical");
        //if (eastCollision && xForce > 0)
        //{
        //    xForce = 0;
        //}
        //if(westCollision && xForce < 0)
        //{
        //    xForce = 0;
        //}
        //if(southCollision && yForce < 0)
        //{
        //    yForce = 0;
        //}
        //if(northCollision && yForce > 0)
        //{
        //    yForce = 0;
        //}
        // Vector2 movementForce = new Vector2(xForce, yForce);
        if (!frozen)
        {
            rigidBody.velocity = new Vector2(xForce, yForce);
        } 
        //Vector3 newRotation = addVectors(transform.rotation.eulerAngles, new Vector3(0, 0, 1));
       // transform.Rotate(new Vector3(0, 0, 2));
        
	}
}
