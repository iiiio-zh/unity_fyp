using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Windows.Speech;

public class StoryManager : MonoBehaviour
{

	#region Field Region
	public static StoryManager Instance;

	[Header("Story")]
	[SerializeField] GameObject story;
	[SerializeField] GameObject prologue;
	[SerializeField] int branchingChapter;
	[SerializeField] int skipChapter;

	[Header("Story Canvas")]
	[SerializeField] Text text1;
	[SerializeField] Text text2;

	//Move this to story manager!
	[Header("Story Panel")]
	[SerializeField] Animator controlPanel;
	[SerializeField] GameObject nextChapterButton;
	[SerializeField] GameObject replayButton;

	[Header("Reminder Billboard")]
	[SerializeField] Animator billBoard;
	[SerializeField] AudioClip[] reminderClips;

	private List<SceneCollection> chapters;
	private List<ScriptCollection> scripts;
	private float currCountdownValue;
	private int maxChapterNum;
	private int maxSceneNum;
	private int scriptCount;
	private string currentScriptToBeRead;
	private string altCurrentScriptToBeRead;
	private AudioSource privateSpeaker;

	//pauses a coroutine
	private bool reminderFunctionMod;
	private bool replaying;

	private KeywordRecognizer script_Recogniser;

	[Header("Variables (Do not touch)")]
	public int currentChapterIndex = 0;
	public int currentSceneIndex = 0;
	public bool storyEnded = false;

	#endregion

	#region Initialise Region

	void Awake()
	{
		//This is a common approach to handling a class with a reference to itself.
		//If instance variable doesn't exist, assign this object to it
		if (Instance == null)
			Instance = this;
		//Otherwise, if the instance variable does exist, but it isn't this object, destroy this object.
		//This is useful so that we cannot have more than one GameManager object in a scene at a time.
		else if (Instance != this)
			Destroy(this);
	}

	//Use this for initialization

	void Start()
	{
		privateSpeaker = GetComponent<AudioSource>();
		chapters = new List<SceneCollection>();
		scripts = new List<ScriptCollection>();
	}

	public void InitialiseStory()
	{
		Transform storyRoot = story.transform;
		maxChapterNum = storyRoot.childCount;
		for (int i = 0; i < storyRoot.childCount; i++)
		{
			chapters.Add(storyRoot.GetChild(i).GetComponent<SceneCollection>());
			Debug.Log(chapters[i].ChapterName);
		}
	}

	public void InitialiseVoiceControl()
	{
		// A chapter has many scenes, now it will loop through the chapter's scenes
		// A scene may have multiple options or none
		for (int i = 0; i < chapters[currentChapterIndex].scenes.Length; i++)
		{
			scripts.Add(chapters[currentChapterIndex].scenes[i].ScriptCollection);
			Debug.Log("Added scene " + i + " from chapter " + currentChapterIndex);
		}

		// We need to add all scripts in the chapter
		// Double for loop because ScriptCollection has a class script which contains 
		// the actual script along some conditions
		List<string> tempHolder = new List<string>();

		foreach (ScriptCollection temp in scripts)
		{
			Debug.Log(temp.scripts.Length);
			if (temp.scripts.Length > 0)
			{
				for (int i = 0; i < temp.scripts.Length; i++)
				{
					tempHolder.Add(temp.scripts[i].script);
				}
			}
		}

		if (tempHolder.Count > 0)
		{
			string[] script_Keywords = new string[tempHolder.Count];

			for (int i = 0; i < script_Keywords.Length; i++)
			{
				script_Keywords[i] = tempHolder[i];
			}


			script_Recogniser = new KeywordRecognizer(script_Keywords, ConfidenceLevel.Low);
			script_Recogniser.OnPhraseRecognized += OnScriptRecognized;
			script_Recogniser.Start();
		}

		scripts.Clear();

	}

	public void DisablePrologue()
	{
		if (prologue.gameObject.activeInHierarchy)
			prologue.SetActive(false);
	}

	public void EnablePrologue()
	{
		if (!prologue.gameObject.activeInHierarchy)
			prologue.SetActive(true);

	}

	#endregion

	#region Method Region

