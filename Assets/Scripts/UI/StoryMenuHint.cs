using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryMenuHint : MonoBehaviour {

	[SerializeField] GameObject hint1;
	[SerializeField] GameObject hint2;
	[SerializeField] GameObject hint3;
	[SerializeField] GameObject hint4;
	[SerializeField] GameObject hint5;


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			if (hint1.activeInHierarchy)
				hint1.SetActive(false);
			else
				hint1.SetActive(true);
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			if (hint2.activeInHierarchy)
				hint2.SetActive(false);
			else
				hint2.SetActive(true);
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			if (hint3.activeInHierarchy)
				hint3.SetActive(false);
			else
				hint3.SetActive(true);
		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			if (hint4.activeInHierarchy)
				hint4.SetActive(false);
			else
				hint4.SetActive(true);
		}

		if (Input.GetKeyDown(KeyCode.Y))
		{
			if (hint5.activeInHierarchy)
				hint5.SetActive(false);
			else
				hint5.SetActive(true);
		}
	}
}
