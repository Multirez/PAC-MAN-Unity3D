using UnityEngine;
using System;
using System.Collections;

public class Player : Unit{
	public static Player instance;

	private Vector3 targetDir;//result direction from inputs
	private Vector3 startPos;

	private void Awake(){
		instance=this;
	}
	protected override void Start(){
		base.Start();
		startPos=lastPos;//save start position for restore after death
		targetDir=Vector3.zero;
		targetPos=lastPos;
		speed=Global.instance.pacmanSpeed*speedCoef;
	}

	#region Move
	public void TryMove(Vector3 inputDir){
		Vector3 moveDir=targetPos-lastPos;
		float angle=Vector3.Angle(moveDir, inputDir);
		if(angle==0)//do nothing
			return;
		if(angle==180){//change direction now
			moveDir=lastPos;
			lastPos=targetPos;
			targetPos=moveDir;
		}
		//sets new value for target direction
		targetDir=inputDir;
	}

	protected override void UpdateMoveDirection(){
		//we are at node point and can to change move direction
		//current ceil
		Vector3 pos=transform.position;
		pos=new Vector3(Mathf.Round(pos.x), 0f, Mathf.Round(pos.z));
		Engine.Ceil currentCeil=Engine.instance.ceils[(int)pos.x, (int)pos.z];
		//------------
		//---- trigers must be activated here
		//-----------

		//target ceil
		Vector3 newTargetPos=pos+GetRoundedDir(targetDir);
		Engine.Ceil targetCeil=Engine.instance.ceils[(int)newTargetPos.x, (int)newTargetPos.z];
		//check is can change direction
		if(targetCeil.tile==null){//the way is free
			targetPos=newTargetPos;
			lastPos=pos;
			return;
		}

		//check is can continue to move
		if(targetPos!=lastPos){
			newTargetPos=pos+(targetPos-lastPos);
			targetCeil=Engine.instance.ceils[(int)newTargetPos.x, (int)newTargetPos.z];
			if(targetCeil.tile==null){//the way is free
				targetPos=newTargetPos;
				lastPos=pos;
				return;
			}
		}

		//no way - stay here
		lastPos=targetPos;
	}	
	
	private Vector3 GetRoundedDir(Vector3 direction){
		if(Mathf.Abs(direction.x)<Mathf.Abs(direction.z))
			return new Vector3(0f, 0f, Math.Sign(direction.z));
		else
			return new Vector3(Math.Sign(direction.x), 0f, 0f);
	}
	#endregion

	#region Attack
	void OnCollisionEnter(Collision collision){
		print("New collision detect!");
		Unit unit=collision.gameObject.GetComponent<Unit>();
		if(unit){
			if(unit.power<this.power){
				EatUnit(unit);
			}else{
				unit.EatUnit(this);
			}
		}
	}
	protected override void EatMe(){
		//show all animations but don't remove from arrays
		collider.enabled=false;
		Engine.instance.OnPlayerDeath();
	}	
	public void RestoreAfterDeath(){
		transform.position=startPos;
		collider.enabled=true;
		lastPos=startPos;
		targetPos=startPos;
		targetDir=Vector3.zero;
		speed=Global.instance.pacmanSpeed*speedCoef;
	}
	#endregion
}
