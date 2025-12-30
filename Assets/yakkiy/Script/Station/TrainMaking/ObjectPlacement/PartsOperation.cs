using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class PartsOperation : MonoBehaviour
{
    public static PartsOperation Instance;

    [SerializeField] PartsFamily partsRoot; //パーツ用親子関係もどきの最親

    [Header("セレクトパーツレイヤー")]
    [SerializeField] LayerMask ignoreSelectedParts;
    [Header("パーツを接続できるレイヤー")]
    [SerializeField] LayerMask placeableLayers;     

    [Header("パーツの状態を表すマテリアル")]
    [SerializeField] Material pickupMaterial;         //パーツ選択時のマテリアル
    [SerializeField] Material cannotBePlacedMaterial; //パーツが正しくない位置に設置された場合のマテリアル

    [Header("正しくない位置に配置されたパーツ")]
    public List<MakingPart> unregisteredParts;  
    public Material CannotBePlacedMaterial { get { return cannotBePlacedMaterial; } }

    bool isPartsMove;//パーツを移動

    bool leftClick;//左クリック入力

    bool isOperation = true;//操作可能

    Vector2 mousePosition;//マウス位置

    MakingPart selectPart;//選択されたパーツ

    /// <summary>
    /// 左クリック押す
    /// </summary>
    void OnLeftClickDown()
    {
        if (!isOperation) return;//操作不可時は入力を受け付けない

        leftClick = true;

        ClickRay();
    }

    /// <summary>
    /// 左クリック離す
    /// </summary>
    void OnLeftClickUp()
    {
        if (!isOperation) return;//操作不可時は入力を受け付けない

        leftClick = false;

        Deselect();

    }

    /// <summary>
    /// マウス位置トラッキング
    /// </summary>
    /// <param name="inputValue"></param>
    void OnMousePosition(InputValue inputValue)
    {
        mousePosition = inputValue.Get<Vector2>();
    }

    private void Awake()
    {
       Singleton();
    }

    private void Start()
    {
        TrainReadout_Making();
    }

    private void Update()
    {
        PartsMove();
    }

    /// <summary>
    /// 操作可能状態切り替え
    /// </summary>
    public void OperationPossibleChange(bool active)
    {
        isOperation = active;

        if(!isOperation)//操作不可にする場合現在操作ちゅうのパーツを離す
        {
            Deselect();
        }
    }

    /// <summary>
    /// 選択を解除する
    /// </summary>
    void Deselect()
    {
        isPartsMove = false;

        if (selectPart != null)//パーツlayerに戻す
        {
            for (int i = 0; i < selectPart.ColliderObjs.Length; i++)
            {
                selectPart.ColliderObjs[i].layer = 8;
            }
        }
    }

    /// <summary>
    /// データをもとに列車を出現させる
    /// </summary>
    [ContextMenu("設計図をロード")]
    void TrainReadout_Making()
    {
        Readout_Making(Train.Instance.parameter.myTrain.bogie , partsRoot);//階層を全て調べてメイキングプレハブを生成する
    }

    /// <summary>
    /// 階層を全て調べてメイキングプレハブを生成する
    /// </summary>
    /// <param name="childPart">今回階層に配置するパーツのデータ</param>
    /// <param name="parent">一つ前の階のPartsFamily</param>
    void Readout_Making(List<PartObject> childPart , PartsFamily parent)
    {
        for (int i = 0; i < childPart.Count; i++)
        {
            //パーツデータをもとにパーツを生成
            GameObject obj = Instantiate(childPart[i].partProperty.makingPrefab, childPart[i].pos, childPart[i].rot);

            PartsFamily partsFamily;

            if (obj.TryGetComponent<MakingPart>(out MakingPart makingPart))
            {
                makingPart.beStructure = true;

            }

            obj.TryGetComponent<PartsFamily>(out partsFamily);

            partsFamily.SetParent(parent);

            //配置したパーツデータにさらに下の階層が存在したらReadout_Makingを追加で呼び出す
            if (childPart[i].childPart.Count > 0) Readout_Making(childPart[i].childPart , partsFamily);
        }
    }

    /// <summary>
    /// クリックした場所を調べる
    /// </summary>
    void ClickRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);    // カメラからマウスカーソルの位置のRayを作成
        RaycastHit hit;
        // Rayを飛ばして当たり判定をチェック
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log($"レイヒット:{hit.collider.gameObject.name}");

            PartsHit(hit);
        }
        else
        {
            PartsUnSelect();
        }
    }

    void PartsHit(RaycastHit hit)
    {
        if (hit.collider.gameObject.layer != 8)
        {
            //ヒットしたオブジェクトが列車のパーツでなければ選択を解除してリターン
            PartsUnSelect();
            return;
        }

        MakingPart hitParts = MakingPartSearch(hit.collider.gameObject.transform);

        if (selectPart == hitParts)
        {
            isPartsMove = true;
            RemovePart(hitParts.PartsFamily);
        }
        else
        {
            PartsUnSelect();

            PartsSelect(hitParts);
        }
    }

    /// <summary>
    /// 列車構造から指定したパーツを外す
    /// </summary>
    void RemovePart(PartsFamily partsFamily)
    {
        partsFamily.RemoveChildren();
    }

    /// <summary>
    /// ヒットしたオブジェクトのMakingPartコンポーネントを探す
    /// </summary>
    /// <param name="hitTransform"></param>
    /// <returns></returns>
    MakingPart MakingPartSearch(Transform hitTransform)
    {
        MakingPart part = null;

        if (hitTransform.gameObject.TryGetComponent<MakingPart>(out part)) return part;//ヒットしたオブジェクトにMakingPartコンポーネントがあれば即return

        //ヒットしたオブジェクトの親を順番に調べる
        for (Transform investigate = hitTransform; investigate.parent != null; investigate = investigate.parent)
        {
            if (investigate.parent.gameObject.TryGetComponent<MakingPart>(out part)) return part;
        }

        return part;
    }

    void PartsMove()
    {
        if (!isPartsMove || selectPart == null) return;
        //Debug.Log("パーツ移動中");

        //if (selectPart.ColliderObj.transform.childCount != 0) ChildrenMassacre(selectPart.ColliderObj.transform);//選択されたパーツに接続されたパーツがあれば解除する

        //selectPart.PartCollider.isTrigger = true;
        //セレクトパーツlayerに変更
        for (int i = 0; i < selectPart.ColliderObjs.Length; i++)
        {
            selectPart.ColliderObjs[i].layer = 9;
        }

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);    // カメラからマウスカーソルの位置のRayを作成
        RaycastHit hit;
        // Rayを飛ばして当たり判定をチェック
        if (Physics.Raycast(ray, out hit , float.MaxValue , ignoreSelectedParts))
        {
            if (((1 << hit.collider.gameObject.layer) & placeableLayers) != 0)
            {
                PartsFamily parent = GetPartsFamily(hit.collider.gameObject);

                selectPart.PartsFamily.SetParent(parent);

            }
            else
            {
                if(selectPart.PartsFamily.Parent != null) selectPart.PartsFamily.RemoveParent();
            }


            selectPart.transform.position =  hit.point;

            selectPart.transform.rotation = Quaternion.LookRotation(hit.normal);
        }
    }

    /// <summary>
    /// 対象の親を順番に調べてPartsFamilyを探す
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    PartsFamily GetPartsFamily(GameObject obj)
    {
        for (Transform what = obj.transform; what.parent != null; what = what.parent)
        {
            if(what.parent.gameObject.TryGetComponent<PartsFamily>(out PartsFamily parent))
            {
                return parent;
            }
        }

        return null;
    }

    /// <summary>
    /// パーツを選択
    /// </summary>
    /// <param name="hitMakingPart"></param>
    void PartsSelect(MakingPart hitMakingPart)
    {
        selectPart = hitMakingPart;

        selectPart.isPickup = true;//選択したパーツのピックアップフラグを立てる

        MaterialFill(selectPart.MeshRenderers, pickupMaterial);
    }

    /// <summary>
    /// 選択されたパーツがあったっ場合いじったパラメータをリセットして選択解除する
    /// </summary>
    void PartsUnSelect()
    {
        if (selectPart == null) return;

        selectPart.isPickup = false;//選択したパーツのピックアップフラグをおろす

        selectPart.ReturnTheMaterial();

        //selectPart.PartCollider.isTrigger = false;

        selectPart = null;
    }

    void MaterialFill(MeshRenderer[] meshRenderers ,Material material)
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = material;
        }
    }

    void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// myTrainに反映させる
    /// </summary>
    [ContextMenu("設計図をセーブ")]
   
    public void Completion()
    {
        if(unregisteredParts.Count > 0)//正しくパーツが配置されていなければセーブできない
        {
            SystemMessage.Instance.ErrorMessage("配置されていないパーツがあります");

            return;
        }


        List<PartObject> trainObject = new List<PartObject>();

        Registration(partsRoot.Child,ref trainObject);

        Train.Instance.parameter.myTrain.bogie = trainObject;

        void Registration(List<PartsFamily> partFamily , ref List<PartObject> partObjects)
        {
            for (int i = 0; i < partFamily.Count; i++)
            {
                PartObject partObject = new PartObject();
                partObject.partProperty = partFamily[i].PartProperty;
                partObject.pos = partFamily[i].transform.position;
                partObject.rot = partFamily[i].transform.rotation;

                partObjects.Add(partObject);
                
                if (partFamily.Count > 0) Registration(partFamily[i].Child , ref partObject.childPart);
            }
        }


        //ステージに移動する
        SceneManager.LoadScene("Stage");
    }
}


