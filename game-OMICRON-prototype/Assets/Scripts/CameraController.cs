﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	Vector3 offset;
	
	void Start () {

		offset = transform.position;
	}
	
	void Update () {

		transform.position = player.transform.position + offset; 
	}
}
