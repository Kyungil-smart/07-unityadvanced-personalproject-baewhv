using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectManager : SingletonMonoBehaviour<ObjectManager>
{
    public MeleeWeapon BasicMeleeWeapon;
    public RangeWeapon BasicRangeWeapon;

    void Awake()
    {
        var tempObj = Addressables.LoadAssetAsync<GameObject>("Weapon_Guitar").WaitForCompletion();
        BasicMeleeWeapon = GameObject.Instantiate(tempObj, Vector3.zero, Quaternion.identity).GetComponent<MeleeWeapon>();
        tempObj = Addressables.LoadAssetAsync<GameObject>("QueueGun").WaitForCompletion();
        BasicRangeWeapon = GameObject.Instantiate(tempObj, Vector3.zero, Quaternion.identity).GetComponent<RangeWeapon>();
    }
    

}
