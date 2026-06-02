using System.Collections;
using UnityEngine;

public class ItemProperty : MonoBehaviour
{
    // インスペクターからアイテムごとにIDを設定できるようにする
    [SerializeField] private string itemId;

    // プレイヤーからIDを読み取るための公開プロパティ
    public string ItemId => itemId;
}
