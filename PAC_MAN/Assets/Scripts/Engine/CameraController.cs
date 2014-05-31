using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Vector3 relativePos;//relative character
	public float speed=1;

	void Start () {
		//init camera pos
		transform.position=Player.instance.transform.position+relativePos;
	}

	void Update () {
		Vector3 newPos=Player.instance.transform.position+relativePos;
		transform.position=Vector3.Lerp(transform.position, newPos, Time.deltaTime*speed);
	}
}
