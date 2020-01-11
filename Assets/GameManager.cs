using System;
using System.Collections;
using UnityEngine;

namespace Assets
{


    public class GameManager : MonoBehaviour
    {
        public  GameObject BlueCube;
        public  GameObject BlueFinalRanged;
        public  GameObject RedCube;
        public  GameObject RedCylinder;
        public  GameObject GreenSphere;
        public GameObject BlueResourceBuilding;
        public GameObject RedResourceBuilding;
        public GameObject BlueFactoryBuilding;
        public GameObject RedFactoryBuilding;
        public GameObject RedFinalRanged;

        float currentTime = 0f;
        float startingTime = 10f;

        bool toSpawn = true;
        Map map;
        GameEngine gameEngine;

        public void DestroyGameObject(GameObject obj)
        {
            Destroy(obj);
            
        }

       

        public GameObject FactorySpawnUnit(Unit u)
        {
            var actualPos = new Vector3(u.XPos, 0, u.YPos);

            if(u.Team ==0)
            {
                if(u is MeeleeUnit)
                {
                    return Instantiate(BlueCube, actualPos, transform.rotation);

                }
                else if (u is RangedUnit)
                {
                    return Instantiate(BlueFinalRanged, actualPos, transform.rotation);

                }
            }
            else if (u.Team == 1)
            {
                if (u is MeeleeUnit)
                {
                    return Instantiate(RedCube, actualPos, transform.rotation);

                }
                else if (u is RangedUnit)
                {
                    return Instantiate(RedCylinder, actualPos, transform.rotation);

                }

            }
            print("BIG ISSSUEEE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            return null;
        }

        public void printToConsole(string message)
        {
            print(message);
        }

        // Use this for initialization
         void Start()
        {
            currentTime = startingTime;
            gameEngine = new GameEngine(10, 4, 20, 20, 40, this);
            map = gameEngine.map;
            //map = new Map(10, 2, 20, 20);

            GameObject unit = Instantiate(BlueCube, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(270, 0, 0)));
            if(unit != null)
            {
                print("unit not null");
            }
            else
            {
                print("unit null");
            }
            UnitDamageHandler h = unit.GetComponent<UnitDamageHandler>() as UnitDamageHandler;
            //h = new UnitDamageHandler();
            h.UpdateHealth(75, 100);
            //goal is to place units on the unity plane. including correct models and colors
/*
            //bind UNITS to GameObjects
            for (int i = 0; i < map.MAP_ROWS; i++)
            {
                for (int j = 0; j < map.MAP_COLS; j++)
                {
                    if (map.unitMap[i, j] != null)
                    {
                        //we have a wizard, ranged or meelee unit
                        Unit unitToMap = map.unitMap[i, j];
                        //unitToMap.UnityObject = new GameObject();
                        var actualPos = new Vector3(unitToMap.XPos, 0, unitToMap.YPos);
                        unitToMap.GameManager = this;

                        if (unitToMap.Team == 0)
                        {
                            //blue team
                            if (unitToMap is MeeleeUnit)
                            {
                                //unitToMap.UnityObject = Instantiate(BlueCube, actualPos, transform.rotation);
                                actualPos = new Vector3(0, 0, 0);
                                unitToMap.UnityObject = Instantiate(BlueCube, actualPos, transform.rotation);
                                
                            }
                            else if (unitToMap is RangedUnit)
                            {
                                unitToMap.UnityObject = Instantiate(BlueCylinder, actualPos, transform.rotation);

                            }
                        }
                        else if (unitToMap.Team == 1)
                        {
                            //Red team
                            if (unitToMap is MeeleeUnit)
                            {
                                unitToMap.UnityObject = Instantiate(RedCube, actualPos, transform.rotation);

                            }
                            else if (unitToMap is RangedUnit)
                            {
                                unitToMap.UnityObject = Instantiate(RedCylinder, actualPos, transform.rotation);

                            }
                        }
                        else if (unitToMap.Team == 2)
                        {
                            //Wizards
                            unitToMap.UnityObject = Instantiate(GreenSphere, actualPos, transform.rotation);

                        }
                    }
                }
            }
            
            //Place buildings on the unity plane
            foreach (Building b in map.buildingList)
            {
                var actualPos = new Vector3(b.XPos, 0, b.YPos);
                b.GameManager = this;
                //grid.Rows[b.XPos].Cells[b.YPos].Value = b.Symbol;
                if (b.Team == 0)
                {
                    //grid.Rows[b.XPos].Cells[b.YPos].Style.BackColor = Color.LightBlue;
                    if(b is FactoryBuilding)
                    {
                        b.UnityObject = Instantiate(BlueFactoryBuilding, actualPos, transform.rotation);
                    }
                    else if(b is ResourceBuilding)
                    {
                        b.UnityObject = Instantiate(BlueResourceBuilding, actualPos, transform.rotation);

                    }

                }
                else if (b.Team == 1)
                {
                    //grid.Rows[b.XPos].Cells[b.YPos].Style.BackColor = Color.Pink;
                    if (b is FactoryBuilding)
                    {
                        b.UnityObject = Instantiate(RedFactoryBuilding, actualPos, transform.rotation);

                    }
                    else if (b is ResourceBuilding)
                    {
                        b.UnityObject = Instantiate(RedResourceBuilding, actualPos, transform.rotation);

                    }
                }
            }
            */
         
        }

        // Update is called once per frame
        public float movementSpeed = 1;
        GameObject obj1 = null, obj2 = null, obj3 = null;
        int oldWholeNumber = -1;
        void Update()
        {
            return;
            /*
            if (toSpawn)
            {
                obj1 = new GameObject();
                obj2 = new GameObject();
                obj3 = new GameObject();

                obj1 = Instantiate(BlueCube, transform.position, transform.rotation);
                obj2 = Instantiate(BlueCube, transform.position, transform.rotation);
                obj3 = Instantiate(BlueCube, transform.position, transform.rotation);

                obj1.transform.position = new Vector3(3, 1, 3);
                obj2.transform.position = new Vector3(1, 1, 6);
                obj3.transform.position = new Vector3(6, 1, 10);
                toSpawn = false;
            }
            //to object 2


            var direction = (obj2.transform.position - obj1.transform.position).normalized;
            //print("direction: " + direction);
            obj1.transform.position += direction * Time.deltaTime;
            obj2.transform.position += new Vector3(1, 0, 1) * Time.deltaTime;
            */

            currentTime = currentTime - 1 * Time.deltaTime;
            string stringNumber = currentTime.ToString("0");
            int wholeNumber = System.Int32.Parse(stringNumber);
            if(oldWholeNumber != wholeNumber)
            {
                if ((wholeNumber) % 4 == 0)
                {
                    print(wholeNumber);
                    gameEngine.PerformRound();
                }
                oldWholeNumber = wholeNumber;
            }

            
            

        }

        public void MoveUnitTowards( Vector3 toPos, GameObject unitToMove) 
        {
            //var direction = (fromPos.transform.position - toPos.transform.position).normalized;
            //unitToMove.transform.position += direction * Time.deltaTime;
           // unitToMove.transform.position = toPos;

            StartCoroutine(MoveOverSeconds(unitToMove, toPos, 2f));


        }

        public GameObject CreateGameObject(GameObject prefab, Transform trans, Transform transRot)
        {
            // return new Instantiate(prefab, trans, transRot);
            return null;
        }


        public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
        {
            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.position;
            while (elapsedTime < seconds)
            {
                objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToMove.transform.position = end;
        }



    }
}