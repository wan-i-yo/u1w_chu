using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using HC.Debug;
using System.Linq;　

public class GameManager : MonoBehaviour
{
    public int GameState { get; set; }  // ゲームの状態

    // カメラの位置制御用
    [SerializeField] private GameObject mainCamera = default;
    private Transform cameraTrans = default;
    [SerializeField] private float moveCameraY = default;


    public int InputArrowKey { get; private set; }
    BoxCollider2D bc = default;
    private bool cameraMoving = default;

    // コマンド群
    [SerializeField] private GameObject Arrows = default;
    [SerializeField] private List<GameObject> ArrowsPrefabs = default;

    // 当たり判定表示時間
    [SerializeField] private float colliderDispTime = default;

    // 表示タイマー
    [SerializeField] private GameObject timerObj = default;
    Timer timer = default;

    // タイムアップ時の足の画像
    [SerializeField] private List<GameObject> Result1Image = default;

    // 結果表示画面の相手
    [SerializeField] private List<GameObject> ResultEnemy = default;

    // スコア
    public int Score { get; set; }

    // コンボ
    public int Combo { get; set; }
    public int MaxCombo { get; set; }

    private bool stateResult1 = default;

    [SerializeField] private GameObject titleObj = default;
    [SerializeField] private GameObject startArrow = default;

    // 手
    [SerializeField] private List<Sprite> fists = default;
    [SerializeField] private GameObject leftHandObj = default;
    [SerializeField] private GameObject rightHandObj = default;
    private Transform leftHand = default;
    private Transform rightHand = default;
    [SerializeField] private SpriteRenderer leftSP = default;
    [SerializeField] private SpriteRenderer rightSP = default;

    // サウンド
    [SerializeField] private AudioSource _audioSource = default;
    [SerializeField] private List<AudioClip> punchSE = default;
    [SerializeField] private List<AudioClip> missSE = default;

    [SerializeField] private bool noInput = default;
    [SerializeField] private float noInputInterval = default;

    [SerializeField] private GameObject ResultPlayer = default;

    [SerializeField] private GameObject sikiri = default;

    [SerializeField] private AudioClip hajimeSE = default;

    [SerializeField] private GameObject scoreObj = default;

    void Start()
    {
        Debug.unityLogger.logEnabled = false;
        InputArrowKey = 0;
        cameraTrans = mainCamera.GetComponent<Transform>();
        GameState = ConstData.GameState.title;
        print("GameManager.cs State:"+GameState);
        bc = GetComponent<BoxCollider2D>();
        timer = timerObj.GetComponent<Timer>();
        timerObj.SetActive(false);
        Score = 0;
        cameraMoving = false;
        stateResult1 = false;
        Combo = 0;
        titleObj.SetActive(true);
        startArrow.SetActive(true);
        leftHand = leftHandObj.GetComponent<Transform>();
        rightHand = rightHandObj.GetComponent<Transform>();
        leftSP = leftHandObj.GetComponent<SpriteRenderer>();
        rightSP = rightHandObj.GetComponent<SpriteRenderer>();
        ResultPlayer.SetActive(false);
        scoreObj.SetActive(false);
    }

