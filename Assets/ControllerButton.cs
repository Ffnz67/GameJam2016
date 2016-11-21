﻿using UnityEngine;
using System.Collections;

public class ControllerButton : MonoBehaviour {
	void Start () {
		transform.GetChild(0).gameObject.SetActive(!FindObjectOfType<ControllerManager>().UsingController);
	}

	public void OnClick () {
		FindObjectOfType<ControllerManager>().ToggleSetting();
		transform.GetChild(0).gameObject.SetActive(!FindObjectOfType<ControllerManager>().UsingController);
	}
}
