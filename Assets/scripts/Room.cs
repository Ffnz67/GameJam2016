﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Room : MonoBehaviour
{

	private ChatBehaviour chat;

	public enum RoomType { TYPE_O, TYPE_I, TYPE_L, TYPE_T, TYPE_X }
	public RoomType type;

	public List<GameObject> doors;
	public List<GameObject> dust;
	public List<GameObject> tiles;

	public List<GameObject> mapIcons;

	public GameObject doneMapIcon;

	private bool done = false;
	private bool perfect = true;

	void Awake()
	{
		doors = new List<GameObject>();
		tiles = new List<GameObject>();
		dust = new List<GameObject>();
	}


	void Start()
	{
		FurnitureBehaviour[] f = GetComponentsInChildren<FurnitureBehaviour>();

		foreach (FurnitureBehaviour fe in f)
		{
			fe.ParentRoom = this;
		}

		chat = GameObject.FindGameObjectWithTag("Chat").GetComponent<ChatBehaviour>();

		GetComponent<RandomDustPlacement>().GenerateDust();
	}


	void Update()
	{
		if (!done)
		{
			float dustInPercent = 0f;

			foreach (GameObject d in dust)
			{
				if (d != null) dustInPercent += 1;
			}

			if (dust.Count == 0)
			{
				dustInPercent = 0;
			}
			else dustInPercent /= dust.Count;
			tiles.ForEach((x) =>
			{
				SpriteRenderer sr = x.GetComponent<SpriteRenderer>();
				sr.color = Color.HSVToRGB(0, 0, ((1f - dustInPercent) * 0.5f) + 0.5f);
			});
			mapIcons.ForEach((x) =>
			{
				SpriteRenderer sr = x.GetComponent<SpriteRenderer>();
				Color c = sr.color;
				sr.color = new Color(dustInPercent, (1f - dustInPercent), 0f);
			});

			if (dustInPercent == 0)
			{
				done = true;
				GameObject o = Instantiate(doneMapIcon, transform.position, Quaternion.identity) as GameObject;
				if (perfect) o.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);
				else o.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

				if (dust.Count > 0)
				{
					FindObjectOfType<Sparkling>().PlaySparkle(transform.position, transform.lossyScale.x);
					Stats s = GameObject.FindGameObjectWithTag("Stats").GetComponent<Stats>();

					s.Cleared++;
					s.ClearedPerfect += perfect ? 1 : 0;

					if (perfect) chat.PopUp(2);
					int sec = perfect ? -20 : -10;

					GameObject.FindGameObjectWithTag("uiTimer").GetComponent<UITimeBehaviour>().AddPunish(sec);

					GameObject textPrefab = GameObject.FindGameObjectWithTag("FloorManager").GetComponent<FloorComponents>().TextPrefab;

					GameObject text = Instantiate(textPrefab, transform.position, Quaternion.identity) as GameObject;
					text.GetComponent<TextMesh>().color = new Color(0, 1, 0);
					text.GetComponent<TextMesh>().text = (perfect ? "PERFECT! " : " ") + (-sec) + " Sec";
					text.transform.DOBlendableMoveBy(Vector2.up, 4).OnComplete<Tween>(() => Destroy(text));
				}
			}
		}

	}

	public void StuffSmashed()
	{
		perfect = false;
	}

}
