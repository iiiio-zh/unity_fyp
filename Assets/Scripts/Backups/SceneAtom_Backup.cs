using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace SceneAtom_Backup {
	[System.Serializable]
	public class SceneAtom
	{

		[Header("Movies")]
		[SerializeField] GameObject startSceneMovie;
		[SerializeField] GameObject endSceneMovie;

		[Header("Scripts & Hint")]
		[TextArea(3, 10)]
		[SerializeField] string script;

		[TextArea(3, 10)]
		[SerializeField] string alternateScript;

		[TextArea(1, 10)]
		[SerializeField] string hint;

		[TextArea(1, 10)]
		[SerializeField] string alternateHint;

		[Header("Trigger")]
		[SerializeField] bool isOptional;
		[SerializeField] bool isProgressor;
		[SerializeField] bool glassTrigger;
		[SerializeField] bool bottleTrigger;

		private bool isRead = false;

		public string Script
		{
			get { return script; }
		}

		public string AlternateScript
		{
			get { return alternateScript; }
		}

		public string Hint
		{
			get { return hint; }
		}

		public string AlternateHint
		{
			get { return alternateHint; }
		}

		public bool IsOptional
		{
			get { return isOptional; }
			set { isOptional = value; }
		}

		public bool IsProgressor
		{
			get { return isProgressor; }
		}

		public bool IsBottleTrigger
		{
			get { return bottleTrigger; }
		}

		public bool IsGlassTrigger
		{
			get { return glassTrigger; }
		}

		public bool IsRead
		{
			get { return isRead; }
			set { isRead = value; }
		}

		public PlayableDirector StartSceneMovie
		{
			get { return startSceneMovie.GetComponent<PlayableDirector>(); }
		}

		public PlayableDirector EndSceneMovie
		{
			get { return endSceneMovie.GetComponent<PlayableDirector>(); }
		}
	}

}
