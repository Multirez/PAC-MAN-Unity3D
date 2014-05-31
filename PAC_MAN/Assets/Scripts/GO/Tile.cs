using UnityEngine;
using System;
using System.Collections;

public class Tile : MonoBehaviour {

	protected virtual void Start(){
		//---- register tile in engine ceils
		Vector3 pos=transform.position;
		try{
			Engine.instance.ceils[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z)].tile=this;
		}catch(IndexOutOfRangeException e){
			Debug.LogWarning("Level Design Error! The Tile is out of level bounds. "+e.Message, this);
		}
	}

}