	public void StartChapter(int lastSceneIndex, bool nextChapter = false)
	{
		// this section is hard coded
		if (nextChapter && currentChapterIndex < maxChapterNum)
		{
			if (currentChapterIndex == branchingChapter)
			{
				// to switch between magic wand chapter and backpack chapter
				switch (GameManager_Alt.Instance.currentStoryState)
				{
					case GameManager_Alt.StoryState.MagicWand:
						currentChapterIndex++;
						break;
					case GameManager_Alt.StoryState.Backpack:
						currentChapterIndex = currentChapterIndex + 2;
						break;
					case GameManager_Alt.StoryState.Default:
						currentChapterIndex++;
						break;
				}
			}
			else if (currentChapterIndex == skipChapter)
				currentChapterIndex = currentChapterIndex + 2;
			else
				currentChapterIndex++;

			currentSceneIndex = 0;
		}

		if (currentChapterIndex >= maxChapterNum)
		{
			// set the last scene of last chapter false
			chapters[currentChapterIndex - 1].scenes[lastSceneIndex].StartSceneMovie.gameObject.SetActive(false);
			storyEnded = true;
			return;
		}

		InitialiseVoiceControl();
		SceneCollection currentSceneCollection = chapters[currentChapterIndex];
		maxSceneNum = currentSceneCollection.scenes.Length;

		Debug.Log("currentChapterIndex: " + currentChapterIndex + " currentSceneIndex: " + currentSceneIndex);
		Debug.Log("maxChapNum: " + maxChapterNum + " maxSceneNum: " + maxSceneNum);

		DisplayNextScript(true);
	}

	public void DisplayNextScript(bool start = false)
	{
		if (!start && currentSceneIndex < maxSceneNum)
			currentSceneIndex++;

		DisplayHelper();
		PlayCurrentSceneMovie();

		scriptCount = chapters[currentChapterIndex].scenes[currentSceneIndex].ScriptCollection.scripts.Length;
		bool advanceChapter = chapters[currentChapterIndex].scenes[currentSceneIndex].NextChapter;

		if (scriptCount > 0)
		{
			Debug.Log("> 0");
			IEnumerator function = DelayPanelShow((float)chapters[currentChapterIndex].scenes[currentSceneIndex].StartSceneMovie.duration);
			StartCoroutine(function);
		}
		else if (scriptCount <= 0 && advanceChapter)
		{
			Debug.Log("<= 0 chap");
			StartChapter(currentSceneIndex, true);
		}
		else if (scriptCount <= 0)
		{
			Debug.Log("<= 0");
			IEnumerator function = DelayNextScript((float)chapters[currentChapterIndex].scenes[currentSceneIndex].StartSceneMovie.duration);
			StartCoroutine(function);
		}



	}

	private void DisplayHelper()
	{
		Script[] scriptsHolder = chapters[currentChapterIndex].scenes[currentSceneIndex].ScriptCollection.scripts;

		switch (scriptsHolder.Length)
		{
			case 1:
				currentScriptToBeRead = scriptsHolder[0].script;
				text1.text = "1. " + scriptsHolder[0].script;
				text2.text = "";
				break;
			case 2:
				currentScriptToBeRead = scriptsHolder[0].script;
				altCurrentScriptToBeRead = scriptsHolder[1].script;
				text1.text = "1. " + scriptsHolder[0].script;
				text2.text = "2. " + scriptsHolder[1].script;
				break;
			default:
				Debug.Log("No script found");
				break;
		}

		EnableChapterSwitcher();
	}

	public void EnableChapterSwitcher()
	{
		SceneAtom tempAtom = chapters[currentChapterIndex].scenes[currentSceneIndex];
		if (tempAtom.NextChapter)
			nextChapterButton.SetActive(true);
		else
			nextChapterButton.SetActive(false);
	}

	private void PlayCurrentSceneMovie()
	{
		chapters[currentChapterIndex].scenes[currentSceneIndex].StartSceneMovie.gameObject.SetActive(true);
		chapters[currentChapterIndex].scenes[currentSceneIndex].StartSceneMovie.Play();
	}

	public void ReplayCurrentSceneMovie()
	{
		chapters[currentChapterIndex].scenes[currentSceneIndex].StartSceneMovie.Play();
		IEnumerator function = DelayReplay((float)chapters[currentChapterIndex].scenes[currentSceneIndex].StartSceneMovie.duration);
		StartCoroutine(function);
	}

	IEnumerator DelayReplay(float time)
	{
		replaying = true;
		currCountdownValue += Mathf.FloorToInt(time);
		yield return new WaitForSeconds(time);
		replaying = false;
	}

