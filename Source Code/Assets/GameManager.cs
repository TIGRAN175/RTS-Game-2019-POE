using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{


    public class GameManager : MonoBehaviour
    {
        public  GameObject BlueMeelee;
        public  GameObject BlueRanged;
        public  GameObject RedMeelee;
        public  GameObject RedRanged;
        public  GameObject GreenWizard;
        public GameObject BlueResourceBuilding;
        public GameObject RedResourceBuilding;
        public GameObject BlueFactoryBuilding;
        public GameObject RedFactoryBuilding;
        public Text GameOverText;
        public GameObject WinScreen;

        float currentTime = 0f;
        float startingTime = 10f;

        bool toSpawn = true;
        Map map;
        GameEngine gameEngine;

        public void DestroyGameObject(GameObject obj)
        {
            Destroy(obj);
            
        }

        public bool isGameOver = false;
        public string gameOverMessage = "";

        public GameObject FactorySpawnUnit(Unit u)
        {
            var actualPos = new Vector3(u.XPos, 0, u.YPos);

            if(u.Team ==0)
            {
                if(u is MeeleeUnit)
                {

                    return Instantiate(BlueMeelee, new Vector3(u.XPos, 1, u.YPos), Quaternion.Euler(new Vector3(0, 180, -90)));

                }
                else if (u is RangedUnit)
                {
                    return Instantiate(BlueRanged, actualPos, transform.rotation);

                }
            }
            else if (u.Team == 1)
            {
                if (u is MeeleeUnit)
                {
                    return Instantiate(RedMeelee, new Vector3(u.XPos, 1, u.YPos), Quaternion.Euler(new Vector3(0, 180, -90)));

                }
                else if (u is RangedUnit)
                {
                    return Instantiate(RedRanged, actualPos, transform.rotation);

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
            gameEngine = new GameEngine(20, 6, 20, 20, 40, this);
            map = gameEngine.map;
            //map = new Map(10, 2, 20, 20);


            // meelee rotations: 0, 180, -90
            //wiz  270, 0, 0
            //GameObject unit = Instantiate(BlueMeelee, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 180, -90)));
           /*
            GameObject unit = Instantiate(RedMeelee, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 180, -90)));

            UnitDamageHandler h = unit.GetComponent<UnitDamageHandler>() as UnitDamageHandler;
          // h = new UnitDamageHandler();
           h.UpdateHealth(75, 100);
           h.UpdateHealth(50, 100);
            
            */
            //goal is to place units on the unity plane. including correct models and colors

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
                                unitToMap.UnityObject = Instantiate(BlueMeelee, new Vector3(unitToMap.XPos, 1, unitToMap.YPos), Quaternion.Euler(new Vector3(0, 180, -90)));
                                
                            }
                            else if (unitToMap is RangedUnit)
                            {
                                unitToMap.UnityObject = Instantiate(BlueRanged, actualPos, transform.rotation);

                            }
                        }
                        else if (unitToMap.Team == 1)
                        {
                            //Red team
                            if (unitToMap is MeeleeUnit)
                            {
                                unitToMap.UnityObject = Instantiate(RedMeelee, new Vector3(unitToMap.XPos, 1, unitToMap.YPos), Quaternion.Euler(new Vector3(0, 180, -90)));

                            }
                            else if (unitToMap is RangedUnit)
                            {
                                unitToMap.UnityObject = Instantiate(RedRanged, actualPos, transform.rotation);

                            }
                        }
                        else if (unitToMap.Team == 2)
                        {
                            //Wizards
                            unitToMap.UnityObject = Instantiate(GreenWizard, actualPos, Quaternion.Euler(new Vector3(270, 0, 0)));

                        }
                    }
                }
            }
            
            //Place buildings on the unity plane
            foreach (Building b in map.buildingList)
            {
                var actualPos = new Vector3(b.XPos, 0, b.YPos);
                b.GameManager = this;
                if (b.Team == 0)
                {
                    if(b is FactoryBuilding)
                    {
                        b.UnityObject = Instantiate(BlueFactoryBuilding, actualPos, Quaternion.Euler(new Vector3(270, 0, 0)));
                    }
                    else if(b is ResourceBuilding)
                    {
                        b.UnityObject = Instantiate(BlueResourceBuilding, actualPos, Quaternion.Euler(new Vector3(270, 0, 0)));

                    }

                }
                else if (b.Team == 1)
                {
                    //grid.Rows[b.XPos].Cells[b.YPos].Style.BackColor = Color.Pink;
                    if (b is FactoryBuilding)
                    {
                        b.UnityObject = Instantiate(RedFactoryBuilding, actualPos, Quaternion.Euler(new Vector3(270, 0, 0)));

                    }
                    else if (b is ResourceBuilding)
                    {
                        b.UnityObject = Instantiate(RedResourceBuilding, actualPos, Quaternion.Euler(new Vector3(270, 0, 0)));

                    }
                }
            }
            
         
        }

        // Update is called once per frame
        public float movementSpeed = 1;
        GameObject obj1 = null, obj2 = null, obj3 = null;
        int oldWholeNumber = -1;
        void Update()
        {
           // isGameOver = true;
            //gameOverMessage = "Round Limit Reached! It's a draw!";
            if (isGameOver)
            {
                DisplayGameOver();
                return;  
            }
           

            currentTime = currentTime - 1 * Time.deltaTime;
            string stringNumber = currentTime.ToString("0");
            int wholeNumber = System.Int32.Parse(stringNumber);
            if(oldWholeNumber != wholeNumber)
            {
                if ((wholeNumber) % 4 == 0)
                {
                    gameEngine.PerformRound();
                }
                oldWholeNumber = wholeNumber;
            }

            
            

        }

        public void DisplayGameOver()
        {
            GameOverText.text = gameOverMessage;
            //WinScreen.SetActive(true);
        }

        public void MoveUnitTowards( Vector3 toPos, GameObject unitToMove) 
        {
            StartCoroutine(MoveOverSeconds(unitToMove, toPos, 2f));
        }

        public GameObject CreateGameObject(GameObject prefab, Transform trans, Transform transRot)
        {
            // return new Instantiate(prefab, trans, transRot);
            return null;
        }


        public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
        {
            if(objectToMove == null)
            {
                print("Trying to Move corpses");
                yield break;
            }
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