using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject player;
    public GameObject kagiPrefab;
    public GameObject kaginawaPrefab;

    public Vector3 playerPos;
    public Vector3 kaginawaPos;
    public Vector3 directionPK;
    public float distancePK;

    public enum PLAYER_STATE
    {
        NOMAL,
        THROW,
        HIT,
        JOINT,
        STAY,
    }


    private PLAYER_STATE playerState;

	// Use this for initialization
	void Start () {
        playerState = PLAYER_STATE.NOMAL;
	}
	
	// Update is called once per frame
	void Update () {




        switch(playerState){
            case PLAYER_STATE.NOMAL:
                if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)){
                    playerState = PLAYER_STATE.THROW;
                }
                break;
            case PLAYER_STATE.THROW:

                break;
            case PLAYER_STATE.HIT:
                break;
            case PLAYER_STATE.JOINT:
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                {
                    playerState = PLAYER_STATE.THROW;
                }
                break;
            case PLAYER_STATE.STAY:
                break;

        }
  	}


}
