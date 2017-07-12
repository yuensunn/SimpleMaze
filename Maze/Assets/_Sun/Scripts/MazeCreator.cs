using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SimpleMaze
{
    [AddComponentMenu("SimepleMaze/MazeCreator")]
    public class MazeCreator : MonoBehaviour
    {

        public int width;
        public int height;
        public float size;
        Maze maze;
        public Material material;
        public GameObject cellPrefab;
        public GameObject playerObject;
        public GameObject subject;

        void Start()
        {
            maze = new Maze(width, height);
            CreatMazeObject();
            PlaceActorAndSubjects();
        }

        void CreatMazeObject()
        {

            GameObject wallGameObject = new GameObject();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (maze.GetMazeAt(i, j) == 1)
                    {
                        GameObject path = GenerateCell(i, j, transform);
                        AssignMaterial(i, j, path.GetComponent<Renderer>(), path.GetComponent<MeshFilter>());
                    }
                    else
                    {
                      GenerateCell(i, j, wallGameObject.transform);
                    }

                }
            }
            Utility.MeshCombiner.CombineMesh(gameObject, true); // Combine path gameobjects to 1 gameobject
            Utility.MeshCombiner.CombineMesh(wallGameObject, true); //combine wall gameobjects to 1 gameobject
            Utility.Mesh2DColliderMaker.CreatePolygon2DColliderPoints(
                wallGameObject.GetComponent<MeshFilter>(), wallGameObject.AddComponent<PolygonCollider2D>());// Make polygoncollider for wall
        }

        void PlaceActorAndSubjects()
        {
            foreach (Coordinate item in maze.GetDeadEnds())
            {
                Instantiate(subject,GetPosition(item.x,item.y),Quaternion.identity);
            }
            Coordinate c = new Coordinate(Random.Range(0, width), Random.Range(0, height)); // get random position to locate playerobject
            while (maze.GetDeadEnds().Contains(c) || maze.GetMazeAt(c.x, c.y) == 0)
            {
                c = new Coordinate(Random.Range(0, width), Random.Range(0, height));
            }

            playerObject.transform.position = GetPosition(c.x, c.y);
        }

        void AssignMaterial(int i, int j, Renderer renderer, MeshFilter mesh)
        {
            renderer.material = material;
            Orientation orientation = Orientation.None;
            if (maze.GetMazeAt(i, j + 1) != 1) orientation |= Orientation.Top;
            if (maze.GetMazeAt(i, j - 1) != 1) orientation |= Orientation.Down;
            if (maze.GetMazeAt(i - 1, j) != 1) orientation |= Orientation.Left;
            if (maze.GetMazeAt(i + 1, j) != 1) orientation |= Orientation.Right;
            mesh.mesh.uv = UV[orientation];
        }

        GameObject GenerateCell(int x, int y, Transform parent)
        {
            GameObject go = GameObject.Instantiate(cellPrefab, GetPosition(x, y), Quaternion.identity);
            go.transform.localScale = Vector3.one * size;
            go.transform.SetParent(parent);
            return go;
        }
        Vector3 GetPosition(int x, int y)
        {
            float totalWidth = width * size;
            float totalHeight = height * size;
            return new Vector3(x * size - ((totalWidth / 2) - size / 2), y * size - ((totalHeight / 2) - size / 2), 0);
        }
        static readonly Dictionary<Orientation, Vector2[]> UV = new Dictionary<Orientation, Vector2[]> // Static UV Map
    {
       {(Orientation)1, new Vector2[]{new Vector2(0,0.75f), new Vector2(0.25f,1f), new Vector2(0.25f,0.75f), new Vector2(0,1f)}},
        {(Orientation)8, new Vector2[]{new Vector2(0.25f,0.75f),new Vector2(0.5f,1f),new Vector2(0.5f,0.75f),new Vector2(0.25f,1f)}},
        {(Orientation)4, new Vector2[]{new Vector2(0.5f,0.75f),new Vector2(0.75f,1),new Vector2(0.75f,0.75f),new Vector2(0.5f,1f)}},
        {(Orientation)2, new Vector2[]{new Vector2(0.75f,0.75f),new Vector2(1,1),new Vector2(1,0.75f),new Vector2(0.75f,1f)}},

        {(Orientation)9, new Vector2[]{new Vector2(0,0.5f), new Vector2(0.25f,0.75f), new Vector2(0.25f,0.5f), new Vector2(0,0.75f)}},
        {(Orientation)12, new Vector2[]{new Vector2(0.25f,0.5f), new Vector2(0.5f,0.75f), new Vector2(0.5f,0.5f), new Vector2(0.25f,0.75f)}},
        {(Orientation)6, new Vector2[]{new Vector2(0.5f,0.5f), new Vector2(0.75f,0.75f), new Vector2(0.75f,0.5f), new Vector2(0.5f,0.75f)}},
        {(Orientation)3, new Vector2[]{new Vector2(0.75f,0.5f), new Vector2(1f,0.75f), new Vector2(1f,0.5f), new Vector2(0.75f,0.75f)}},

         {(Orientation)10, new Vector2[]{new Vector2(0,0.25f), new Vector2(0.25f,0.5f), new Vector2(0.25f,0.25f), new Vector2(0,0.5f)}},
        {(Orientation)5, new Vector2[]{new Vector2(0.25f,0.25f), new Vector2(0.5f,0.5f), new Vector2(0.5f,0.25f), new Vector2(0.25f,0.5f)}},
        {(Orientation)11,new Vector2[]{new Vector2(0.5f,0.25f), new Vector2(0.75f,0.5f), new Vector2(0.75f,0.25f), new Vector2(0.5f,0.5f)}},
       {(Orientation)13,new Vector2[]{new Vector2(0.75f,0.25f), new Vector2(1f,0.5f), new Vector2(1f,0.25f), new Vector2(0.75f,0.5f)}},

        {(Orientation)14, new Vector2[]{new Vector2(0,0), new Vector2(0.25f,0.25f), new Vector2(0.25f,0), new Vector2(0,0.25f)}},
        {(Orientation)7, new Vector2[]{new Vector2(0.25f,0), new Vector2(0.5f,0.25f), new Vector2(0.5f,0), new Vector2(0.25f,0.25f)}},
        {(Orientation)0, new Vector2[]{new Vector2(0.5f,0), new Vector2(0.75f,0.25f), new Vector2(0.75f,0), new Vector2(0.5f,0.25f)}},
    };
        [System.Flags]
        public enum Orientation
        {
            None = 0, Top = 1, Left = 2, Down = 4, Right = 8
        }

    }

}