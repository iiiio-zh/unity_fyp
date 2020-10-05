//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Playables;
//using UnityEngine.Windows.Speech;

//public class StoryManager_Backup : MonoBehaviour
//{

//	#region Field Region
//	public static StoryManager Instance;

//	[Header("Story")]
//	[SerializeField] GameObject story;

//	[Header("Story Canvas")]
//	[SerializeField] Text chapterText;
//	[SerializeField] Text storyText;

//	[Header("Helper Buttons")]
//	[SerializeField] GameObject nextScriptButton;
//	[SerializeField] GameObject nextChapterButton;

//	private List<SceneCollection> scripts;
//	private int currentChapterIndex = 0;
//	private int currentSceneIndex = 0;
//	private int maxChapterNum;
//	private int maxSceneNum;

//	private KeywordRecognizer script_Recogniser;

//	#endregion

//	#region Initialise Region

//	void Awake()
//	{
//		//This is a common approach to handling a class with a reference to itself.
//		//If instance variable doesn't exist, assign this object to it
//		if (Instance == null)
//			Instance = this;
//		//Otherwise, if the instance variable does exist, but it isn't this object, destroy this object.
//		//This is useful so that we cannot have more than one GameManager object in a scene at a time.
//		else if (Instance != this)
//			Destroy(this);
//	}

//	//Use this for initialization

//	void Start()
//	{
//		scripts = new List<SceneCollection>();
//	}

//	public void InitialiseScripts()
//	{
//		Transform storyRoot = story.transform;
//		maxChapterNum = storyRoot.childCount;
//		//Debug.Log(storyRoot.childCount);
//		for (int i = 0; i < storyRoot.childCount; i++)
//		{
//			scripts.Add(storyRoot.GetChild(i).GetComponent<SceneCollection>());
//			Debug.Log(scripts[i].ChapterName);
//		}

//	}

//	public void InitialiseVoiceControl()
//	{
//		string[] script_Keywords = new string[scripts[currentChapterIndex].scenes.Length];

//		//for (int i = 0; i < scripts[currentChapterIndex].scenes.Length; i++)
//		//{
//		//	script_Keywords[i] = scripts[currentChapterIndex].scenes[i].Script;
//		//	Debug.Log(script_Keywords[i]);
//		//}

//		script_Recogniser = new KeywordRecognizer(script_Keywords, ConfidenceLevel.Low);
//		script_Recogniser.OnPhraseRecognized += OnScriptRecognized;
//		script_Recogniser.Start();

//	}

//	#endregion

//	#region Method Region

//	public void StartChapter()
//	{
//		InitialiseScripts();
//		InitialiseVoiceControl();
//		SceneCollection currentSceneCollection = scripts[currentChapterIndex];
//		maxSceneNum = currentSceneCollection.scenes.Length;

//		chapterText.text = currentSceneCollection.ChapterName;

//		DisplayNextSentence(true);
//	}

//public string GetSentence()
	//{
	//		return scripts[currentChapterIndex].scenes[currentSceneIndex].Script;
	//}

//	public void EnableChapterSwitcher()
//	{
//		SceneAtom tempAtom = scripts[currentChapterIndex].scenes[currentSceneIndex];
//		if (tempAtom.NextChapter && tempAtom.IsRead)
//			nextChapterButton.SetActive(true);
//		else
//			nextChapterButton.SetActive(false);
//	}

//	public void EnableNextScript()
//	{
//		if (scripts[currentChapterIndex].scenes[currentSceneIndex].IsRead)
//			nextScriptButton.SetActive(true);
//		else
//			nextScriptButton.SetActive(false);
//	}

//	public void DisplayNextSentence(bool start = false)
//	{
//		if (!start && currentSceneIndex < maxSceneNum - 1)
//			currentSceneIndex++;

//		DisplayHelper();
//	}

//	public void DisplayPreviousSentence()
//	{
//		if (currentSceneIndex > 0)
//			currentSceneIndex--;

//		DisplayHelper();
//	}

//	private void DisplayHelper()
//	{

//		EnableChapterSwitcher();
//		EnableNextScript();

//		string sentence = GetSentence();

//		StopAllCoroutines();
//		StartCoroutine(TypeSentence(sentence, storyText));

//		PlayableDirector tempDirector = scripts[currentChapterIndex].scenes[currentSceneIndex].StartSceneMovie;
//		if (tempDirector)
//			tempDirector.Play();

//	}

//	public void AdvanceChapter()
//	{
//		if (currentChapterIndex < maxChapterNum - 1)
//		{
//			currentChapterIndex++;
//		}
//		currentSceneIndex = 0;

//		StartChapter();
//	}

//	IEnumerator TypeSentence(string sentence, Text textBox)
//	{
//		textBox.text = "";
//		foreach (char letter in sentence.ToCharArray())
//		{
//			textBox.text += letter;
//			yield return null;
//		}
//	}

//	private void OnScriptRecognized(PhraseRecognizedEventArgs args)
//	{
//		Debug.Log(args.text + " " + args.confidence);

//		if (args.text == GetSentence())
//		{
//			scripts[currentChapterIndex].scenes[currentSceneIndex].IsRead = true;
//			EnableChapterSwitcher();
//			EnableNextScript();
//		}

//	}

//	#endregion
//}
