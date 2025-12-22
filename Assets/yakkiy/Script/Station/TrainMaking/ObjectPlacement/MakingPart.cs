using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MakingPart : MonoBehaviour
{
    const string ACTIVE_SCENE_NAME = "Station";

    //[SerializeField] PartProperty partProperty;
    //public PartProperty PartProperty { get { return partProperty; } }

    [SerializeField] GameObject colliderObj;

    [SerializeField] PartsFamily partsFamily;//メイキング用親子関係もどき
    public PartsFamily PartsFamily { get { return partsFamily; } }

    public GameObject ColliderObj{  get { return colliderObj; } }

    [SerializeField] MeshRenderer[] meshRenderers;
    public MeshRenderer[] MeshRenderers {  get { return meshRenderers; } }

    Material[] materials;

    public bool isPickup;

    public bool beStructure;//列車の構造に組み込まれている

    bool isBuried;//めり込んでいる

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != ACTIVE_SCENE_NAME) Destroy(this);//駅でなければこのスクリプトは削除する

        partsFamily = GetComponent<PartsFamily>();

        GetMaterial();
    }

    void GetMaterial()
    {
        materials = new Material[meshRenderers.Length];

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            materials[i] = meshRenderers[i].material;
        }
    }

    private void Update()
    {
        BeStructureCheck();

        CorrectPlacement();
    }

    void BeStructureCheck()
    {
        beStructure = partsFamily.Parent == null ? false : true;
    }

    public void ReturnTheMaterial()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = materials[i];
        }
    }

    void CorrectPlacement()//配置が正しいかのチェック
    {
        if (isPickup) return;//ピックアップ中は考えない

        if (beStructure) return;//列車に組み込まれていたら正しい

        //if(isBuried) return;//めり込んでいたら正しくない

        MaterialFill(PartsOperation.Instance.CannotBePlacedMaterial);
    }

    

    public void ReceiveOnCollisionStay(Collision collision)
    {
        isBuried = true;
    }

    public void ReceiveOnCollisionExit(Collision collision)
    {
        isBuried = false;
    }

    void MaterialFill(Material material)
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = material;
        }
    }
}
