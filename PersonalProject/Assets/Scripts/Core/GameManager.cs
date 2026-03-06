using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private PlayerController _player;


    private void Awake()
    {
        Cursor.visible = false;
        
        if (!_player)
        {
            _player = FindAnyObjectByType<PlayerController>();
            if (!_player)
            {
                var tempObj = Addressables.LoadAssetAsync<GameObject>("Player").WaitForCompletion();
                _player = GameObject.Instantiate(tempObj, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();
            }
        }
        _player.Equipment.SetRangeWeapon(ObjectManager.Instance.BasicRangeWeapon);
        _player.Equipment.SetMeleeWeapon(ObjectManager.Instance.BasicMeleeWeapon);

    }
    //플레이어 초기화
    //각종 매니저 초기화
}
