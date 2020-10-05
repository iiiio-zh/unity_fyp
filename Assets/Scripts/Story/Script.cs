using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Script {

	#region Field Region

	[Header("Scripts")]
	[TextArea(2, 10)]
	[SerializeField] public string script;

	[Header("Correct answer")]
	[SerializeField] public bool isCorrect = false;

	#endregion

	#region Property Region

	public bool IsRead
	{
		get; set;
	}

	#endregion
}
