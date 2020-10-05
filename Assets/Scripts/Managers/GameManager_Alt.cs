using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.Windows.Speech;

public class GameManager_Alt : MonoBehaviour
{

	#region Field Region

	public static GameManager_Alt Instance;

	[Header("Menu Canvas")]
	[SerializeField] GameObject menuCanvas;

	[Header("Story Canvas")]
	[SerializeField] GameObject storyCanvas;

	[Header("Commands")]
	[SerializeField]string[] commands;

	public StoryState currentStoryState = StoryState.Default;
	private KeywordRecognizer command_Recogniser;

	#endregion

	#region State Region

	public enum StoryState
	{
		Default,
		MagicWand,
		Backpack
	}

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

	//This method reloads the scene after the player has been defeated
	void ReloadScene()
	{
		//Get a reference to the current scene
		Scene currentScene = SceneManager.GetActiveScene();
		//Tell the SceneManager to load the current scene (which reloads it)
		SceneManager.LoadScene(currentScene.buildIndex);
	}

	private void Start()
	{
		command_Recogniser = new KeywordRecognizer(commands, ConfidenceLevel.Low);
		command_Recogniser.OnPhraseRecognized += OnCommandRecognized;
		command_Recogniser.Start();
	}

	#endregion

	#region Method Region

	private void Update()
	{
		if (StoryManager.Instance.storyEnded)
		{
			//StoryManager.Instance.EndStory();
			StoryManager.Instance.EnablePrologue();
			storyCanvas.SetActive(false);
			menuCanvas.SetActive(true);
		}

	}

	public void ToggleCanvas(GameObject canvas, bool toggle)
	{
		if (canvas)
			canvas.SetActive(toggle);
	}

	public void ToggleMenuCanvas(bool toggle)
	{
		ToggleCanvas(menuCanvas, toggle);
	}

	public void ToggleStoryCanvas(bool toggle)
	{
		ToggleCanvas(storyCanvas, toggle);
	}

	public void EndGame()
	{
		Application.Quit();
	}

	public void StartGame()
	{
		ToggleMenuCanvas(false);
		ToggleStoryCanvas(true);
		StoryManager.Instance.currentChapterIndex = 0;
		StoryManager.Instance.currentSceneIndex = 0;
		StoryManager.Instance.storyEnded = false;
		StoryManager.Instance.DisablePrologue();
		StoryManager.Instance.InitialiseStory();
		StoryManager.Instance.StartChapter(0);

	}

	public void SetGameState(StoryState state)
	{
		currentStoryState = state;
	}

	private void OnCommandRecognized(PhraseRecognizedEventArgs args)
	{
		Debug.Log(args.text + " " + args.confidence);
		switch (args.text)
		{
			case "Start Game":
				StartGame();
			break;
			case "Quit Game":
				EndGame();
			break;
			case "Replay":
				StoryManager.Instance.ReplayCurrentSceneMovie();
			break;
		}
	}

	#endregion
}
