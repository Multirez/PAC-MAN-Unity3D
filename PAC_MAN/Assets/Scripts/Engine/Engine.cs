using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour{
	private class Ceil{
		public Tile tile=null;
		public Trigger trigger=null;
	}

	private Ceil[,] ceils;

	public int levelWidth=10;
	public int levelHeight=10;

	private void Awake(){
		//---- initialize ceils
		ceils=new Ceil[levelWidth,levelHeight];
		ceils.Initialize();
	}



	private void OnDrawGizmos(){
		Gizmos.color=new Color(0f, 0f, 0.5f, 0.3f);
		Gizmos.DrawCube(new Vector3(levelWidth*0.5f-0.5f, 0f, levelHeight*0.5f-0.5f), 
		                new Vector3(levelWidth, 0.1f, levelHeight));
	}
}
