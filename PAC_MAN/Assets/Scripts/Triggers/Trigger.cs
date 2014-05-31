using UnityEngine;
using System;
using System.Collections;

public class Trigger : MonoBehaviour {

	protected virtual void Start(){
		//---- register trigger in engine ceils
		Vector3 pos=transform.position;
		try{
			Engine.instance.ceils[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z)].trigger=this;
		}catch(IndexOutOfRangeException e){
			Debug.LogWarning("Level Design Error! The Trigger is out of level bounds."+e.Message, this);
		}
	}
}
