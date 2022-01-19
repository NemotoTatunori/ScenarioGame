using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineScript : MonoBehaviour
{
    void Start()
    {
        //IEnumerable：繰り返し処理できるやつ（コレクション）
        IEnumerable ary = new int[] { 1, 2, 3, 4, 5 };
        //IEnumerator：反復処理そのもの（イテレーター）
        IEnumerator e = ary.GetEnumerator();
        //MoveNext()で次の要素を取り出す
        //続きの要素があるならtrue。なければfalse
        while (e.MoveNext())
        {
            // Current から現在指している値を取得できる
            Debug.Log($"Current={e.Current}");
        }

        StartCoroutine(DoAsync());
    }

    void Update()
    {

    }

    IEnumerator DoAsync()
    {
        while (true)
        {
            yield return enumerator(Vector3.right, 60);
            yield return enumerator(Vector3.up, 60);
            yield return enumerator(Vector3.forward, 60);
        }
    }

    IEnumerator enumerator(Vector3 axis, int frame)
    {
        for (int i = 0; i < 90; i++)
        {
            transform.Rotate(axis);
            yield return new WaitForSeconds(0.01f);

        }
    }
}
