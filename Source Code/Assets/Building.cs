using System;
using UnityEngine;

namespace Assets
{
    [Serializable]
    abstract public class Building
    {
        protected int xPos;
        protected int yPos;
        protected int health;
        protected int maxHealth;
        protected int team;
        protected char symbol;
        protected GameObject unityObject;
        protected GameManager gameManager;

        public Building(int xPos, int yPos, int health, int team, char symbol)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.health = health;
            this.team = team;
            this.symbol = symbol;
            maxHealth = health;
        }

        public abstract GameManager GameManager { get; set; }
        public abstract GameObject UnityObject { get; set; }
        public abstract int XPos { get; set; }
        public abstract int YPos { get; set; }
        public abstract int MaxHealth { get; }
        public abstract int Health { get; set; }
        public abstract int Team { get; set; }
        public abstract char Symbol { get; set; }

        public abstract void Save();
        public abstract void DeathHandler(Map map);
        public abstract string ToString();

    }
}
