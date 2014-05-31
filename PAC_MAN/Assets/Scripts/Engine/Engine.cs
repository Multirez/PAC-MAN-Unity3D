using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour{
	public static Engine instance;

	public class Ceil{
		public Tile tile=null;
		public Trigger trigger=null;
	}

	private Vector3 inputDirGUI=Vector3.zero;

	public Ceil[,] ceils;
	//inspector vars
	public int levelWidth=10;
	public int levelHeight=10;

	#region Standart Functions
	private void Awake(){
		instance=this;
		//---- initialize ceils
		ceils=new Ceil[levelWidth,levelHeight];
		for(int x=0; x<levelWidth; x++)
			for(int z=0; z<levelHeight; z++)
				ceils[x,z]=new Ceil();
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
}
