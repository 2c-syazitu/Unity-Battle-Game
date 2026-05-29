using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class BattleSystem : MonoBehaviour
{
    /*フェーズの管理*/
    enum Phase
    {
        Start,
        ChoseCommand,
        Execute,
        Result,
        End
    }

    /*コマンド選択フェーズでの管理*/
    enum ChoseCommandPhase
    {
        Wait,
        AttackPhase,
        SkillPhase,
        DefPhase,
        ItemPhase,
        SelectTargetPhase,
    }

    /*セレクト画面での入力状態の管理*/
    /*0:Back = 戻るボタン
      1:True = 戻る以外の任意のボタン
      2:False = 入力待機
    */
    public enum SelectedTarget
    {
        True, //0
        False, //1
        Back, //2
    }

    public enum BattleResult
    {
        None,
        Victory,
        Defeat,
    }


    [SerializeField] private GameObject selectWindowPanel;
    [SerializeField] private SelectWindow selectWindow;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private EnemyUI enemyUI;
    [SerializeField] private Transform playerPanel;
    [SerializeField] private Transform enemyPanel;
    [SerializeField] private TextMeshProUGUI turnLabel;
    [SerializeField] private TextMeshProUGUI textLabel;
    [SerializeField] private GameObject butCover;
    [SerializeField] private Lv lvData;

    private CreateCharacter creChar;

    private Character turnHolder;
    private List<Character> targetList = new List<Character>();
    private Turn turn;
    private List<Character> characterList = new List<Character>();
    private List<Player> playerList = new List<Player>();
    private List<Enemy> enemyList = new List<Enemy>();

    private Skill attack;
    private Skill defnce;
    private Phase phase;
    private ChoseCommandPhase commandPahse;
    private BattleResult battleResult;

    /*戦闘で分配される経験値の総量*/
    private int exp;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        creChar = new CreateCharacter(this);

        /*諸々のリストを取得*/
        characterList = creChar.getCharacterList();
        playerList = creChar.getPlayerList();
        enemyList = creChar.getEnemyList();

        /*素早さソートされるリスト*/
        List<Character> list = new List<Character>(characterList);

        /*通常攻撃の作成*/
        attack = new Attack(this);
        defnce = new Defence(this);

        /*それぞれのUIをセット*/
        foreach (Character c in characterList)
        {
            CharacterUI uiPrefab = null;
            Transform parent = null;

            if (c is Player p)
            {
                uiPrefab = playerUI;
                parent = playerPanel;
            }
            else if (c is Enemy e)
            {
                uiPrefab = enemyUI;
                parent = enemyPanel;
            }
            if (uiPrefab != null)
            {
                // インスタンス化して、基底クラスのメソッドでセット
                CharacterUI instance = Instantiate(uiPrefab, parent);
                c.setUI(instance);
            }
        }

        /*諸々の初期化*/
        turn = new Turn(list);
        phase = Phase.Start;
        commandPahse = ChoseCommandPhase.Wait;
        exp = 0;

        /*バトルスタート*/
        StartCoroutine(Battle());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Battle()
    {
        while (phase != Phase.End)
        {
            Debug.Log(phase);
            turnHolder = turn.getTurnHolder();
            yield return null;
            switch (phase)
            {
                case Phase.Start:
                    phase = Phase.ChoseCommand;
                    break;
                case Phase.ChoseCommand:
                    turnLabel.text = $"{turnHolder.getName()}'s turn!!";
                    /*コマンド選択のコルーチン*/
                    yield return StartCoroutine(ChoseCommand());
                    /*フェーズが変更されるまで*/
                    yield return new WaitUntil(() => phase != Phase.ChoseCommand);

                    break;
                case Phase.Execute:
                    Debug.Log($"bs143:{turnHolder.getAtk()}");
                    Skill skill = turnHolder.getExecuteSkill();
                    if (skill.getCastTurn() == Skill.SkillCastTurn.Quick)
                    {
                        skill.activate(turnHolder, targetList);
                    }
                    else if (skill.getCastTurn() == Skill.SkillCastTurn.Charge)
                    {
                        Debug.Log($"bs152:{skill.getName()} ; {turnHolder.getTargetList().Count}");
                        skill.activate(turnHolder, turnHolder.getTargetList());
                    }

                    /*実行スキルがアイテムスキルだった場合アイテムの使用処理*/
                    if (skill.getItem() != null)
                    {
                        skill.getItem().useItem(turnHolder);
                    }
                    targetList.Clear();
                    /*テキストの出力が終わるまで待機*/
                    yield return new WaitUntil(() => getInteractable());

                    /*戦闘終了するかどうか*/
                    /*戦闘続行*/
                    if (finishBattle() == BattleResult.None)
                    {
                        turn.changeTurn();
                        phase = Phase.ChoseCommand;
                    }
                    /*戦闘終了・勝利*/
                    else if (finishBattle() == BattleResult.Victory)
                    {
                        phase = Phase.Result;
                    }
                    /*戦闘終了・敗北*/
                    else if (finishBattle() == BattleResult.Defeat)
                    {

                    }

                    break;
                case Phase.Result:
                    phase = Phase.End;
                    /*経験値計算
                      分配は生存しているプレイヤーに均等に
                    */
                    /*生存したプレイヤーの数*/
                    int alive = 0;
                    for (int i = 0; i < playerList.Count; i++)
                    {
                        alive++;
                    }

                    /*生存したプレイヤーに経験値をセット*/
                    for (int i = 0; i < playerList.Count; i++)
                    {
                        /*プレイヤーが生存しているなら*/
                        if (playerList[i].getAlive())
                        {
                            playerList[i].setExp((float)exp / alive);
                            yield return new WaitUntil(() => getInteractable());
                        }
                    }
                    break;
                case Phase.End:
                    break;
            }
        }
    }



    private SelectedTarget selectedTarget = SelectedTarget.False;

    IEnumerator ChoseCommand()
    /*コマンド選択画面でのコルーチン*/
    {
        Debug.Log($"bs177:{turnHolder.getTargetList().Count}");
        if ((turnHolder.getExecuteSkill() != null) &&
            (turnHolder.getExecuteSkill().getCastTurn() == Skill.SkillCastTurn.Charge))
        {
            phase = Phase.Execute;
        }
        /*諸々の初期化*/
        selectedTarget = SelectedTarget.False;
        commandPahse = ChoseCommandPhase.Wait;
        selectWindowPanel.SetActive(false);
        selectWindow.delBut();

        setTextLabel($"{turnHolder.getName()} turn.\nplease chose command.");

        yield return new WaitUntil(() => getInteractable());

        /*ボタンの入力を有効*/
        butCover.SetActive(false);

        /*フェーズが移行するまで*/
        while (phase == Phase.ChoseCommand)
        {
            /*セレクト画面の初期化*/
            Debug.Log($"bs195");
            selectWindowPanel.SetActive(false);
            selectWindow.delBut();

            yield return new WaitUntil(() => commandPahse != ChoseCommandPhase.Wait);
            /*ボタンの入力を無効*/
            butCover.SetActive(true);

            switch (commandPahse)
            {
                /*通常攻撃*/
                case ChoseCommandPhase.AttackPhase:
                    /*実行スキルは固定で通常攻撃*/
                    turnHolder.setExecute(attack);
                    selectedTarget = SelectedTarget.True;

                    /*対象に応じてフェーズ移行*/
                    changeSelectPhase();
                    break;

                /*スキル*/
                case ChoseCommandPhase.SkillPhase:
                    /*行動キャラのスキルボタンの配置、表示*/
                    selectWindowPanel.SetActive(true);
                    selectWindow.createSkillBut(turnHolder);

                    /*実行スキルがセレクト画面で選択されるまで*/
                    yield return new WaitUntil(() => selectedTarget != SelectedTarget.False);

                    /*対象が選択された時点でセレクト画面を非表示*/
                    selectWindowPanel.SetActive(false);

                    /*対象に応じてフェーズ移行*/
                    changeSelectPhase();
                    break;

                case ChoseCommandPhase.DefPhase:
                    /*実行スキルは固定で防御*/
                    Debug.Log("bs238");
                    turnHolder.setExecute(defnce);
                    targetList.Clear();
                    targetList.Add(turnHolder);
                    Debug.Log($"bs238:taList.Count {targetList.Count} : turn {turnHolder.getName()}");
                    turnHolder.setTargetList(targetList);

                    /*フェーズ移行*/
                    phase = Phase.Execute;
                    break;

                case ChoseCommandPhase.ItemPhase:
                    selectWindowPanel.SetActive(true);
                    selectWindow.createItemBut(turnHolder);

                    /*アイテムが選択されるまで*/
                    yield return new WaitUntil(() => selectedTarget != SelectedTarget.False);

                    /*対象が選択された時点でセレクト画面を非表示*/
                    selectWindowPanel.SetActive(false);

                    /*対象に応じてフェーズ移行*/
                    changeSelectPhase();
                    break;

                /*対象選択*/
                case ChoseCommandPhase.SelectTargetPhase:
                    selectedTarget = SelectedTarget.False;

                    /*対象リストと実行スキルを参照して表示*/
                    selectWindow.createChaBut(createTargetList(turnHolder, turnHolder.getExecuteSkill()), turnHolder);
                    selectWindowPanel.SetActive(true);

                    /*対象が選択されるまで*/
                    yield return new WaitUntil(() => selectedTarget != SelectedTarget.False);

                    /*対象が選択された時点でセレクト画面を非表示*/
                    selectWindowPanel.SetActive(false);

                    if (selectedTarget == SelectedTarget.True)
                    {
                        /*フェーズ移行*/
                        phase = Phase.Execute;
                        targetList = selectWindow.getTargetList();
                    }
                    else if (selectedTarget == SelectedTarget.Back)
                    {
                        /*諸々の初期化*/
                        selectedTarget = SelectedTarget.False;
                        commandPahse = ChoseCommandPhase.Wait;
                        butCover.SetActive(false);
                    }
                    break;
            }
        }
    }

    /*実行スキルに合わせて対象選択可能リストを生成*/
    public List<Character> createTargetList(Character turnHolder, Skill executeSkill)
    {
        List<Character> list = new List<Character>();

        /*スキルの対象の生死*/
        bool tf = executeSkill.getTargetAlive();
        /*スキルの対象の陣営*/
        Skill.SkillTarget target = executeSkill.getTarget();
        Character.CharacterType type = turnHolder.getType();

        for (int i = 0; i < characterList.Count; i++)
        {

            /*生死の判定*/
            if (characterList[i].getAlive() == tf)
            {
                /*対象が味方*/
                if (target == Skill.SkillTarget.Ally)
                {
                    /*タイプが一致すれば*/
                    if (characterList[i].getType() == type)
                    {
                        list.Add(characterList[i]);
                    }
                }
                /*対象が敵*/
                else if (target == Skill.SkillTarget.Enemy)
                {
                    /*タイプが異なれば*/
                    if (characterList[i].getType() != type)
                    {
                        list.Add(characterList[i]);
                    }
                }
            }
        }
        return list;
    }

    /*対象に応じてフェーズ移行*/
    public void changeSelectPhase()
    {
        Debug.Log($"bs327:{selectedTarget}");
        if (selectedTarget == SelectedTarget.True)
        {
            if (turnHolder.getNowMp() < turnHolder.getExecuteSkill().getCost())
            {
                /*MPが足りない時の処理*/
                selectedTarget = SelectedTarget.False;
                commandPahse = ChoseCommandPhase.Wait;
                setTextLabel("Not enough MP");
            }
            /*対象に応じてフェーズ移行*/
            /*単体なら対象選択*/
            if (turnHolder.getExecuteSkill().getRange() == Skill.SkillRange.Single)
            {
                /*フェーズ移行*/
                if (createTargetList(turnHolder, turnHolder.getExecuteSkill()).Count > 1)
                {
                    /*対象が複数いるなら対象選択*/
                    commandPahse = ChoseCommandPhase.SelectTargetPhase;
                }
                else
                {
                    /*対象が一体なら即時実行*/
                    targetList = createTargetList(turnHolder, turnHolder.getExecuteSkill());
                    turnHolder.setTargetList(targetList);
                    phase = Phase.Execute;
                }
            }
            /*全体なら対象選択せずに実行*/
            else if (turnHolder.getExecuteSkill().getRange() == Skill.SkillRange.Area)
            {
                /*targetListに対象リストを代入*/
                targetList = createTargetList(turnHolder, turnHolder.getExecuteSkill());
                turnHolder.setTargetList(targetList);
                phase = Phase.Execute;
            }
        }
        else if (selectedTarget == SelectedTarget.Back)
        {
            /*諸々の初期化*/
            selectedTarget = SelectedTarget.False;
            commandPahse = ChoseCommandPhase.Wait;
            butCover.SetActive(false);
        }
        Debug.Log($"bs373:{selectedTarget}");
    }

    public List<Character> getPlayerList()
    {
        return characterList;
    }

    public Character getTurnHolder()
    {
        return turnHolder;
    }

    /*セッター*/
    public void setExecuteSkill(Skill skill)
    {
        turnHolder.setExecute(skill);
    }

    /*テキストの出力が可能かどうか(出力中か否か)*/
    private bool interactableText = true;
    /*テキストを一時的に保存するキュー*/
    private Queue<string> textQueue = new Queue<string>();

    public void setTextLabel(string text)
    {
        /*キューにテキストを入力*/
        textQueue.Enqueue(text);

        if (interactableText)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    IEnumerator ProcessQueue()
    {
        interactableText = false;
        /*キューの先頭から順番に出力*/
        while (textQueue.Count > 0)
        {
            /*キューの先頭を取得、削除*/
            string nextText = textQueue.Dequeue();
            yield return StartCoroutine(ShowText(nextText));
            /*一文の出力が終わった時点で秒数待機*/
            yield return new WaitForSeconds(0.5f);
        }
        interactableText = true;
    }

    IEnumerator ShowText(string text)
    {
        /*テキストラベルのリセット*/
        textLabel.text = "";
        /*テキストを一文字ずつ順に出力*/
        foreach (char c in text)
        {
            textLabel.text += c;
            /*一文字間に遅延をセット*/
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void setExp(int i)
    {
        exp += i;
    }

    /*戦闘終了の判定
      想定として、敵味方両方が同時に全滅したら敗北判定
    */
    public BattleResult finishBattle()
    {
        battleResult = BattleResult.None;
        /*勝利*/
        if (enemyList.All(e => !e.getAlive()))
        {
            battleResult = BattleResult.Victory;
        }
        /*敗北*/
        if (playerList.All(p => !p.getAlive()))
        {
            battleResult = BattleResult.Defeat;
        }

        return battleResult;
    }

    /*次の処理に移行可能かの判定*/
    public bool getInteractable()
    {
        //　　テキスト処理
        if (interactableText &&
            //キャラクターのアニメーション処理
            characterList.All(c => c.getInteractableAnime())
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void setSelectedTarget(int i)
    /*入力されたターゲットの判定*/
    /*0:Back = 戻るボタン
      1:True = 戻る以外の任意のボタン
      2:False = 入力待機
    */

    {
        if (i == 0)
        {
            selectedTarget = SelectedTarget.True;
        }
        else if (i == 1)
        {
            selectedTarget = SelectedTarget.False;
        }
        else if (i == 2)
        {
            selectedTarget = SelectedTarget.Back;
        }
    }


    /*コマンド選択フェーズでの状態管理*/
    public void setChoseCommandPhase(int i)
    {
        if (i == 0)
        {
            commandPahse = ChoseCommandPhase.Wait;
        }
        else if (i == 1)
        {
            commandPahse = ChoseCommandPhase.AttackPhase;
        }
        else if (i == 2)
        {
            commandPahse = ChoseCommandPhase.SkillPhase;
        }
        else if (i == 3)
        {
            commandPahse = ChoseCommandPhase.DefPhase;
        }
        else if (i == 4)
        {
            commandPahse = ChoseCommandPhase.ItemPhase;
        }
        else if (i == 5)
        {
            commandPahse = ChoseCommandPhase.SelectTargetPhase;
        }
    }

    public Lv getLvData()
    {
        return lvData;
    }
}
