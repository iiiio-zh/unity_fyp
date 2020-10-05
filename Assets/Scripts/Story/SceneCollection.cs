using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCollection : MonoBehaviour {

	[SerializeField] public SceneAtom[] scenes;

	string chapterName;

	public string ChapterName
	{
		get { return chapterName; }
	}

	private void Start()
	{
		chapterName = this.gameObject.name;
	}

}
