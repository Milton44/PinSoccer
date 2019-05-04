using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBall : MonoBehaviour {

    #region
    public GameObject ball;
    public Transform spawnPos;
    #endregion
    public void Spawn()
	{
        
		GameObject tempBall = Instantiate (ball, spawnPos.position, spawnPos.rotation)as GameObject;
	}
}
