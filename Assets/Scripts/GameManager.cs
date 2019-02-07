using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private Transform _RightHandAnchor;

    [SerializeField]
    private Transform _LeftHandAnchor;

    [SerializeField]
    private Transform _CenterEyeAnchor;

    [SerializeField]
    private float _MaxDistance = 100.0f;

    [SerializeField]
    private LineRenderer _LaserPointerRenderer;

    private Transform Pointer
    {
        get
        {
            // 現在アクティブなコントローラーを取得
            var controller = OVRInput.GetActiveController();
            if (controller == OVRInput.Controller.RTrackedRemote)
            {
                return _RightHandAnchor;
            }
            else if (controller == OVRInput.Controller.LTrackedRemote)
            {
                return _LeftHandAnchor;
            }
            // どちらも取れなければ目の間からビームが出る
            return _CenterEyeAnchor;
        }
    }

    public GameObject player;
    public GameObject kagiPrefab;
    public GameObject kaginawaPrefab;
    private GameObject kagiInstance;
    private GameObject kaginawaInstance;

    public Vector3 playerPos;
    public Vector3 kaginawaPos;
    private Vector3 directionPK;
    private float distancePK;



    public float kagiPower = 200f;

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

        var pointer = Pointer;
        if (pointer == null || _LaserPointerRenderer == null)
        {
            return;
        }
        // コントローラー位置からRayを飛ばす
        Ray pointerRay = new Ray(pointer.position, pointer.forward);

        // レーザーの起点
        _LaserPointerRenderer.SetPosition(0, pointerRay.origin);

        RaycastHit hitInfo;
        if (Physics.Raycast(pointerRay, out hitInfo, _MaxDistance))
        {
            // Rayがヒットしたらそこまで
            _LaserPointerRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            // Rayがヒットしなかったら向いている方向にMaxDistance伸ばす
            _LaserPointerRenderer.SetPosition(1, pointerRay.origin + pointerRay.direction * _MaxDistance);
        }



        switch (playerState){
            case PLAYER_STATE.NOMAL:
                if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)){
                    Shot(pointer);
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

    public void Shot(Transform pointer)
    {
        kagiInstance = Instantiate(kagiPrefab, pointer.position, pointer.rotation) as GameObject;
        kagiInstance.GetComponent<Rigidbody>().AddForce(kagiInstance.transform.forward * kagiPower);

    }

}
