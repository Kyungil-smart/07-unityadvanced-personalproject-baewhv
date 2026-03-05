using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectManager : SingletonMonoBehaviour<ObjectManager>
{

    public RangeWeapon BasicRangeWeapon;

    void Awake()
    {
        var tempObj = Addressables.LoadAssetAsync<GameObject>("QueueGun").WaitForCompletion();
        BasicRangeWeapon = GameObject.Instantiate(tempObj, Vector3.zero, Quaternion.identity).GetComponent<RangeWeapon>();
    }
    

}
