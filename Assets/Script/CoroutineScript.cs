using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineScript : MonoBehaviour
{
    void Start()
    {
        //IEnumerable�F�J��Ԃ������ł����i�R���N�V�����j
        IEnumerable ary = new int[] { 1, 2, 3, 4, 5 };
        //IEnumerator�F�����������̂��́i�C�e���[�^�[�j
        IEnumerator e = ary.GetEnumerator();
        //MoveNext()�Ŏ��̗v�f�����o��
        //�����̗v�f������Ȃ�true�B�Ȃ����false
        while (e.MoveNext())
        {
            // Current ���猻�ݎw���Ă���l���擾�ł���
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
