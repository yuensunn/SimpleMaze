using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public class MaterialCombineInstance
    {
        public Material m_material;
        public List<CombineInstance> m_combineInstances = new List<CombineInstance>();
        public MaterialCombineInstance(Material material)
        {
            m_material = material;
        }
        public void AddCombineInstance(CombineInstance combineInstance)
        {
            m_combineInstances.Add(combineInstance);
        }
    }
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshCombiner : MonoBehaviour
    {

        void Start()
        {
            CombineMesh(gameObject);
        }


        /*
        public static void CombineMesh(GameObject target)
        {
            Vector3 position = target.transform.position;
            target.transform.position = Vector3.zero;
            Dictionary<string, MaterialCombineInstance> combinedMesh = new Dictionary<string, MaterialCombineInstance>();
            MeshFilter[] meshFilters = target.GetComponentsInChildren<MeshFilter>();

            for (int i = 0; i < meshFilters.Length; i++)
            {
                if (meshFilters[i].transform != target.transform)
                {
                    MeshFilter currentMeshFilter = meshFilters[i];
                    MeshRenderer currentMeshRenderer = currentMeshFilter.GetComponent<MeshRenderer>();
                    CombineInstance combine = new CombineInstance();
                    string materialName = currentMeshRenderer.material.name.Replace(" (Instance)", "");

                    combine.mesh = currentMeshFilter.mesh;
                    combine.transform = currentMeshFilter.transform.localToWorldMatrix;

                    if (!combinedMesh.ContainsKey(materialName))
                    {
                        combinedMesh.Add(materialName, new MaterialCombineInstance(currentMeshRenderer.material));
                    }
                    combinedMesh[materialName].AddCombineInstance(combine);
                    currentMeshFilter.gameObject.SetActive(false);
                }
            }

            Debug.Log(combinedMesh.Keys.Count);
            List<CombineInstance> totalMesh = new List<CombineInstance>();
            List<Material> totalMaterial = new List<Material>();
            foreach (KeyValuePair<string, MaterialCombineInstance> kvp in combinedMesh)
            {
                Mesh newMesh = new Mesh();
                newMesh.CombineMeshes(kvp.Value.m_combineInstances.ToArray());
                CombineInstance combineInstnace = new CombineInstance();
                combineInstnace.mesh = newMesh;
                combineInstnace.transform = target.transform.localToWorldMatrix;
                totalMesh.Add(combineInstnace);
                totalMaterial.Add(kvp.Value.m_material);
            }

            Mesh combinedAllMesh = new Mesh();
            combinedAllMesh.CombineMeshes(totalMesh.ToArray(), false);
            MeshFilter thisMeshFilter = target.GetComponent<MeshFilter>();
            MeshRenderer thisMeshRenderer = target.GetComponent<MeshRenderer>();

            thisMeshFilter.mesh = combinedAllMesh;
            thisMeshRenderer.materials = totalMaterial.ToArray();

            target.transform.position = position;
        }
        */
        public static void CombineMesh(GameObject target, bool destory = false)
        {

            if (target.GetComponent<MeshFilter>() == null)
            {
                target.AddComponent<MeshFilter>();
                target.AddComponent<MeshRenderer>();
            }
            Vector3 position = target.transform.position;
            target.transform.position = Vector3.zero;
            Dictionary<Material, List<CombineInstance>> combinedMesh = new Dictionary<Material, List<CombineInstance>>();
            MeshFilter[] meshFilters = target.GetComponentsInChildren<MeshFilter>();

            for (int i = meshFilters.Length - 1; i >= 0; i--)
            {
                if (meshFilters[i].transform != target.transform)
                {
                    MeshFilter currentMeshFilter = meshFilters[i];
                    MeshRenderer currentMeshRenderer = currentMeshFilter.GetComponent<MeshRenderer>();
                    CombineInstance combine = new CombineInstance();


                    combine.mesh = currentMeshFilter.sharedMesh;
                    combine.transform = currentMeshFilter.transform.localToWorldMatrix;

                    if (!combinedMesh.ContainsKey(currentMeshRenderer.sharedMaterial))
                    {
                        combinedMesh.Add(currentMeshRenderer.sharedMaterial, new List<CombineInstance>());
                    }
                    combinedMesh[currentMeshRenderer.sharedMaterial].Add(combine);

                    if (!destory) currentMeshFilter.gameObject.SetActive(false);
                    else DestroyImmediate(currentMeshFilter.gameObject);

                }

                /*
                for (int x = target.transform.childCount-1; x >= 0; x--)
                {

                    if (target.transform.GetChild(i).GetComponent<MeshRenderer>() == null)
                    {
                        DestroyImmediate(target.transform.GetChild(i));
                    }
                }*/
            }
            List<CombineInstance> totalMesh = new List<CombineInstance>();
            List<Material> totalMaterial = new List<Material>();
            foreach (KeyValuePair<Material, List<CombineInstance>> kvp in combinedMesh)
            {
                Mesh newMesh = new Mesh();
                newMesh.CombineMeshes(kvp.Value.ToArray());
                CombineInstance combineInstnace = new CombineInstance();
                combineInstnace.mesh = newMesh;
                combineInstnace.transform = target.transform.localToWorldMatrix;
                totalMesh.Add(combineInstnace);
                totalMaterial.Add(kvp.Key);
            }

            Mesh combinedAllMesh = new Mesh();
            combinedAllMesh.CombineMeshes(totalMesh.ToArray(), false);
            MeshFilter thisMeshFilter = target.GetComponent<MeshFilter>();
            MeshRenderer thisMeshRenderer = target.GetComponent<MeshRenderer>();

            thisMeshFilter.mesh = combinedAllMesh;
            thisMeshRenderer.materials = totalMaterial.ToArray();

            target.transform.position = position;

        }

        public static void CombineMeshGameObjecs(GameObject target, bool destroy = true)
        {
            Vector3 position = target.transform.position;
            target.transform.position = Vector3.zero;
            Dictionary<Material, List<CombineInstance>> combinedMesh = new Dictionary<Material, List<CombineInstance>>();
            MeshFilter[] meshFilters = target.GetComponentsInChildren<MeshFilter>();

            for (int i = meshFilters.Length - 1; i >= 0; i--)
            {
                if (meshFilters[i].transform != target.transform)
                {
                    MeshFilter currentMeshFilter = meshFilters[i];
                    MeshRenderer currentMeshRenderer = currentMeshFilter.GetComponent<MeshRenderer>();
                    CombineInstance combine = new CombineInstance();


                    combine.mesh = currentMeshFilter.sharedMesh;
                    combine.transform = currentMeshFilter.transform.localToWorldMatrix;

                    if (!combinedMesh.ContainsKey(currentMeshRenderer.sharedMaterial))
                    {
                        combinedMesh.Add(currentMeshRenderer.sharedMaterial, new List<CombineInstance>());
                    }
                    combinedMesh[currentMeshRenderer.sharedMaterial].Add(combine);

                    if (!destroy) currentMeshFilter.gameObject.SetActive(false);
                    else DestroyImmediate(currentMeshFilter.gameObject);

                }
            }

            foreach (KeyValuePair<Material, List<CombineInstance>> kvp in combinedMesh)
            {
                Mesh newMesh = new Mesh();
                newMesh.CombineMeshes(kvp.Value.ToArray());
                GameObject go = new GameObject();
                go.transform.parent = target.transform;
                go.AddComponent<MeshFilter>().mesh = newMesh;
                go.AddComponent<MeshRenderer>().material = kvp.Key;
            }

        }

        public static void CombineMultipleMesh(GameObject target, bool destory = false)
        {
            Vector3 position = target.transform.position;
            target.transform.position = Vector3.zero;
            Dictionary<Material, List<CombineInstance>> combinedMesh = new Dictionary<Material, List<CombineInstance>>();
            MeshFilter[] meshFilters = target.GetComponentsInChildren<MeshFilter>();

            for (int i = meshFilters.Length - 1; i >= 0; i--)
            {
                if (meshFilters[i].transform != target.transform)
                {
                    MeshFilter currentMeshFilter = meshFilters[i];
                    MeshRenderer currentMeshRenderer = currentMeshFilter.GetComponent<MeshRenderer>();
                    CombineInstance combine = new CombineInstance();


                    combine.mesh = currentMeshFilter.sharedMesh;
                    combine.transform = currentMeshFilter.transform.localToWorldMatrix;

                    if (!combinedMesh.ContainsKey(currentMeshRenderer.sharedMaterial))
                    {
                        combinedMesh.Add(currentMeshRenderer.sharedMaterial, new List<CombineInstance>());
                    }
                    combinedMesh[currentMeshRenderer.sharedMaterial].Add(combine);

                    if (!destory) currentMeshFilter.gameObject.SetActive(false);
                    else DestroyImmediate(currentMeshFilter.gameObject);

                }
            }

            Mesh newMesh = new Mesh();
            List<Material> totalMaterial = new List<Material>();
            int triangleCount = 0;
            List<CombineInstance> toBeAdded = new List<CombineInstance>();
            foreach (KeyValuePair<Material, List<CombineInstance>> kvp in combinedMesh)
            {
                List<CombineInstance> ci = kvp.Value;
                totalMaterial.Add(kvp.Key);
                for (int i = 0; i < ci.Count; i++)
                {
                    if (triangleCount + ci[i].mesh.triangles.Length < 65000)
                    {
                        toBeAdded.Add(ci[i]);
                        triangleCount += ci[i].mesh.triangles.Length;
                    }
                    else
                    {
                        Debug.Log(triangleCount);
                        newMesh.CombineMeshes(toBeAdded.ToArray());
                        GameObject newObject = new GameObject();
                        newObject.transform.parent = target.transform;
                        newObject.transform.localPosition = Vector3.zero;
                        newObject.transform.localScale = Vector3.one;
                        newObject.transform.localRotation = Quaternion.identity;

                        newObject.AddComponent<MeshFilter>().mesh = newMesh;
                        newObject.AddComponent<MeshRenderer>().materials = totalMaterial.ToArray();
                        newMesh = new Mesh();
                        triangleCount = 0;
                        toBeAdded = new List<CombineInstance>();
                        totalMaterial = new List<Material>();
                        if (i < ci.Count - 1)
                        {
                            totalMaterial.Add(kvp.Key);
                        }

                        toBeAdded.Add(ci[i]);
                        triangleCount += ci[i].mesh.triangles.Length;
                    }
                }

            }


            /*List<CombineInstance> totalMesh = new List<CombineInstance>();
            List<Material> totalMaterial = new List<Material>();
            foreach (KeyValuePair<Material, List<CombineInstance>> kvp in combinedMesh)
            {
                Mesh newMesh = new Mesh();
                newMesh.CombineMeshes(kvp.Value.ToArray());
                GameObject child = new GameObject();
                child.transform.parent = target.transform;
                child.transform.localScale = Vector3.one;
                child.transform.localRotation = Quaternion.identity;
                child.AddComponent<MeshFilter>().mesh = newMesh;
                child.AddComponent<MeshRenderer>().material = kvp.Key;
            }
            return;
            Mesh combinedAllMesh = new Mesh();
            combinedAllMesh.CombineMeshes(totalMesh.ToArray(), false);
            MeshFilter thisMeshFilter = target.GetComponent<MeshFilter>();
            MeshRenderer thisMeshRenderer = target.GetComponent<MeshRenderer>();

            thisMeshFilter.mesh = combinedAllMesh;
            thisMeshRenderer.materials = totalMaterial.ToArray();

            target.transform.position = position;*/
        }


        public static void DisplayVertices(GameObject target)
        {
            Debug.Log((target.GetComponent<MeshFilter>().mesh.triangles).ToString());
        }
        public static void DisplayTriangles(GameObject target)
        {
            Debug.Log(target.GetComponent<MeshFilter>().mesh.triangles.Length);
        }
    }
}