using UnityEngine;
using System;
using System.Collections;

public class Trigger : MonoBehaviour {

	protected virtual void Start(){
		//---- register trigger in engine ceils
		Vector3 pos=transform.position;
		int x=Mathf.RoundToInt(pos.x);
		int z=Mathf.RoundToInt(pos.z);
		transform.position=new Vector3(x, 0f, z);
		try{
			Engine.instance.ceils[x, z].trigger=this;
		}catch(IndexOutOfRangeException e){
			Debug.LogWarning("Level Design Error! The Trigger is out of level bounds."+e.Message, this);
		}
	}
}
