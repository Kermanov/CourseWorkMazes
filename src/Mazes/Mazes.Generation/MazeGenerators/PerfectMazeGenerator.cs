﻿using Mazes.Generation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mazes.Generation.MazeGenerators
{
    public class PerfectMazeGenerator : IMazeGenerator
    {
        private Maze maze;
        private bool[,] visitedCells;
        private CellPosition currentPosition;
        private int visitedCounter;
        private Random rand;
        private Stack<CellPosition> stack;

        public PerfectMazeGenerator()
        {
            rand = new Random();
            stack = new Stack<CellPosition>();
        }

        public Maze Generate(int width, int height)
        {
            maze = new Maze(width, height);
            visitedCells = new bool[height, width];
            visitedCounter = 0;

            currentPosition = new CellPosition(0, 0);
            SetVisited(currentPosition);

            while (!IsAllVisited())
            {
                var nextRandomUnvisitedCellPos = GetNextRandomUnvisitedCellPos(currentPosition);
                if (nextRandomUnvisitedCellPos != null)
                {
                    stack.Push(currentPosition);
                    RemoveWall(currentPosition, nextRandomUnvisitedCellPos);
                    currentPosition = nextRandomUnvisitedCellPos;
                    SetVisited(currentPosition);
                }
                else if (stack.Count > 0)
                {
                    currentPosition = stack.Pop();
                }
                else
                {
                    currentPosition = GetRandomUnvisitedCellPos();
                    SetVisited(currentPosition);
                }
            }

            return maze;
        }

        private void SetVisited(CellPosition cellPosition)
        {
            visitedCells[cellPosition.Row, cellPosition.Col] = true;
            ++visitedCounter;
        }

        private bool IsAllVisited()
        {
            return visitedCounter == maze.Height * maze.Width;
        }

        private bool IsVisited(CellPosition cellPosition)
        {
            return visitedCells[cellPosition.Row, cellPosition.Col];
        }

        private CellPosition GetNextRandomUnvisitedCellPos(CellPosition cellPosition)
        {
            var nextCellPositions = maze.GetNextCellPositions(cellPosition);
            nextCellPositions.RemoveAll(cellPos => !IsVisited(cellPos));

            if (nextCellPositions.Count > 0)
            {
                var randomIndex = rand.Next(0, nextCellPositions.Count);
                return nextCellPositions[randomIndex];
            }
            else
            {
                return null;
            }
        }

        private void RemoveWall(CellPosition cellPos1, CellPosition cellPos2)
        {
            if (cellPos1.Row < cellPos2.Row)
            {
                maze[cellPos2].TopSide = SideState.Open;
            }
            if (cellPos1.Row > cellPos2.Row)
            {
                maze[cellPos1].TopSide = SideState.Open;
            }
            if (cellPos1.Col < cellPos2.Col)
            {
                maze[cellPos2].LeftSide = SideState.Open;
            }
            if (cellPos1.Col > cellPos2.Col)
            {
                maze[cellPos1].LeftSide = SideState.Open;
            }
        }

        private CellPosition GetRandomUnvisitedCellPos()
        {
            var unvisitedCellPositions = new List<CellPosition>();
            for (int i = 0; i < maze.Height; ++i)
            {
                for (int j = 0; j < maze.Width; ++j)
                {
                    var cellPosition = new CellPosition(i, j);
                    if (!IsVisited(cellPosition))
                    {
                        unvisitedCellPositions.Add(cellPosition);
                    }
                }
            }

            var randomIndex = rand.Next(0, unvisitedCellPositions.Count);
            return unvisitedCellPositions[randomIndex];
        }
    }
}
