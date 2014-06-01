using UnityEngine;
using Random=UnityEngine.Random;
using System;
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
		//start moving
		SelectRandomWay(this, Engine.GetCeil(transform.position));
	}

	#region Move
	protected virtual void Update(){
		//move	
		float distance=speed*Time.deltaTime;
		if(targetPos!=lastPos){			
			Vector3 moveDir=targetPos-transform.position;
			transform.Translate(moveDir.normalized*distance, Space.World);
		}else
			transform.position=targetPos;
		//check is point node
		if(Vector3.SqrMagnitude(targetPos-transform.position)<(distance*distance)){
			UpdateMoveDirection();
		}
	}

	protected virtual void UpdateMoveDirection(){
		//AI must be here
		AIRandom(this);
	}
	#endregion

	#region AI
	private static void AIRandom(Unit forUnit){
		//AI select radom waypoint on crossroads
		Engine.Ceil myCeil=Engine.GetCeil(forUnit.targetPos);
		if(myCeil.isCross){
			SelectRandomWay(forUnit, myCeil);
		}else{
			ContinueMoving(forUnit, myCeil);
		}

	}
	private static void ContinueMoving(Unit forUnit, Engine.Ceil fromCeil){
		Vector3 dir=forUnit.targetPos-forUnit.lastPos;
		int dx=Math.Sign(dir.x);
		int dz=Math.Sign(dir.z);
		Engine.Ceil targetCeil=Engine.instance.ceils[fromCeil.x+dx, fromCeil.z+dz];
		if(targetCeil.tile==null){
			SetNextWaypoint(forUnit, targetCeil);
			return;
		}
		SetNextWaypoint(forUnit, Engine.instance.ceils[fromCeil.x-dx, fromCeil.z-dz]);
	}
	private static void SelectRandomWay(Unit forUnit, Engine.Ceil fromCeil){
		Engine.Ceil[] neighbors=new Engine.Ceil[4]{
			Engine.instance.ceils[fromCeil.x+1, fromCeil.z],
			Engine.instance.ceils[fromCeil.x, fromCeil.z+1],
			Engine.instance.ceils[fromCeil.x-1, fromCeil.z],
			Engine.instance.ceils[fromCeil.x, fromCeil.z-1],
		};
		int rnd, times=0;
		do{
			rnd=Random.Range(0, 4);
			times++;
		}while(neighbors[rnd].tile!=null && times<10);
		if(times<10){
			SetNextWaypoint(forUnit, neighbors[rnd]);
			return;
		}
		//can't find a random way
		ContinueMoving(forUnit, fromCeil);
	}
	private static void SetNextWaypoint(Unit forUnit, Engine.Ceil nextCeil){
		forUnit.lastPos=forUnit.targetPos;
		forUnit.targetPos=new Vector3(nextCeil.x, 0f, nextCeil.z);
	}
	#endregion
}
