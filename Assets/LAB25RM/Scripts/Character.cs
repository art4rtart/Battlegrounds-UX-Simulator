using System;
using UnityEngine;

[Serializable]
public class Character 
{
    public enum CharacterType { Humman, Zombie };
    public CharacterType characterType;

    public enum ZombieAnimType { Normal, Crawl };
    public ZombieAnimType zombieType;

    public  string name;
    public float health;
}
