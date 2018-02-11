using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradualGravity : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
	}
}
