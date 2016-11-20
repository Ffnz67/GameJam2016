﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;



public class Stats : MonoBehaviour {

    public int numberPerfect = 0;
    public int roomsCleared = 0;
    public int totalRooms = 0;
    public int restTime = 0;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if(roomsCleared == totalRooms)
        {
            SceneManager.LoadScene("EndScene");
        }
    }
	
}
