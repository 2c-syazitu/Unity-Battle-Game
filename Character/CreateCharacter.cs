using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateCharacter
{
    private List<Character> characterList = new List<Character>();
    private List<Player> plaerList = new List<Player>();
    private List<Enemy> EnemyList = new List<Enemy>();

    public CreateCharacter(BattleSystem sys)
    {
        /*                            name,hp,mp,atk,mgc,def,spd*/
        characterList.Add(new Player("aourora", 100, 100, 10, 10, 0, 10, sys));
        characterList.Add(new Player("hewi", 100, 100, 10, 10, 0, 9, sys));
        characterList.Add(new Player("skarner", 100, 100, 10, 10, 0, 8, sys));
        characterList.Add(new Player("zyra", 100, 100, 10, 10, 0, 7, sys));


        characterList.Add(new Doragon("Doragon1", 100, 100, 10, 10, 0, 6, sys));
        characterList.Add(new Doragon("Doragon2", 100, 100, 10, 10, 0, 5, sys));
        characterList.Add(new Doragon("Doragon3", 100, 100, 10, 10, 0, 4, sys));

        for (int i = 0; i < characterList.Count; i++)
        {
            setList(characterList[i]);
        }

        /*アイテムとかのテスト用*/
        characterList[1].setEquipment(new NormalSword(sys));
        characterList[0].setEquipment(new ThunderSword(sys));
    }

    public void setList(Character c)
    {
        if (c is Player p)
        {
            plaerList.Add(p);
        }
        else if (c is Enemy e)
        {
            EnemyList.Add(e);
        }
    }

    public List<Character> getCharacterList()
    {
        return characterList;
    }

    public List<Player> getPlayerList()
    {
        return plaerList;
    }

    public List<Enemy> getEnemyList()
    {
        return EnemyList;
    }
}
