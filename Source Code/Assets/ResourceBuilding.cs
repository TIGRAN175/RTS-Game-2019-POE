﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets
{

    [Serializable]

    class ResourceBuilding : Building
    {
        protected string type;
        protected int resourcesGenerated;
        protected int resourcesGenPerRound;
        protected int poolRemaining;
        public static int MAX_POOL_AMOUNT;

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
            get { return maxHealth; }
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

        public int ResourcesGenerated
        {
            get { return resourcesGenerated; }
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

        public ResourceBuilding(int x, int y, int hp, int teamNum, string type, int resourcesGenerated, int resourcesGenPerRound, int poolRemaining) : base(x,y,hp,teamNum, '$')
        {
            this.type = type;
            this.resourcesGenerated = resourcesGenerated;
            this.resourcesGenPerRound = resourcesGenPerRound;
            this.poolRemaining = poolRemaining;
            MAX_POOL_AMOUNT = poolRemaining;
        }

        public void GenerateResourcesForRound()
        {
            if(poolRemaining >= resourcesGenPerRound)
            {
                //take resources from the pool
                poolRemaining = poolRemaining - resourcesGenPerRound; // take from pool
                resourcesGenerated = resourcesGenerated + resourcesGenPerRound; // add to generated 
            }else if(poolRemaining > 0)
            {
                resourcesGenerated = resourcesGenerated + poolRemaining;
                poolRemaining = 0;
            }
            else
            {
                //no resources left...
               // Debug.WriteLine("No resources remaining for team " + Team);
            }        

        }

        public override void DeathHandler(Map map)
        {
            map.DestroyBuidling(this);

        }

        public override string ToString()
        {
            return "Resource build - team: " + team + " (" +xPos + "," + yPos + ")" +"  health: "+ health +"/" + maxHealth + "  resGen: " + resourcesGenerated + "  resPerRound: " + resourcesGenPerRound + "  poolRem: " + poolRemaining ;
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
