using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour{

	public void ShowToolTip() {
		gameObject.SetActive(true);
	}

	public void HideToolTip()
	{
		gameObject.SetActive(false);
	}
}
