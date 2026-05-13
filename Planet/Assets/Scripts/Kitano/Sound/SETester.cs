using UnityEngine;

public class SETester : MonoBehaviour
{
    void Start()
    {
        Debug.Log("SETester Start");
    }
    void Update()
    {
        // Spaceキー
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("ｽﾍﾟｰｽ押された。");
            AudioManager.instance.PlaySE("Contact");
        }
    }
}