using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMenuHint : MonoBehaviour {

	[SerializeField] GameObject hint;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			if(hint.activeInHierarchy)
				hint.SetActive(false);
			else
				hint.SetActive(true);
		}
			
	}
}
