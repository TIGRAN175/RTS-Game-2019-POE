using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets
{

    [Serializable]
    class FactoryBuilding : Building
    {



        protected int spawnX = -1;
        protected int spawnY = -1;

        protected int unitType; // 0 for meelee and 1 for ranged
        protected int productionSpeed;

        //Properties

        public override int XPos
        {
            get { return xPos; }
            set { xPos = value; }
        }

        public override int YPos
        {
            get { return yPos; }
            set { yPos = value; }
        }
        public override int Health
        {
            get { return health; }
            set { health = value; }
        }

        public override int MaxHealth
        {
            get { return  maxHealth; }
        }

        public override int Team
        {
            get { return team; }
            set { team = value; }
        }

        public override char Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }

        public override GameManager GameManager
        {
            get
            {
                return gameManager;
            }

            set
            {
                gameManager = value;
            }
        }

        public override GameObject UnityObject { get { return unityObject; } set { unityObject = value; } }

        public FactoryBuilding(int x, int y, int hp, int teamNum, int unitType, int productionSpeed, Map map) : base(x, y, hp, teamNum, 'F')
        {
            this.unitType = unitType;
            this.productionSpeed = productionSpeed;
            if(map.IsInMap(x + 1, y))
            {
                spawnX = x + 1;
                spawnY = y;
            }
            else if(map.IsInMap(x - 1, y))
            {
                spawnX = x - 1;
                spawnY = y;
            }
            
           
        }

        //this will be called by the game engine every X number of rounds
        public void SpawnUnit(Map map)
        {
            if(spawnX == -1)
            {
                return; // there is no valid place to spawn the unit
            }

            if(map.unitMap[spawnX, spawnY] != null )
            {
                //a unit is already there, so dont spawn
                return;
            }

            //otherwise spawn:

            System.Random rand = new System.Random();
            if(unitType == 0)
            {
                //meelee
                MeeleeUnit m = new MeeleeUnit(spawnX, spawnY, team, map.meeleeNames[rand.Next(0, map.meeleeNames.Length)] );
                m.GameManager = gameManager;
                map.PlaceUnit(m);
            }else if(unitType == 1)
            {
                //ranged
                RangedUnit r = new RangedUnit(spawnX, spawnY, team, map.rangedNames[rand.Next(0, map.rangedNames.Length)]);
                r.GameManager = gameManager;
                map.PlaceUnit(r);

            }

        }

        public int GetProductionSpeed()
        {
            return productionSpeed;
        }

        

        public override void DeathHandler(Map map)
        {
            map.DestroyBuidling(this);
        }

        public override string ToString()
        {
            return "Factory - team " + team + " (" + xPos + "," + yPos + ")" + "  health: " + health + "/" + maxHealth + "  unitType: " + (unitType == 0 ? "meelee" : "ranged") + "  spawnRate: " + productionSpeed + "  spawnPoint: (" + spawnX + "," + spawnY+ ")";
        }

        public override void Save()
        {
            string pathString = System.IO.Path.GetRandomFileName() + ".dat";
            var systemPath = Directory.GetCurrentDirectory();
            string dirPath = Path.Combine(systemPath, "buildings/");
            var complete = Path.Combine(dirPath, pathString);

            if (Directory.Exists(dirPath))
            {
                File.Create(complete).Close();
            }
            else
            {
                Directory.CreateDirectory(dirPath);
                File.Create(complete).Close();
            }
            Stream stream = new FileStream(complete, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(stream, this);
            stream.Close();
        }
    }
}
