﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCon : MonoBehaviour
{
    [SerializeField] GameObject player;
    //[SerializeField] GameObject enemy;
    [SerializeField] List<GameObject> enemys;
    [SerializeField] List<int> enemysSetPlayerNo;
    [SerializeField] GameObject goal;
    [SerializeField] CameraCon cameraCon;
    //[SerializeField] MazeMake mazeMake;//マップ生成スクリプト
    [SerializeField] int mapSize;
    public float insPosiY_Player;
    public float insPosiY_Enemy;
    public float insPosiY_Goal;
    float time;
    float consoleTime = 0;//デバッグ用
    GameObject insPlayer;
    CharCon_Y  charCon;



    // Start is called before the first frame update
    void Start()
    {
        insPlayer = Instantiate(player);
        cameraCon.setTransform(insPlayer.transform);
        charCon = insPlayer.GetComponent<CharCon_Y>();
        InsObject(enemys[SetEneNo()],insPosiY_Enemy);
        InsObject(goal,insPosiY_Goal);
        
        //mapSize = mazeMake.getMapSize();//マップ生成スクリプトからマップサイズを取得
        insPlayer.transform.position = ObjectPosition(mapSize,insPosiY_Player);
        //InsObject(goal);//ゴール生成
        //InsObject(enemy);//エネミー生成

    }

    // Update is called once per frame
    void Update()
    {
        GameTime();
        //ConsoleTime();//デバッグ用
        Goal();
    }

    Vector3 ObjectPosition(int mapSize,float y)//プレイヤー、エネミーのポジションを設定
    {
        int x = Random.Range(0, (mapSize + 1) / 2) * 2;
        int z = Random.Range(0, (mapSize + 1) / 2) * 2;
        Vector3 posi = new Vector3(x, y, z);
        return posi;
    }
    void InsObject(GameObject gameObject,float f)//オブジェクト（エネミー）をマップに追加
    {
        if (gameObject != null)
        {
            Vector3 posi = new Vector3();
            while (true)//プレイヤーとエネミーのポジションが被らなくなるまで繰り返す
            {
                posi = ObjectPosition(mapSize,f);
                if (!(posi.x == insPlayer.transform.position.x &&
                    posi.z == insPlayer.transform.position.z))
                {
                    break;
                }
            }
            GameObject e = Instantiate(gameObject);
            e.transform.position = posi;
            SetPlayer(e);
        }
        else
        {
            Debug.Log("insObject_null");
        }
       
    }

    int SetEneNo()
    {
        return Random.Range(0, enemys.Count);
    }

    //エラー回避のため、無効化しています。
    void SetPlayer(GameObject e)
    {
        if (EnemyCheck(e))
        {
            //e.EnemyController.Player(insPlayer);
        }
    }
    bool EnemyCheck(GameObject e)
    {
        foreach (int i in enemysSetPlayerNo)
        {
            if(e == enemys[i])
            {
                return true;
            }
        }
        return false;
    }
    
    float GameTime()//実時間取得
    {
        time += Time.deltaTime;
        return time;
    }
    void ConsoleTime()//Debug用　コンソールに経過時間表示
    {
        if (time - consoleTime >= 1)
        {
            Debug.Log(time + "秒");
            consoleTime = time;
        }
    }
    void ReturnToTitle()
    {
        SceneManager.LoadScene("");
    }
    void Goal()
    {
        if (charCon.GetIsGoal())
        {
            Debug.Log("GameCon_Goal:" + charCon.GetIsGoal());
            charCon.StartGoalAnim();
        }
    }
    void GameOver()
    {
        if (charCon.Life() >= charCon.DefaultLife())
        {
            Debug.Log("GameCon_GameOver");
        }
    }
}
