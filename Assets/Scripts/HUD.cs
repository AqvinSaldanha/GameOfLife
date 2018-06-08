using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Handles all the UI controls.
/// </summary>
public class HUD : MonoBehaviour {
 
	// Create delegate and events to inform other objects of UI activity
	public delegate void UIAction();
	public static event UIAction StartClicked;
	public static event UIAction PauseClicked;
	public static event UIAction ClearClicked;


	public delegate void UISpeedAdjustAction(float value);
	public static event UISpeedAdjustAction ChangeSpeed;

	public delegate void UIActionPatternClick(int value);
	public static event UIActionPatternClick ShowPattern;


	bool m_paused= true;

	[SerializeField]
	Text m_startButtonText;

	[SerializeField]
	Slider m_cameraSlider;

	[SerializeField]
	Slider m_speedSlider;

	[SerializeField]
	GameObject m_howToPlay;

	void Awake()
	{ 
		m_startButtonText.text = "Start";  
	}

	public void StartPause()
	{ 	
		AdjustSpeed ();
		if (m_paused) {
			m_paused = false;
			m_startButtonText.text = "Pause";
			StartClicked ();
		} else {
			m_paused = true;
			m_startButtonText.text = "Start";
			PauseClicked ();
		}
	} 

	public void ClearWorld()
	{
		ClearClicked ();
	}

	public void AdjustCamera()
	{
		Camera.main.orthographicSize = 10 + (m_cameraSlider.value * 15);
	}

	public void AdjustSpeed()
	{
		ChangeSpeed (m_speedSlider.value);
	}

	public void SetPattern(int pattern)
	{
		ShowPattern (pattern);
	}

	public void ShowHideHowToPlay()
	{
		if (m_howToPlay.activeSelf) {
			m_howToPlay.SetActive (false);
		} else {
			m_howToPlay.SetActive (true);
		}
	}
}
