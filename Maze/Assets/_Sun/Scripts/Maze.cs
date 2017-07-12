using System.Collections;
using System.Collections.Generic;
using System;

namespace SimpleMaze
{
    /// <summary>
    /// DFS maze
    /// </summary>
    public class Maze
    {
        int width, height;
        int[,] maze;
        Random random;
        Stack<Coordinate> reviewStack = new Stack<Coordinate>();
        List<Coordinate> deadEnds = new List<Coordinate>();
        public Maze(int width, int height)
        {
            this.width = width;
            this.height = height;
            random = new Random();
            maze = new int[width, height];

            Coordinate current = RandomPosition();
            reviewStack.Push(current);
            List<Coordinate> neighbours;
            while (reviewStack.Count > 0)
            {
                maze[current.x, current.y] = 1;
                neighbours = GetNeighbours(current);
                if (neighbours.Count == 0)
                {
                    if (NotVisited(current)) deadEnds.Add(current);
                    current = reviewStack.Pop();
                }
                else
                {
                    reviewStack.Push(current);
                    current = neighbours[random.Next(neighbours.Count)];
                }

            }

        }


        public List<Coordinate> GetDeadEnds()
        {
            return deadEnds;
        }
        public int GetMazeAt(int x, int y)
        {
            if (Validate(new Coordinate(x, y)))
            {
                return maze[x, y];
            }
            else return -1;

        }




        List<Coordinate> GetNeighbours(Coordinate coordinate)
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            for (int i = 0; i < Direction.Length; i++)
            {
                Coordinate temp = coordinate + Direction[i];
                if (temp.x % 2 == 1 || temp.y % 2 == 1)
                {
                    if (Validate(temp))
                        if (maze[temp.x, temp.y] == 0 && NotVisited(temp))
                        {
                            coordinates.Add(temp);
                        }
                }
            }
            return coordinates;
        }
        bool NotVisited(Coordinate coordinate)
        {
            int x = 0;
            for (int i = 0; i < Direction.Length; i++)
            {
                Coordinate c = coordinate + Direction[i];
                if (Validate(c))
                {
                    if (maze[c.x, c.y] == 0) x++;
                }
            }

            return x == 3;
        }
        bool Validate(Coordinate coordinate)
        {
            if (coordinate.x < 0 || coordinate.x >= width || coordinate.y < 0 || coordinate.y >= height) return false;
            return true;
        }


        Coordinate RandomPosition()
        {
            Coordinate coordinate = Coordinate.Zero;
            while (coordinate.x % 2 == 0)
            {
                coordinate.x = random.Next(width);
            }
            while (coordinate.y % 2 == 0)
            {
                coordinate.y = random.Next(height);
            }
            return coordinate;
        }
        public static readonly Coordinate[] Direction = {
        new Coordinate(-1,0), //Left
        new Coordinate(0,1), //Top
  	 	new Coordinate(1,0),	//Right
   		new Coordinate(0,-1)}; // Bottom

    }


    public struct Coordinate
    {

        public int x;
        public int y;

        public static readonly Coordinate One = new Coordinate(1, 1);
        public static readonly Coordinate Zero = new Coordinate(0, 0);
        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static Coordinate operator +(Coordinate a, Coordinate b)
        {
            a.x += b.x;
            a.y += b.y;
            return a;
        }

        public static Coordinate operator /(Coordinate a, int integer)
        {
            a.x /= integer;
            a.y /= integer;
            return a;
        }

        public static bool operator ==(Coordinate a, Coordinate b)
        {
            if (a.x == b.x && a.y == b.y) return true;
            else return false;
        }

        public static bool operator !=(Coordinate a, Coordinate b)
        {
            if (a.x == b.x && a.y == b.y) return false;
            else return true;
        }


        public override bool Equals(object obj)
        {
            Coordinate coordinate = (Coordinate)obj;

            return this.x == coordinate.x && this.y == coordinate.y;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode();
        }

    }
}