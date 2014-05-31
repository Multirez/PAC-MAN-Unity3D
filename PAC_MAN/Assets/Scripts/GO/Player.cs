using UnityEngine;
using System.Collections;

public class Player : Unit{
	public static Player instance;
	
	private Vector3 targetDir;//result direction from inputs

	private void Awake(){
		instance=this;
	}
	protected override void Start(){
		base.Start ();		
		targetDir=Vector3.zero;
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
			print("New direction approved "+GetRoundedDir(targetDir));
			return;
		}

		//check is can continue to move
		newTargetPos=pos+(targetPos-lastPos);
		targetCeil=Engine.instance.ceils[(int)newTargetPos.x, (int)newTargetPos.z];
		if(targetCeil.tile==null){//the way is free
			targetPos=newTargetPos;
			lastPos=pos;
			print("Old direction approved");
			return;
		}

		//no way - stay here
		lastPos=targetPos;
	}	
	
	private Vector3 GetRoundedDir(Vector3 direction){
		print(direction);
		if(Mathf.Abs(direction.x)<Mathf.Abs(direction.z))
			return new Vector3(0f, 0f, Mathf.Sign(direction.z));
		else
			return new Vector3(Mathf.Sign(direction.x), 0f, 0f);
	}
	#endregion
}
