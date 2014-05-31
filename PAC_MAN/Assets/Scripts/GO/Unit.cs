using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour{	
	protected Vector3 lastPos;
	protected Vector3 targetPos;
	protected float speed;

	//inspector vars
	public float speedCoef=1f;//speed coefficient for current unit
	
	protected virtual void Start(){
		Vector3 pos=transform.position;
		transform.position=new Vector3(Mathf.RoundToInt(pos.x), 0f, Mathf.RoundToInt(pos.z));
		//init move
		lastPos=transform.position;
		targetPos=lastPos;
		//calc speed
		speed=Global.instance.unitSpeed*speedCoef;
	}

	#region Move
	protected virtual void Update(){
		//move
		Vector3 moveDir=targetPos-transform.position;
		float distance=speed*Time.deltaTime;
		transform.Translate(moveDir.normalized*distance, Space.World);
		//check is point node
		if(Vector3.SqrMagnitude(targetPos-transform.position)<(distance*distance)){
			UpdateMoveDirection();
		}
	}

	protected virtual void UpdateMoveDirection(){
		//AI must be here
	}
	#endregion
}
