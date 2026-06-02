using UnityEngine;

public static class GamingColor
{
    /// <summary>
    /// 時間に応じて異なる色を返す
    /// <br>Updateなどの毎フレーム更新する関数内で使用すると虹色に変化し続ける</br>
    /// </summary>
    public static Color GetRainbowColor(float changeSpeed)
    {
        // ===============虹色に変化するコード=================
        float hue = (Time.time * changeSpeed) % 1.0f;
        Color rainbowColor = Color.HSVToRGB(hue, 1.0f, 1.0f);
        return rainbowColor;
        //=====================================================
    }
}
