using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SceneAtom {

	#region Field Region

	[Header("Movies")]
	[SerializeField] GameObject sceneMovie;
	[SerializeField] GameObject wrongMovie;

	[Header("Scripts")]
	[SerializeField] ScriptCollection scriptCollection;

	[Header("Trigger")]
	[SerializeField] bool nextChapter;
	[SerializeField] bool stateChanger = false;
	[SerializeField] bool isOption = false;

	#endregion

	#region Property Region

	public ScriptCollection ScriptCollection
	{
		get { return scriptCollection; }
	}

	public bool NextChapter
	{
		get { return nextChapter; }
	}

	public bool StateChanger
	{
		get { return stateChanger; }
	}

	public bool IsOption
	{
		get { return isOption; }
	}

	public PlayableDirector StartSceneMovie
	{
		get { return sceneMovie.GetComponent<PlayableDirector>(); }
	}

	public PlayableDirector WrongSceneMovie
	{
		get { return wrongMovie.GetComponent<PlayableDirector>(); }
	}

	#endregion
}