    void Update()
    {
        switch(GameState)
        {
            case ConstData.GameState.title:
                // if (InputKey() == 2) {
                if (Input.GetKeyDown(KeyCode.DownArrow)) {
                    DownCameraAndCollision(moveCameraY + 0.7f);
                    print("GameManager.cs State:"+GameState);
                    titleObj.SetActive(false);
                    startArrow.SetActive(false);
                    Score += 100;
                    GameState = ConstData.GameState.stage1;
                    timerObj.SetActive(true);
                    _audioSource.PlayOneShot(hajimeSE, 1.0f);
                    scoreObj.SetActive(true);
                }
                break;
            case ConstData.GameState.stage1:
                transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
                if (timer.GameTime < 0 ) {
                    Result1Image[0].SetActive(true);
                    Result1Image[1].SetActive(true);
                    Result1Image[0].transform.SetParent(Arrows.transform);
                    Result1Image[1].transform.SetParent(Arrows.transform);
                    // Result1Image[0].transform.parent = Arrows.transform;
                    // Result1Image[1].transform.parent = Arrows.transform;
                    GameState = ConstData.GameState.result1;
                }
                break;
            case ConstData.GameState.result1:
                if (!stateResult1) {
                    leftHandObj.SetActive(false);
                    rightHandObj.SetActive(false);
                    DOVirtual.DelayedCall(
                        1.0f,   // 遅延させる（待機する）時間
                        () => {
                            Transform[] tr = Arrows.transform.GetComponentsInChildren<Transform>();
                            var query = tr.Where(p => p.gameObject.name.Contains("arrow"));
                            foreach (Transform item in query) {
                                item.gameObject.SetActive(false);
                            }

                            // 長い相手表示
                            Vector3 offset = Result1Image[0].transform.position;
                            offset.x += 17.5f;
                            GameObject a = default;
                            if (Score > 12000) {
                                a = ResultEnemy[0];
                                offset.y -= 0.1f;
                            } else if (Score > 6000) {
                                a = ResultEnemy[1];
                                offset.y -= 0.8f;
                            } else {
                                a = ResultEnemy[2];
                                offset.y -= 1.4f;
                            }
                            a.SetActive(true);
                            a.transform.position = offset;
                            offset.x = 10.0f;
                            offset.y += 0.5f;
                            sikiri.transform.position = offset;

                            //カメラ縦移動後、横移動
                            Vector3 temping = Result1Image[0].transform.position;
                            cameraTrans.DOLocalMove (
                                new Vector3(cameraTrans.position.x, temping.y, cameraTrans.position.z),　　//移動後の座標
                                2.0f 　　　　　　//時間
                            ).SetEase(Ease.InOutQuad)
                            .OnComplete(() => {
                                //スコアによってResultEnemyを生成
                                cameraTrans.DOLocalMove (
                                    new Vector3(temping.x+21.0f, cameraTrans.position.y, cameraTrans.position.z),　　//移動後の座標
                                    1.0f 　　　　　　//時間
                                ).SetEase(Ease.InOutQuad);
                            });
                        }

                    // 何mか表示
                    // 「長くね？」みたいなコメント表示
                    // リトライ、ランキング、ツイートボタン表示
                    )
                    .OnComplete(
                        () => {
                            DOVirtual.DelayedCall (
                                4.0f,
                                () =>
                                ResultPlayer.SetActive(true)
                            );
                        }
                    );

                    GameState = ConstData.GameState.result1;
                    stateResult1 = true;
                }

                break;
            default:
                break;
        }
    }

    void OnGUI() {
        if (GameState == ConstData.GameState.stage1 && !noInput) {
            if (Event.current.type == EventType.KeyDown/* && !cameraMoving*/) {
                int inputKey = GetIntFromKeyCode(Event.current.keyCode);
                    print("GameManager.cs inpu:"+inputKey);
                switch (inputKey) {
                    case 1:
                        InputArrowKey = 1;
                        bc.enabled = true;
                        ChangeDerayColliderEnable();
                        print("GameManager.cs OnGUI press left");
                    break;
                    case 2:
                        InputArrowKey = 2;
                        bc.enabled = true;
                        ChangeDerayColliderEnable();
                        print("GameManager.cs OnGUI press down");
                    break;
                    case 3:
                        InputArrowKey = 3;
                        bc.enabled = true;
                        ChangeDerayColliderEnable();
                        print("GameManager.cs OnGUI press right");
                    break;
                    case 4:
                        InputArrowKey = 4;
                        bc.enabled = true;
                        ChangeDerayColliderEnable();
                        print("GameManager.cs OnGUI press up");
                    break;
                    default:
                    break;
                }
            }
        }
	}

