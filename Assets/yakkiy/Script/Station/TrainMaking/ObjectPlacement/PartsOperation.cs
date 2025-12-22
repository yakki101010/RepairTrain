using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class PartsOperation : MonoBehaviour
{
    public static PartsOperation Instance;

    [SerializeField] PartsFamily partsRoot;//パーツ用親子関係もどきの最親

    [SerializeField] LayerMask ignoreSelectedParts;
    [SerializeField] LayerMask placeableLayers;//パーツを接続できるレイヤー

    [SerializeField] Material pickupMaterial;
    [SerializeField] Material cannotBePlacedMaterial;
    public Material CannotBePlacedMaterial { get { return cannotBePlacedMaterial; } }

    bool isPartsMove;//パーツを移動

    bool leftClick;//左クリック入力
    Vector2 mousePosition;//マウス位置

    MakingPart selectPart;//選択されたパーツ
    void OnLeftClickDown()
    {
        leftClick = true;

        ClickRay();
    }

    void OnLeftClickUp()
    {
        leftClick = false;

        isPartsMove = false;

        if(selectPart != null) selectPart.ColliderObj.layer = 8;//パーツlayerに戻す
    }

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

    void TrainReadout_Making()//データをもとに列車を出現させる
    {
        Readout_Making(Train.Instance.parameter.myTrain.bogie , partsRoot);//階層を全て調べてメイキングプレハブを生成する
    }

    void Readout_Making(List<PartObject> childPart , PartsFamily parent)//階層を全て調べてメイキングプレハブを生成する
    {
        for (int i = 0; i < childPart.Count; i++)
        {
            GameObject obj = Instantiate(childPart[i].partProperty.makingPrefab, childPart[i].pos, childPart[i].rot);

            PartsFamily partsFamily;

            if (obj.TryGetComponent<MakingPart>(out MakingPart makingPart))
            {
                makingPart.beStructure = true;

            }

            obj.TryGetComponent<PartsFamily>(out partsFamily);

            partsFamily.SetParent(parent);

            if (childPart[i].ChildPart.Count > 0) Readout_Making(childPart[i].ChildPart , partsFamily);
        }
    }

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


    MakingPart MakingPartSearch(Transform hitTransform)//ヒットしたオブジェクトのMakingPartコンポーネントを探す
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
        selectPart.ColliderObj.layer = 9;//セレクトパーツlayerに変更

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

}


