using UnityEngine;
using System.Collections;

public class Player : Unit{
	public static Player instance;

	void Awake(){
		instance=this;
	}
	

}