    private int InputKey() {
        InputArrowKey = 0;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow)) {
            InputArrowKey = 1;
            bc.enabled = true;
            print("GameManager.cs InputKey press left");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            InputArrowKey = 3;
            bc.enabled = true;
            print("GameManager.cs InputKey press right");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            InputArrowKey = 4;
            bc.enabled = true;
            print("GameManager.cs InputKey press up");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            InputArrowKey = 2;
            bc.enabled = true;
            print("GameManager.cs InputKey press down");
        }
        return InputArrowKey;
    }

    private void ChangeDerayColliderEnable() {
        DOVirtual.DelayedCall(
            0.1f,   // 遅延させる（待機する）時間
            () => {
                bc.enabled = false;// こちらに実行する処理
            }
        );
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var aa = other.gameObject.GetComponent<ArrowCommand>().Direction;
        print("GameManager.cs OnTriggerEnter2D direction:"+aa + " " + InputArrowKey);
        if (GameState == ConstData.GameState.stage1) {
            if (aa == InputArrowKey && aa != 0) {
                DownCameraAndCollision(moveCameraY);
                other.gameObject.SetActive(false);
                int randomHand = Random.Range(0, 4);
                if ( randomHand > 2 ) {
                    leftSP.sprite = fists[randomHand];
                    leftHand.DOMove(
                        new Vector3(cameraTrans.position.x-1.0f, cameraTrans.position.y-3.5f, -1.1f),　　//移動後の座標
                        0.15f 　　　　　　//時間
                    ).OnComplete(
                        () => {
                        //cameraTrans.DOShakePosition(2.0f, new Vector3(10.0f, 10.0f, 10.0f), 10, 40, true, false);
                            _audioSource.PlayOneShot(punchSE[randomHand], 1.0f);
                            leftHand.DOLocalMove (
                                new Vector3(-1.0f, -3.0f, -1.0f),　　//移動後の座標
                                0.15f 　　　　　　//時間
                            );
                        }
                    );
                } else {
                    rightSP.sprite = fists[randomHand];
                    rightHand.DOMove(
                        new Vector3(cameraTrans.position.x+1.0f, cameraTrans.position.y-3.5f, -1.1f),　　//移動後の座標
                        0.15f 　　　　　　//時間
                    ).OnComplete(
                        () => {
                        //cameraTrans.DOShakePosition(2.0f, new Vector3(10.0f, 10.0f, 10.0f), 10, 40, true, false);
                            _audioSource.PlayOneShot(punchSE[randomHand], 1.0f);
                            rightHand.DOLocalMove (
                                new Vector3(1.0f, -3.0f, -1.0f),　　//移動後の座標
                                0.15f 　　　　　　//時間
                            );
                        }
                    );
                }
                int random = Random.Range(0, 6);
                GameObject arrow = Instantiate(ArrowsPrefabs[random], transform.position, Quaternion.identity);
                arrow.transform.SetParent(Arrows.transform);
                Combo += 1;
                Score += 100 + Combo * 10;
                if (MaxCombo < Combo) {
                    MaxCombo = Combo;
                }
            } else {
                noInput = true;
                int randomHand = Random.Range(0, 4);
                if ( randomHand > 2 ) {
                    leftSP.sprite = fists[randomHand];
                    leftHand.DOMove(
                        new Vector3(cameraTrans.position.x-1.0f, cameraTrans.position.y-1.5f, -1.1f),　　//移動後の座標
                        0.15f 　　　　　　//時間
                    ).OnComplete(
                        () => {
                            cameraTrans.DOPunchPosition(new Vector3(0.2f, 0.2f, 0.2f), noInputInterval)
                            .OnComplete(
                                () => {
                                    noInput = false;
                                }
                            );
                            _audioSource.PlayOneShot(missSE[randomHand], 1.0f);
                            leftHand.DOLocalMove (
                                new Vector3(-1.0f, -3.0f, -1.0f),　　//移動後の座標
                                0.15f 　　　　　　//時間
                            );
                        }
                    );
                } else {
                    rightSP.sprite = fists[randomHand];
                    rightHand.DOMove(
                        new Vector3(cameraTrans.position.x+1.0f, cameraTrans.position.y-1.5f, -1.1f),　　//移動後の座標
                        0.15f 　　　　　　//時間
                    ).OnComplete(
                        () => {
                            cameraTrans.DOPunchPosition(new Vector3(0.2f, 0.2f, 0.2f), noInputInterval)
                            .OnComplete(
                                () => {
                                    noInput = false;
                                }
                            );
                            _audioSource.PlayOneShot(missSE[randomHand], 1.0f);
                            rightHand.DOLocalMove (
                                new Vector3(1.0f, -3.0f, -1.0f),　　//移動後の座標
                                0.15f 　　　　　　//時間
                            );
                        }
                    );
                    Combo = 0;
                }
            }
        }
        bc.enabled = false;
    }

    private void DownCameraAndCollision(float moveY) {
        cameraMoving = true;
        cameraTrans.DOMove (
            cameraTrans.position + new Vector3(0, moveY, 0),　　//移動後の座標
            colliderDispTime //時間
        ).SetEase(Ease.OutQuint);
        transform.DOMove (
            transform.position + new Vector3(0, moveY, 0),　　//移動後の座標
            colliderDispTime-0.4f 　　　　　　//時間
        )
        .OnComplete(() => {
            cameraMoving = false;
        });
        // transform.position += new Vector3(0, moveCameraY, 0);
        //cameraTrans.position += new Vector3(0, moveCameraY, 0);
    }

    private int GetIntFromKeyCode(KeyCode keyCode) {
        switch (keyCode) {
            case KeyCode.LeftArrow:
                print("GameManager.cs GetIntFromKeyCode Left:"+keyCode);
            return 1;
            case KeyCode.DownArrow:
                print("GameManager.cs GetIntFromKeyCode Down:"+keyCode);
            return 2;
            case KeyCode.RightArrow:
                print("GameManager.cs GetIntFromKeyCode Right:"+keyCode);
            return 3;
            case KeyCode.UpArrow:
                print("GameManager.cs GetIntFromKeyCode Up:"+keyCode);
            return 4;
            default: //上記以外のキーが押された場合は「null文字」を返す。
            return '\0';
        }
    }
}
