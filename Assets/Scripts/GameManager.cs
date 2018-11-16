using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public TowerBtn ClickedBtn { get; set; }

    public ObjectPool Pool { get; set; }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        HandleEscape();
	}

    public void PickTower(TowerBtn towerBtn)
    {
        this.ClickedBtn = towerBtn;
        Hover.Instance.Activate(towerBtn.Sprite);
    }

    private void HandleEscape()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
        }
    }

    public void StartWave()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {

        LevelManager.Instance.GeneratePath();

        int monsterIndex = Random.Range(0, 2);

        string type = string.Empty;

        switch (monsterIndex)
        {
            case 0:
                type = "gingerOrc";
                break;

            case 1:
                type = "blueOrc";
                break;
            case 2:

                type = "helmetOrc";
                break;
        }

       Monster monster = Pool.GetObject(type).GetComponent<Monster>();
        monster.Spawn();

        yield return new WaitForSeconds(2.5f);
    }
}
