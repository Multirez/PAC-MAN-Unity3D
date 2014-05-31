using UnityEngine;
using System;
using System.Collections;

public class Tile : MonoBehaviour {

	protected virtual void Start(){
		//---- register tile in engine ceils
		Vector3 pos=transform.position;
		int x=Mathf.RoundToInt(pos.x);
		int z=Mathf.RoundToInt(pos.z);
		transform.position=new Vector3(x, 0f, z);
		try{
			Engine.instance.ceils[x, z].tile=this;
		}catch(IndexOutOfRangeException e){
			Debug.LogWarning("Level Design Error! The Tile is out of level bounds. "+e.Message, this);
		}
	}

}