	public IEnumerator RemindingResponse(float countdownValue)
	{
		// https://answers.unity.com/questions/225213/c-countdown-timer.html
		int lastSceneIndex = currentSceneIndex;

		currCountdownValue = countdownValue;
		while (currCountdownValue > 0 && lastSceneIndex == currentSceneIndex && scriptCount > 0)
		{
			Debug.Log("Countdown: " + currCountdownValue);
			yield return new WaitForSeconds(1.0f);
			if (!reminderFunctionMod && !replaying)
				currCountdownValue--;
			if (currCountdownValue == 1)
			{
				int index = Random.Range(0, reminderClips.Length);
				privateSpeaker.clip = reminderClips[index];
				privateSpeaker.Play();
				billBoard.SetTrigger("ShowHide");
				IEnumerator function = DelayBillBoardHide(3f);
				StartCoroutine(function);
				currCountdownValue = countdownValue;
			}
		}
	}

	IEnumerator DelayBillBoardHide(float time)
	{
		yield return new WaitForSeconds(time);
		billBoard.SetTrigger("ShowHide");
	}

	IEnumerator DelayPanelShow(float time, bool isWrong = false)
	{
		yield return new WaitForSeconds(time);
		controlPanel.SetTrigger("ShowHide");
		// set the wait timer here
		IEnumerator function = RemindingResponse(15f);
		if (!isWrong)
			StartCoroutine(function);
		else
		{
			reminderFunctionMod = true;
			yield return new WaitForSeconds(time);
			reminderFunctionMod = false;
		}

	}

	IEnumerator DelayNextScript(float time)
	{
		controlPanel.SetTrigger("ShowHide");
		yield return new WaitForSeconds(time);
		chapters[currentChapterIndex].scenes[currentSceneIndex].StartSceneMovie.gameObject.SetActive(false);
		DisplayNextScript();
	}

	private void NextScript()
	{
		controlPanel.SetTrigger("ShowHide");
		chapters[currentChapterIndex].scenes[currentSceneIndex].StartSceneMovie.gameObject.SetActive(false);
		DisplayNextScript();
	}

	//Only used when wrong answer given
	private void RespondMovie()
	{
		controlPanel.SetTrigger("ShowHide");
		chapters[currentChapterIndex].scenes[currentSceneIndex].WrongSceneMovie.gameObject.SetActive(true);
		chapters[currentChapterIndex].scenes[currentSceneIndex].WrongSceneMovie.Play();

		IEnumerator function = DelayPanelShow((float)chapters[currentChapterIndex].scenes[currentSceneIndex].WrongSceneMovie.duration, true);
		StartCoroutine(function);
	}

	private void OnScriptRecognized(PhraseRecognizedEventArgs args)
	{
		Debug.Log(args.text + " " + args.confidence);

		// check if it's a StateChanger, will change the game state
		if (chapters[currentChapterIndex].scenes[currentSceneIndex].StateChanger)
			CheckState(args.text);

		if (chapters[currentChapterIndex].scenes[currentSceneIndex].IsOption)
		{
			foreach (Script temp in chapters[currentChapterIndex].scenes[currentSceneIndex].ScriptCollection.scripts)
			{
				if (args.text == temp.script && temp.isCorrect)
				{
					Debug.Log("1");
					NextScript();
					break;
				}
				else if (args.text == temp.script)
				{
					Debug.Log("2");
					RespondMovie();
				}
			}
		}
		else
		{
			foreach (Script temp in chapters[currentChapterIndex].scenes[currentSceneIndex].ScriptCollection.scripts)
			{
				if (args.text == temp.script)
				{
					NextScript();
				}
			}
		}

	}

	private void CheckState(string text)
	{
		switch (text)
		{
			case "Magic Wand":
				GameManager_Alt.Instance.SetGameState(GameManager_Alt.StoryState.MagicWand);
				Debug.Log("GameState set to magic wand");
				break;
			case "Backpack":
				GameManager_Alt.Instance.SetGameState(GameManager_Alt.StoryState.Backpack);
				Debug.Log("GameState set to backpack");
				break;
			default:
				break;
		}
	}

	#endregion
}

// To access a chapter's scene
// chapters[currentChapterIndex].scenes[currentSceneIndex].
// access the scene's script collection
// chapters[currentChapterIndex].scenes[i].ScriptCollection 