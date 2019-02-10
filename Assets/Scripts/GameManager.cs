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

    private Transform Pointer //いじった、webみて戻せる
    {
        get
        {
            // 現在アクティブなコントローラーを取得

                return _RightHandAnchor;

        }
    }

    public GameObject player;     public GameObject kagiPrefab;     public GameObject kaginawaPrefab;
    public GameObject kagiApproachPrefab;     public GameObject kagiInstance;     public GameObject kaginawaInstance;
    public GameObject kagiApproachInstance;
     public GameObject text;      public Vector3 playerPos;
    public Vector3 playerPos1;
    public Vector3 playerPos2;     public Vector3 kaginawaPos;     public Vector3 directionPK;     public float distancePK;     public double hikakuDirectionPK1;     public double hikakuDirectionPK2;
    public double hikaku;      public bool touchKagi;     public bool deleteKagi;      private float kagiPower = 6000f;     private float kaginawaPower = 50f;


    public enum PLAYER_STATE     {         NOMAL,         THROW,         HIT,         JOINT,         STAY,     };

    public PLAYER_STATE playerState;      // Use this for initialization     void Start () {         playerState = PLAYER_STATE.NOMAL;
        touchKagi = false;         deleteKagi = false;      } 


    void Update()
    {
        var pointer = Pointer;





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
            _LaserPointerRenderer.SetPosition(0, pointer.position);
            _LaserPointerRenderer.SetPosition(1, kaginawaPos);
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                Destroy(kagiInstance);
                Shot(pointer);
            }
            if (touchKagi == true)
            {
                kagiInstance.GetComponent<kagiPrefab>().touchKagi = false;
                kaginawaPos = kagiInstance.transform.position;
                kagiApproachInstance = Instantiate(kagiApproachPrefab, kaginawaPos, new Quaternion(0, 0, 0, 0)) as GameObject;
                playerState = PLAYER_STATE.HIT;
            }
            if (deleteKagi == true)
            {
                kagiInstance.GetComponent<kagiPrefab>().deleteKagi = false;
                Destroy(kagiInstance);
                playerState = PLAYER_STATE.NOMAL;
            }
        }

        if(playerState == PLAYER_STATE.HIT){
            touchKagi = false;
            _LaserPointerRenderer.SetPosition(0, pointer.position);
            _LaserPointerRenderer.SetPosition(1, kaginawaPos);
            playerPos1 = player.transform.position;
            hikakuDirectionPK1 = Vector3.Distance(playerPos1, kaginawaPos);
            if (hikakuDirectionPK1 > hikakuDirectionPK2)
            {
                Destroy(kagiApproachInstance);
                Destroy(kagiInstance);
                kaginawaInstance = Instantiate(kaginawaPrefab, kaginawaPos, new Quaternion(0, 0, 0, 0)) as GameObject;
                kaginawaInstance.GetComponent<CharacterJoint>().connectedBody = player.GetComponent<Rigidbody>();
                playerState = PLAYER_STATE.JOINT;
            }
            playerPos = player.transform.position;
            distancePK = Vector3.Distance(playerPos, kaginawaPos);
            directionPK = (kaginawaPos - playerPos) / distancePK;//単位方向ベクトル

            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                Destroy(kagiInstance);
                player.GetComponent<Rigidbody>().AddForce(directionPK * kaginawaPower);
            }
            if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
            {
                Destroy(kagiApproachInstance);
                Destroy(kagiInstance);
                playerState = PLAYER_STATE.NOMAL;
            } 
        }

        if(playerState == PLAYER_STATE.JOINT){
            _LaserPointerRenderer.SetPosition(0, pointer.position);
            _LaserPointerRenderer.SetPosition(1, kaginawaPos);
            playerPos = player.transform.position;
            distancePK = Vector3.Distance(playerPos, kaginawaPos);
            directionPK = (kaginawaPos - playerPos) / distancePK;           //単位方向ベクトル
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                Destroy(kaginawaInstance);
                player.GetComponent<Rigidbody>().AddForce(directionPK * kaginawaPower);
                kagiApproachInstance = Instantiate(kagiApproachPrefab, kaginawaPos, new Quaternion(0, 0, 0, 0)) as GameObject;
                playerState = PLAYER_STATE.HIT;
            }
            if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
            {
                Destroy(kaginawaInstance);
                playerState = PLAYER_STATE.NOMAL;
            } 
        }

    }

    private void LateUpdate()
    {
        playerPos2 = player.transform.position;
        hikakuDirectionPK2 = Vector3.Distance(playerPos2, kaginawaPos);
        hikaku = hikakuDirectionPK1 - hikakuDirectionPK2;
    }

    public void Shot(Transform pointer)     {         kagiInstance = Instantiate(kagiPrefab, pointer.position, pointer.rotation) as GameObject;         kagiInstance.GetComponent<Rigidbody>().AddForce(kagiInstance.transform.forward * kagiPower);     } 
}