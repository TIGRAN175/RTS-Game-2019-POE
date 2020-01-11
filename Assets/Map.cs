using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets
{

    class Map
    {
        public readonly string[] meeleeNames = { "Knight", "Foot Soldier", "Musketeer" };
        public readonly string[] rangedNames = { "Archer", "Sniper", "Artillary" };
        public List<Building> buildingList;
       // DataGridView myGrid;
        public struct Position
        {
            public int rowIndex, colIndex;
            public Position(int p1, int p2)
            {
                rowIndex = p1;
                colIndex = p2;
            }
        }

        public readonly int MAP_ROWS = 30;
        public readonly int MAP_COLS = 30;
        public readonly int WIZARD_COUNT = 4;

        public Unit[,] unitMap;
        public Map(int numUnitsToCreate, int numBuildingsToCreate, int mapRows, int mapCols)
        {
            MAP_ROWS = mapRows;
            MAP_COLS = mapCols;
            unitMap = new Unit[MAP_ROWS, MAP_COLS];
            buildingList = new List<Building>();
            if (numUnitsToCreate > (MAP_ROWS * MAP_COLS))
            {
                numUnitsToCreate = MAP_ROWS * MAP_COLS;
            }

            System.Random rand = new System.Random();

            numUnitsToCreate = numUnitsToCreate - WIZARD_COUNT;

            int numUnitsTeamOne = numUnitsToCreate / 2;
            int numUnitsTeamTwo = numUnitsToCreate - numUnitsTeamOne;
            while (numUnitsToCreate > 0)
            {
                int rowIndex, colIndex;
                do
                {
                    rowIndex = rand.Next(0, MAP_ROWS);
                    colIndex = rand.Next(0, MAP_COLS);
                } while (unitMap[rowIndex, colIndex] != null);
                //at this point we have an empty spot

                int teamNumber = 0; //default team zero
                if (numUnitsToCreate > numUnitsTeamOne)
                {
                    teamNumber = 1;
                }

                int unitType = rand.Next(0, 2);
                if (unitType == 0)
                {
                    //create meelee

                    unitMap[rowIndex, colIndex] = new MeeleeUnit(rowIndex, colIndex, teamNumber, meeleeNames[rand.Next(0, meeleeNames.Length)]);
                }
                else if (unitType == 1)
                {
                    //create ranged
                    unitMap[rowIndex, colIndex] = new RangedUnit(rowIndex, colIndex, teamNumber, rangedNames[rand.Next(0, rangedNames.Length)]);
                }
                numUnitsToCreate--;
            }

            //create wizards before buildings
            int wizardsToCreate = WIZARD_COUNT;
            while (wizardsToCreate > 0)
            {
                int rowIndex, colIndex;
                do
                {
                    rowIndex = rand.Next(0, MAP_ROWS);
                    colIndex = rand.Next(0, MAP_COLS);
                } while (unitMap[rowIndex, colIndex] != null);
                //at this point we have an empty spot
                unitMap[rowIndex, colIndex] = new Wizard(rowIndex, colIndex);
                wizardsToCreate--;
            }



            //begin to add buildings
            int numBuildingsTeam1 = numBuildingsToCreate / 2;
            int numBuildingsTeam2 = numBuildingsToCreate - numBuildingsTeam1;

            while (numBuildingsToCreate > 0)
            {
                int rowIndex, colIndex;
                do
                {
                    rowIndex = rand.Next(0, MAP_ROWS);
                    colIndex = rand.Next(0, MAP_COLS);
                } while (unitMap[rowIndex, colIndex] != null && !DoesBuildingConflictWithOtherBuilding(rowIndex, colIndex));

                int teamNumber = 0; //default team zero
                if (numBuildingsToCreate > numBuildingsTeam1)
                {
                    teamNumber = 1;
                }

                int buildingType = rand.Next(0, 2);
                if (buildingType == 0)
                {
                    //create Factory building
                    buildingList.Add(new FactoryBuilding(rowIndex, colIndex, 100, teamNumber, rand.Next(0, 2), 2, this));
                }
                else if (buildingType == 1)
                {
                    //create Resource building
                    buildingList.Add(new ResourceBuilding(rowIndex, colIndex, 100, teamNumber, "gold", 0, 10, 50));
                }

                numBuildingsToCreate--;
            }

            

        }

        public bool DoesBuildingConflictWithOtherBuilding(int xPos, int yPos)
        {
            foreach (Building b in buildingList)
            {
                if (b.XPos == xPos && b.YPos == yPos)
                {
                    return true;
                }
                else if (b.XPos == xPos - 1)
                {
                    return true; // to ensure a factory is not directly on top of another
                }
            }
            return false;
        }

        public void GenerateMap(GameManager gameManager)
        {
            //goal is to place units on the unity plane. including correct models and colors

            //bind units to GameObjects
           /* for(int i =0; i< MAP_ROWS; i++)
            {
                for(int j =0; j < MAP_COLS; j++)
                {
                    if(unitMap[i,j] != null)
                    {
                        //we have a wizard, ranged or meelee unit
                        Unit unitToMap = unitMap[i, j];
                        if(unitToMap.Team == 0)
                        {
                            //blue team
                            if(unitToMap is MeeleeUnit)
                            {
                                unitToMap.UnityObject = new GameObject();
                                unitToMap.UnityObject = Instantiate(GameManager.BlueCube, gameManager.transform.position, gameManager.transform.rotation);
                            }
                            else if(unitToMap is RangedUnit)
                            {
                                
                            }
                        }else if(unitToMap.Team == 1)
                        {
                            //Red team
                            if (unitToMap is MeeleeUnit)
                            {

                            }
                            else if (unitToMap is RangedUnit)
                            {

                            }
                        }
                        else if(unitToMap.Team == 2)
                        {
                            //Wizards
                        }
                    }
                }
            }
            
            */
            //Generate buildings
            /*
            foreach (Building b in buildingList)
            {
                grid.Rows[b.XPos].Cells[b.YPos].Value = b.Symbol;
                if (b.Team == 0)
                {
                    grid.Rows[b.XPos].Cells[b.YPos].Style.BackColor = Color.LightBlue;

                }
                else if (b.Team == 1)
                {
                    grid.Rows[b.XPos].Cells[b.YPos].Style.BackColor = Color.Pink;
                }
            }
            */

        }

        public void ClearMap()
        {
            //clear units
            for (int i = 0; i < MAP_ROWS; i++)
            {
                for (int j = 0; j < MAP_COLS; j++)
                {
                    unitMap[i, j] = null;
                   // myGrid.Rows[i].Cells[j].Style.BackColor = Color.White;
                   // myGrid.Rows[i].Cells[j].Value = "";
                }
            }

            //clear buildings
            foreach (Building b in buildingList)
            {
               // myGrid.Rows[b.XPos].Cells[b.YPos].Style.BackColor = Color.White;
               // myGrid.Rows[b.XPos].Cells[b.YPos].Value = "";
            }
            buildingList.Clear();

        }

        

        public bool IsWithinRange(Unit attacker, Unit enemy)
        {
            int attackerX = attacker.XPos;
            int attackerY = attacker.YPos;
            int enemyX = enemy.XPos;
            int enemyY = enemy.YPos;

            double pythagDistance = CalculatePythagDistance(attackerX, attackerY, enemyX, enemyY);
            int onlyIntegerPart = (int)pythagDistance;
            if (attacker.AttackRange >= onlyIntegerPart)
            {
                // we can reach the unit
                return true;
            }
            else
            {
                //cant reach the unit
                return false;
            }
        }

        public bool IsWithinRange(Unit attacker, Building enemy)
        {
            int attackerX = attacker.XPos;
            int attackerY = attacker.YPos;
            int enemyX = enemy.XPos;
            int enemyY = enemy.YPos;

            double pythagDistance = CalculatePythagDistance(attackerX, attackerY, enemyX, enemyY);
            int onlyIntegerPart = (int)pythagDistance;
            if (attacker.AttackRange >= onlyIntegerPart)
            {
                // we can reach the unit
                return true;
            }
            else
            {
                //cant reach the unit
                return false;
            }
        }

        public List<Position> GetValidMoves(Unit unitToMove)
        {
            int rowIndex = unitToMove.XPos;
            int colIndex = unitToMove.YPos;
            List<Position> tempMoves = new List<Position>();
            tempMoves.Add(new Position(rowIndex - 1, colIndex - 1)); //topLeft
            tempMoves.Add(new Position(rowIndex - 1, colIndex));//toptop
            tempMoves.Add(new Position(rowIndex - 1, colIndex + 1)); //topRight
            tempMoves.Add(new Position(rowIndex, colIndex + 1)); // rightRight
            tempMoves.Add(new Position(rowIndex + 1, colIndex + 1));//bottomRight
            tempMoves.Add(new Position(rowIndex + 1, colIndex)); // bottom bottom
            tempMoves.Add(new Position(rowIndex + 1, colIndex - 1)); // bottom left
            tempMoves.Add(new Position(rowIndex, colIndex - 1)); // left left
            List<Position> actualMoves = new List<Position>();
            foreach (Position pos in tempMoves)
            {
                if (CanMoveTo(pos))
                {
                    actualMoves.Add(pos);
                }
            }
            return actualMoves;
        }

        public bool CanMoveTo(Position pos)
        {
            if (IsInMap(pos.rowIndex, pos.colIndex))
            {
                //check if any units conflict or buildings
                if (unitMap[pos.rowIndex, pos.colIndex] == null)
                {
                    //ADD BUILDING CHECK HERE
                    foreach (Building b in buildingList)
                    {
                        if (pos.rowIndex == b.XPos && pos.colIndex == b.YPos)
                        {
                            return false;
                        }
                    }

                    return true;
                }

            }
            return false;
        }

        public void MoveUnitRandomly(Unit unitToMove)
        {
            List<Position> possibleMoves = GetValidMoves(unitToMove);
            System.Random rand = new System.Random();
            if (possibleMoves.Count == 0)
            {
                //Debug.WriteLine("Cant move unit, no possible moves: " + unitToMove.ToString());
                return;
            }

            int randomPosIndex = rand.Next(0, possibleMoves.Count);
            Position randomPositionChosen = possibleMoves.ElementAt(randomPosIndex);
            MoveUnit(unitToMove, randomPositionChosen.rowIndex, randomPositionChosen.colIndex);
        }

        public double CalculatePythagDistance(int x1, int y1, int x2, int y2)
        {
            int xVal = Math.Abs(x1 - x2);
            int yVal = Math.Abs(y1 - y2);
            return Math.Sqrt((xVal * xVal) + (yVal * yVal));
        }

        public void DestroyUnit(Unit unitToDestroy)
        {
            unitMap[unitToDestroy.XPos, unitToDestroy.YPos] = null;
           // myGrid[unitToDestroy.YPos, unitToDestroy.XPos].Style.BackColor = Color.White;
            //myGrid[unitToDestroy.YPos, unitToDestroy.XPos].Value = "";
        }

        public void DestroyBuidling(Building buildingToDestroy)
        {
            buildingList.Remove(buildingToDestroy);
           // myGrid[buildingToDestroy.YPos, buildingToDestroy.XPos].Style.BackColor = Color.White;
           // myGrid[buildingToDestroy.YPos, buildingToDestroy.XPos].Value = "";


        }

        public void MoveUnit(Unit unit, int rowIndexToMove, int colIndexToMove)
        {
            if (!IsInMap(rowIndexToMove, colIndexToMove))
            {
                return;
            }

            int currX = unit.XPos;
            int currY = unit.YPos;
            unit.MoveToPosition(rowIndexToMove, colIndexToMove);
            unitMap[currX, currY] = null;
            unitMap[rowIndexToMove, colIndexToMove] = unit;
           // myGrid[colIndexToMove, rowIndexToMove].Value = "" + unit.Symbol;
            //myGrid[currY, currX].Style.BackColor = Color.White;


           // Color colorToSet = Color.White;
            if (unit.Team == 0)
            {
              //  colorToSet = Color.Aqua;
            }
            else if (unit.Team == 1)
            {
               // colorToSet = Color.Red;
            }else if(unit.Team == 2)
            {
                //colorToSet = Color.Orange;
            }
           // myGrid[colIndexToMove, rowIndexToMove].Style.BackColor = colorToSet;
            //myGrid[currY, currX].Value = "";
        }

        //NB modify for task 3
        public void PlaceUnit(Unit unit)
        {
           // Color colorToSet = Color.White;
            if (unit.Team == 0)
            {
             //   colorToSet = Color.Aqua;
            }
            else if (unit.Team == 1)
            {
               // colorToSet = Color.Red;
            }else if(unit.Team == 2)
            {
              //  colorToSet = Color.Orange;

            }
            unitMap[unit.XPos, unit.YPos] = unit; // add to unitMap
          //  myGrid[unit.YPos, unit.XPos].Style.BackColor = colorToSet; //actual screen grid
            //myGrid[unit.YPos, unit.XPos].Value = "" + unit.Symbol;
        }

        public List<Unit> GetUnitList()
        {
            List<Unit> unitList = new List<Unit>();
            for (int i = 0; i < MAP_ROWS; i++)
            {
                for (int j = 0; j < MAP_COLS; j++)
                {
                    if (unitMap[i, j] != null)
                    {
                        unitList.Add(unitMap[i, j]);
                    }
                }
            }
            return unitList;
        }

        public void MoveTowardsEnemy(Unit unitToMove, int enemyX, int enemyY)
        {
            List<Position> validPositions = GetValidMoves(unitToMove);
            if (validPositions.Count == 0)
            {
                //Debug.WriteLine("Cant Move unit is stuck: " + unitToMove.ToString());
                return;
            }

            //we know the unit can move if we reach here

            List<double> pythagDistances = new List<double>();
            foreach (Position pos in validPositions)
            {
                double currPythagDistance = CalculatePythagDistance(pos.rowIndex, pos.colIndex, enemyX, enemyY);
                pythagDistances.Add(currPythagDistance);
            }

            //at this point we have the corresponding pythags for each coordinate
            int smallestIndex = 0;
            double smallestPythag = pythagDistances.ElementAt(0);

            for (int i = 1; i < pythagDistances.Count; i++)
            {
                if (pythagDistances.ElementAt(i) < smallestPythag)
                {
                    smallestPythag = pythagDistances.ElementAt(i);
                    smallestIndex = i;
                }
            }

            //now we have the correct coordinate at index smallestIndex

            Position finalPositionToMove = validPositions.ElementAt(smallestIndex);
            MoveUnit(unitToMove, finalPositionToMove.rowIndex, finalPositionToMove.colIndex);
        }


        public bool IsInMap(int rowIndex, int colIndex)
        {
            if (rowIndex >= MAP_ROWS || rowIndex < 0)
            {
                return false;
            }

            if (colIndex >= MAP_COLS || colIndex < 0)
            {
                return false;
            }

            //Debug.WriteLine("(" + rowIndex + "," + colIndex + ") is in map");

            return true;
        }

        public Building IsBuildingAtPosition(int x, int y)
        {
            foreach (Building b in buildingList)
            {
                if (b.XPos == x && b.YPos == y)
                {
                    return b;
                }
            }

            return null;
        }

        public String PrintMap()
        {
            String o = "";
            for (int i = 0; i < MAP_ROWS; i++)
            {
                for (int j = 0; j < MAP_COLS; j++)
                {
                    if (unitMap[i, j] != null)
                    {
                        // Console.Write(unitMap[i,j].ToString() + "|");
                        o += unitMap[i, j].Symbol + "|";
                    }
                    else
                    {
                        o += "  |";
                    }

                }
                //Console.WriteLine();
                o += "\n";
            }
            return o;
        }
    }
}
