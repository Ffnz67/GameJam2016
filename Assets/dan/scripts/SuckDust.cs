﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SuckDust : MonoBehaviour {
	[SerializeField]
	private float _centerSuckForce;

	[SerializeField]
	private float _backwardSuckForceMultiplier;

	private List<GameObject> _suckableDust;

	private CircleCollider2D _collider;

	private Plug _plug;

	private bool _sucking;

	private void Awake () {
		_suckableDust = new List<GameObject>();
	}

	private void Start () {
		_collider = GetComponent<CircleCollider2D>();
		_plug = FindObjectOfType<Plug>();
	}

	private void Update () {
		_sucking = Input.GetMouseButton(0) && _plug.Connected;
	}

	private void FixedUpdate () {
		if (_sucking) {
			foreach (GameObject dust in _suckableDust) {
				//TODO This is super hacky (sometimes dust can be null after beeing destroyed [not removed from list])
				if (dust != null) {
					Vector3 suckPosToDustPos = dust.transform.position - transform.position;
					float dotProduct = Mathf.Abs(Mathf.Max(Vector3.Dot(transform.right, suckPosToDustPos.normalized), -_backwardSuckForceMultiplier));
					float distanceFalloff = Mathf.Max(ExtensionMethods.Remap(suckPosToDustPos.magnitude, 0.0f, _collider.radius, 1.0f, 0.0f), 0.0f);
					dust.GetComponent<Rigidbody2D>().AddForce(-suckPosToDustPos.normalized * dotProduct * distanceFalloff * _centerSuckForce);
				}
			}
		}
	}

	private void OnTriggerEnter2D (Collider2D pOther) {
		if (pOther.tag == "Dust") {
			_suckableDust.Add(pOther.gameObject);
		}
		
	}

	private void OnTriggerExit2D (Collider2D pOther) {
		if (pOther.tag == "Dust") {
			_suckableDust.Remove(pOther.gameObject);
		}
	}

	public void RemoveDust (GameObject pDust) {
		_suckableDust.Remove(pDust);
	}
}
