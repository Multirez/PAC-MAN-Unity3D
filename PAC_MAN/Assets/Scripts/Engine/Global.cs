using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour {
	public static Global instance=InitGlobal();
	private static Global InitGlobal(){
		GameObject result=Resources.Load("Global", typeof(GameObject))as GameObject;
		if(result!=null)
			return result.GetComponent<Global>();
		Debug.LogError("Can't initialize Globals.");
		return null;
	}

	//inspector vars
	public int targetFramerate=100;
	public float pacmanSpeed=1f; 
	public float unitSpeed=0.9f;
}
