using UnityEngine;
using System.Collections;

public class NetworkManagerMock
{
    public static bool isReceived = false;

    static MonoBehaviour runner;

    public static void Init(MonoBehaviour mono)
    {
        runner = mono;

        Debug.Log("NetworkManager 初期化");
    }

    public static void ReceiveData()
    {
        Debug.Log("ReceiveData 開始");

        isReceived = false;

        runner.StartCoroutine(MockReceive());
    }

    static IEnumerator MockReceive()
    {
        Debug.Log("通信待機中");

        yield return new WaitForSeconds(2f);

        isReceived = true;

        Debug.Log("通信完了");
    }
}