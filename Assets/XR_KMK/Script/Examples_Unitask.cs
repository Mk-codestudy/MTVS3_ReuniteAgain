using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Examples_Unitask : MonoBehaviour
{
    //�����½�ũ ��ɾ� ���� ��ũ��Ʈ
    //���� ���ӿ� �� �� �� ��


    void Start()
    {
        //�����½�ũ �����ϱ�
        HowtoStartUnitask().Forget(); //Q.Forget�� ������? A.����? API�� �� ������ ������
    }

    //�����½�ũ �����ϱ�
    async UniTaskVoid HowtoStartUnitask()
    {
        // �ڷ�ƾ�� yield�� �ʿ��ϵ��� �����½�ũ���� await�� �ʿ��ϴ�.
        await UniTask.Delay(TimeSpan.FromSeconds(10)); //10�ʸ� ��ٸ���.
    }

    //N�� �ڿ� �����ϱ�
    async UniTaskVoid PlayAftersec()
    {
        Debug.Log("�� �������� �ð��� ���� �����ϰ��...");
        await UniTask.Delay(TimeSpan.FromSeconds(1/*N��*/));
        Debug.Log("����ڰ� ������ �ð� ���Ŀ� ���� �ڵ尡 ����˴ϴ�.");
    }

    // TimeScale�� 0(�ð��� �帣�� ���� ��)������ �ð��� ��� �;�!!!
    async UniTaskVoid IgnoreTimeScale()
    {
        Debug.Log("�� �������� �ð��� ���� �����ϰ��...");
        await UniTask.Delay(TimeSpan.FromSeconds(1), DelayType.UnscaledDeltaTime);
        Debug.Log("����ڰ� ������ �ð� ���Ŀ� ���� �ڵ尡 ����˴ϴ�.");
    }

    //Ư�� ������ �������� �� �����ϱ�
    int a = 5;

    async UniTaskVoid PlayAfterCondition()
    {
        Debug.Log("������ �����Ǳ� ������ ��������� ����ǰ��...");
        await UniTask.WaitUntil(()=> a == 5); //������ �����ȴٸ� (int a == 5 ���)
        Debug.Log("�����Ǹ� ���⵵ �̾ ����˴ϴ�.");
    }

}
