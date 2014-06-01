using UnityEngine;
using System;
using System.Collections;

public class Engine : MonoBehaviour{
	public static Engine instance;

	#region Ceils
	public class Ceil{
		//position
		private int _x;
		public int x {get{return _x;}}
		private int _z;
		public int z {get{return _z;}}

		public Tile tile=null;
		public Trigger trigger=null;
		public bool isCross=false;
		public bool isFree {get{return tile==null;}}

		public Ceil(int x, int z){
			_x=x;
			_z=z;
		}
	}

	public Ceil[,] ceils;

	public static Ceil GetCeil(Vector3 position){
		return GetCeil(new Vector2(position.x, position.z));
	}
	public static Ceil GetCeil(Vector2 position){
		return instance.ceils[Mathf.RoundToInt(position.x), 
		                      Mathf.RoundToInt(position.y)];
	}

	private void CheckForCrosses(){
		if(ceils==null){
			Debug.LogWarning("Error! You must init ceils first.");
			return;
		}
		bool isHoriz, isVert;
		for(int x=1; x<(levelWidth-1); x++)
			for(int z=1; z<(levelHeight-1); z++)
		{
			isHoriz=ceils[x-1,z].tile==null || ceils[x+1, z].tile==null;
			isVert=ceils[x, z-1].tile==null || ceils[x, z+1].tile==null;
			if(isHoriz && isVert)
				ceils[x, z].isCross=true;
		}
	}	
	private IEnumerator CheckForCrossesAfterFrame(){
		yield return null;
		CheckForCrosses();
	}
	#endregion

	private Vector3 inputDirGUI=Vector3.zero;

	//inspector vars
	public int levelWidth=10;
	public int levelHeight=10;
	public int pacmanLifes=2;

	#region Standart Functions
	private void Awake(){
		instance=this;
		Application.targetFrameRate=Global.instance.targetFramerate;
		//---- initialize ceils
		ceils=new Ceil[levelWidth,levelHeight];
		for(int x=0; x<levelWidth; x++)
			for(int z=0; z<levelHeight; z++)
				ceils[x,z]=new Ceil(x, z);
	}

	private void Start(){
		StartCoroutine("CheckForCrossesAfterFrame");
	}

	private void Update(){
		Vector3 inputDir=Vector3.zero;
		if(Input.anyKey){
			inputDir=new Vector3(Input.GetAxis("Horizontal"),
			                     0f,
			                     Input.GetAxis("Vertical"));
		}
		//try to move player
		inputDir+=inputDirGUI;
		if(inputDir!=Vector3.zero)
			Player.instance.TryMove(inputDir+inputDirGUI);
	}

	private void OnDrawGizmos(){
		Gizmos.color=new Color(0f, 0f, 0.5f, 0.3f);
		Gizmos.DrawCube(new Vector3(levelWidth*0.5f-0.5f, 0f, levelHeight*0.5f-0.5f), 
		                new Vector3(levelWidth, 0.1f, levelHeight));
	}
	#endregion


	public void OnPlayerDeath(){
		if(pacmanLifes>0){
			pacmanLifes--;
			StartCoroutine("RestorePlayerAfterDeath");
		}else
			StartCoroutine("ShowGameOver");
	}
	private IEnumerator RestorePlayerAfterDeath(){
		//gradually slow down time
		while(Time.timeScale>0.3f){
			Time.timeScale*=0.95f;
			yield return new WaitForSeconds(0.05f*Time.timeScale);
		}
		Time.timeScale=1f;
		Player.instance.RestoreAfterDeath();
	}
	private IEnumerator ShowGameOver(){
		//gradually slow down time
		while(Time.timeScale>0.3f){
			Time.timeScale*=0.95f;
			yield return new WaitForSeconds(0.05f*Time.timeScale);
		}
		Application.LoadLevel("LevelScores");
	}
}
