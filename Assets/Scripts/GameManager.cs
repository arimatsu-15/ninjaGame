using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


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

    public GameObject player;     public GameObject kagiPrefab;     public GameObject kaginawaPrefab;     public GameObject kagiInstance;     public GameObject kaginawaInstance;      public GameObject text;      public Vector3 playerPos;
    public Vector3 playerPos1;
    public Vector3 playerPos2;     public Vector3 kaginawaPos;     public Vector3 directionPK;     public float distancePK;     public double hikakuDirectionPK1;     public double hikakuDirectionPK2;      public bool touchKagi;     public bool deleteKagi;      private float kagiPower = 2000f;     private float kaginawaPower = 500f;


    public enum PLAYER_STATE     {         NOMAL,         THROW,         HIT,         JOINT,         STAY,     };

    public PLAYER_STATE playerState;      // Use this for initialization     void Start () {         playerState = PLAYER_STATE.NOMAL;
        touchKagi = false;         deleteKagi = false;      } 


    void Update()
    {
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

        text.GetComponent<Text>().text = "playerState=" + playerState + "" + kaginawaPos;



        if (playerState == PLAYER_STATE.NOMAL)
        {
            touchKagi = false;
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                Shot(pointer);
                playerState = PLAYER_STATE.THROW;
            }
        }

        if (playerState == PLAYER_STATE.THROW)
        {
            touchKagi = kagiInstance.GetComponent<kagiPrefab>().touchKagi;
            deleteKagi = kagiInstance.GetComponent<kagiPrefab>().deleteKagi;
            if (touchKagi == true)
            {
                kagiInstance.GetComponent<kagiPrefab>().touchKagi = false;
                playerState = PLAYER_STATE.HIT;
                kaginawaPos = kagiInstance.transform.position;
            }
            if (deleteKagi == true)
            {
                kagiInstance.GetComponent<kagiPrefab>().deleteKagi = false;
                playerState = PLAYER_STATE.NOMAL;
                Destroy(kagiInstance);
            }
        }

        if(playerState == PLAYER_STATE.HIT){
            touchKagi = false;
            playerPos1 = player.transform.position;
            hikakuDirectionPK1 = Vector3.Distance(playerPos1, kaginawaPos);
            playerPos2 = player.transform.position;
            hikakuDirectionPK2 = Vector3.Distance(playerPos2, kaginawaPos);



            if (hikakuDirectionPK1 > 0)
            {
                Destroy(kagiInstance);
                kaginawaInstance = Instantiate(kaginawaPrefab, kaginawaPos, new Quaternion(0, 0, 0, 0)) as GameObject;
                kaginawaInstance.GetComponent<CharacterJoint>().connectedBody = player.GetComponent<Rigidbody>();
                playerState = PLAYER_STATE.JOINT;
            } 
        }

        if(playerState == PLAYER_STATE.JOINT){
            playerPos = player.transform.position;
            distancePK = Vector3.Distance(playerPos, kaginawaPos);
            directionPK = (kaginawaPos - playerPos) / distancePK;           //単位方向ベクトル                 if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
            {
                Destroy(kaginawaInstance);
                playerState = PLAYER_STATE.NOMAL;
            }
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                Destroy(kaginawaInstance);
                player.GetComponent<Rigidbody>().AddForce(directionPK * kaginawaPower);
                playerState = PLAYER_STATE.HIT;
            } 
        }



    }

    public void Shot(Transform pointer)     {         kagiInstance = Instantiate(kagiPrefab, pointer.position, pointer.rotation) as GameObject;         kagiInstance.GetComponent<Rigidbody>().AddForce(kagiInstance.transform.forward * kagiPower);     } 
}