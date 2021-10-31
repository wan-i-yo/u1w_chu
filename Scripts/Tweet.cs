using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ツイート機能のサンプル
/// </summary>
public class Tweet : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    GameManager gm = default;
    NCMBTest isRank;

    // 各種パラメーターはインスペクターから設定する
    string goalText = "正中線突きホーダイの結果" + "%0a" + "最大";    // ツイートに挿入するテキスト
    string desitaText = "点でした！押忍！" + "%0a";

    string unityRoomUrl = "https://unityroom.com/games/midline_punch" + "%0a";
    string hashtags = "正中線突きホーダイ,unity1week%0a";        // ツイートに挿入するハッシュタグ

    private void Start() {
        gm = gameManager.GetComponent<GameManager>();
    }

    public void OnClick()
    {
            var url = "https://twitter.com/intent/tweet?";
            url += "text=" + goalText + gm.MaxCombo.ToString() + "連発、" + gm.Score.ToString() + desitaText;
            url +="&url=" + unityRoomUrl + "&hashtags=" + hashtags;

            #if UNITY_EDITOR
                Application.OpenURL ( url );
            #elif UNITY_WEBGL
                // WebGLの場合は、ゲームプレイ画面と同じウィンドウでツイート画面が開かないよう、処理を変える
                Application.ExternalEval(string.Format("window.open('{0}','_blank')", url));
            #else
                Application.OpenURL(url);
            #endif
    }
}