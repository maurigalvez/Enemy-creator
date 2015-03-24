using UnityEngine;
using System.Collections;
/// <summary>
/// Author: Mauricio Galvez
/// Date Created: 23/03/15
/// Last Edited: 23/03/15
/// Entity Debug - Component to be used for Entity Debug purposes. Draw's Entities' health bar when visible by game's camera.
/// </summary>
public class EntityHealthDebug : MonoBehaviour
{
	//============
	// INSPECTOR PROPERTIES
	//============
	public float HealthBarWidth;			// Width of Health Bar
	public float HealthBarHeight;			// Height of Health Bar
	[HideInInspector]
	public float MaxDistance = 30;			// Maximum Distance From Camera
	public float MinDistance = 3;			// Minumum Distance From Camera
	public float YDistance = 50;			// Y Distance of HealthBar
	//============
	// PRIVATE PROPERTIES
	//============
	private Transform entTransform;			// Instance of entity's transform
	private bool draw;						// True if entity is visible by cam.
	private bool OutOfCamView;				// True if entity is Out of Camera Bounds
	private Vector3 ScreenPosition;			// ScreenPosition
	private Rect healthBar;					// Health Bar Rectangle
	private EntityData eData;				// Instance of Entity Data
	private Camera MainCamera;				// Instance of MainCamera
	private float DistanceFromCam;			// Current Distance From Camera
	private Transform CameraTransform;		// Instance of Camera Transform
	private GUIStyle HBStyle;				// Instance of Health GUIStyle
	/// =========================
	/// START
	/// <summary>
	/// Start this instance.
	/// </summary>
	/// =========================
	private void Start ()
	{
		Init ();
	}
	/// =========================
	/// ON GUI
	/// <summary>
	/// Raises the GUI event.
	/// </summary>
	/// =========================
	private void OnGUI()
	{
		// check if there's no Entity Data
		if(!eData)
		{
			Debug.LogError ("Assign an Entity Data Component to " + this.gameObject.name);
			return;
		}
		// Check if Debug should  not be drawn
		if(!draw)
			return;
		// Check if Debug is out of cam view
		if(OutOfCamView)
			return;
		// draw health
		DrawHealth();
	}
	/// =========================
	/// UPDATE 
	/// <summary>
	/// Update this instance.
	/// </summary>
	/// =========================
	private void Update()
	{
		UpdateDistanceFromCam();
	}
	/// =========================
	/// UPDATE DISTANCE FROM CAM
	/// <summary>
	/// Updates the distance from cam.
	/// </summary>
	/// =========================
	private void UpdateDistanceFromCam()
	{
		// Obtain Distance from Camera
		DistanceFromCam = (CameraTransform.position - entTransform.position).magnitude;
		//Debug.Log (DistanceFromCam);
		// Check if DistanceFromCam is highr than MaxDistance
		if(DistanceFromCam > MaxDistance || DistanceFromCam < MinDistance)
			OutOfCamView = true;
		else
			OutOfCamView = false;
	}
	/// =========================
	/// ON BECAME VISIBLE
	/// <summary>
	/// Raises the became visible event.
	/// </summary>
	/// =========================
	void OnBecameVisible()
	{
		// Enable drawing of Debug
		draw = true;
	}
	/// =========================
	/// ON BECAME INVISIBLE
	/// <summary>
	/// Raises the became invisible event.
	/// </summary>
	/// =========================
	void OnBecameInvisible()
	{
		// Disable drawing of Debug
		draw = false;
	}
	/// =========================
	/// INIT
	/// <summary>
	/// Initializes properties of EntityDebug
	/// </summary>
	/// =========================
	private void Init()
	{
		// Disable drawing of Debug
		draw = false;
		OutOfCamView = true;
		// obtain Entitie's Transform
		entTransform = this.gameObject.transform;
		// Obtain Instance of MainCamera 
		MainCamera = GameObject.Find ("Main Camera").GetComponent<Camera>();
		// Obtain Camera Transform
		CameraTransform = MainCamera.transform;
		// obtain Entity Data
		eData = this.gameObject.GetComponent<EntityData>();
		// check if Entity Data was not found
		if(!eData)
			Debug.LogError ("Assign an Entity Data Component to " + this.gameObject.name);
		//--------
		// Initialize HBStyle
		//--------

	}
	/// =========================
	/// DRAW HEALTH
	/// <summary>
	/// Draws Entity's Health bar
	/// </summary>
	/// =========================
	private void DrawHealth()
	{	
		// obtain Screen position
		ScreenPosition = MainCamera.WorldToScreenPoint (entTransform.position);
		// Invert Y Axis
		ScreenPosition.y = Screen.height - (ScreenPosition.y + 1);
		// Create Health Rectangle
		healthBar = new Rect(ScreenPosition.x - 50, ScreenPosition.y - YDistance, HealthBarWidth * eData.getHealthRatio(),HealthBarHeight);
		// Draw Health Bar
		GUI.color = Color.red;
		GUI.HorizontalScrollbar(healthBar,0,eData.Health,0,eData.MaxHealthPoints);
		GUI.color = Color.white;

	}
}
