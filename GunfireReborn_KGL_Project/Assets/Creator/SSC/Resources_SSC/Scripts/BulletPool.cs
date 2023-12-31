using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    // 오브젝트풀 매니저 싱글턴
    public static BulletPool instance;

    // 오브젝트 풀에 담아둘 프리팹
    public GameObject bulletPrefab;

    // 프리팹을 담아둘 메모리 Stack
    Stack<Bullet> rifleBullet = new Stack<Bullet>();

    // 
    private void Awake()
    {

        instance = this;

        Initialized(10); 
    }

    // 새 프리팹을 생성할 메서드
    private Bullet CreateNewObject()
    {
        var newObj = Instantiate(bulletPrefab, transform).GetComponent<Bullet>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    private void Initialized(int count)
    {
        for(int i = 0; i < count; i++)
        {
            rifleBullet.Push(CreateNewObject());
        }
    }


    // 오브젝트 호출
    public static Bullet GetObject()
    {
        if(instance.rifleBullet.Count > 0)
        {
            var obj = instance.rifleBullet.Pop();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = instance.CreateNewObject();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    // 오브젝트 반환
    public static void ReturnObject(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);

        bullet.transform.SetParent(instance.transform);
        instance.rifleBullet.Push(bullet);
    }

}
