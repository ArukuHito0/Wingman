using UnityEngine;

public static class NetworkManagerMock
{
    public static bool isReceived = false;

    public static void ReceiveData()
    {
        // 仮：2秒後に受信完了
        isReceived = false;
        UnityEngine.Debug.Log("受信開始");

        Instance.StartCoroutine(MockReceive());
    }

    static MonoBehaviour Instance;

    public static void Init(MonoBehaviour mono)
    {
        Instance = mono;
    }

    static System.Collections.IEnumerator MockReceive()
    {
        yield return new WaitForSeconds(2f);
        isReceived = true;
        Debug.Log("受信完了");
    }
}