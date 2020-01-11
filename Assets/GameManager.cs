using UnityEngine;

namespace Assets
{


    public class GameManager : MonoBehaviour
    {
        public  GameObject BlueCube;
        public  GameObject BlueCylinder;
        public  GameObject RedCube;
        public  GameObject RedCylinder;
        public  GameObject GreenSphere;



        bool toSpawn = true;
        Map map;
        GameEngine gameEngine;
        // Use this for initialization
        void Start()
        {
            GameEngine gameEngine;
            map = new Map(10, 2, 20, 20);


            //goal is to place units on the unity plane. including correct models and colors

            //bind units to GameObjects
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




            //gameEngine = new GameEngine(10, 5, 20, 20, 40, grid, timer, textBox, lblWinner, txtResources0, txtResources1);
            //gameEngine.StartTimer();
            //map = gameEngine.map;
            //gameEngine.PerformRound();
            // int roundToUpdate = gameEngine.PerformRound();

            print("AWE GENTSSSSSzzzz");
            // Cube = Instantiate(Cube, new Vector3(0, 0, 0), Quaternion.identity);
        }

        // Update is called once per frame
        public float movementSpeed = 1;
        GameObject obj1 = null, obj2 = null, obj3 = null;
        void Update()
        {

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
            print("direction: " + direction);
            obj1.transform.position += direction * Time.deltaTime;
            obj2.transform.position += new Vector3(1, 0, 1) * Time.deltaTime;
    */
        }

        public GameObject CreateGameObject(GameObject prefab, Transform trans, Transform transRot)
        {
            // return new Instantiate(prefab, trans, transRot);
            return null;
        }




    }
}