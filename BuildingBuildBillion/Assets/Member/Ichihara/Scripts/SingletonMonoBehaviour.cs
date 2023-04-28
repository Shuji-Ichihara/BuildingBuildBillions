using System;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                Type t = typeof(T);
                _instance = (T)FindObjectOfType(t);
                if(_instance == null)
                {
                    Debug.LogError(t + "�������������Ă��� GameObject �͂���܂���B");
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // ���� GameObject �ɃA�^�b�`����Ă��邩���ׂ�B
        // �A�^�b�`����Ă���ꍇ�͔j������B
        if(this != Instance)
        {
            Destroy(this);
            Debug.LogError(
                typeof(T)
                + "�͊��ɑ��� GameObject �ɃA�^�b�`����Ă��邽�߁A�R���|�[�l���g��j�����܂����B"
                + "�A�^�b�`����Ă��� GameObject ��" + Instance.gameObject.name + "�ł��B");
            return;
        }
    }
}
