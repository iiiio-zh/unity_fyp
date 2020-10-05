using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFlash : MonoBehaviour {

	Text startTextAnnouncement;

	bool m_Fading = false;

	float blinkduration = 0.5f;
	float timer = 0f;

	void Start () {
		startTextAnnouncement = GetComponent<Text>();
	}
	

	void Update () {

		timer += Time.deltaTime;
		if (m_Fading == true)
		{
			startTextAnnouncement.CrossFadeAlpha(1, blinkduration, false);
			Toggle(false);
		}

		if (m_Fading == false)
		{
			startTextAnnouncement.CrossFadeAlpha(0, blinkduration, false);
			Toggle(true);
		}
	}

	void Toggle (bool value)
	{
		if (timer > blinkduration)
		{
			m_Fading = value;
			timer = 0f;
		}
	}

}
