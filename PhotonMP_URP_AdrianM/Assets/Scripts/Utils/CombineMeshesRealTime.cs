using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineMeshesRealTime : MonoBehaviour
{
    private List<GameObject> meshes;

    void Start()
    {
        meshes = new List<GameObject>();
        StartThis();
    }

    internal void StartThis()
    {
        SearchDeleteOldMeshes();

        //Debug.Log(Time.time + "    CombineObjChildren           @@@@@@@         StartThis() " + transform.name);

        Matrix4x4 myTransform = transform.worldToLocalMatrix;
        Dictionary<string, List<CombineInstance>> combines = new Dictionary<string, List<CombineInstance>>();
        Dictionary<string, Material> namedMaterials = new Dictionary<string, Material>();
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (var meshRenderer in meshRenderers)
        {
            foreach (var material in meshRenderer.sharedMaterials)
            {
                if (material != null && !combines.ContainsKey(material.name))
                {
                    combines.Add(material.name, new List<CombineInstance>());
                    namedMaterials.Add(material.name, material);
                }
            }
        }

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        foreach (var filter in meshFilters)
        {
            //make this once !!!
            if (filter.GetComponent<BoxCollider>() != null)
            {
                filter.GetComponent<BoxCollider>().isTrigger = true;
            }
            //once

            if (filter.sharedMesh == null)
            {
                continue;
            }

            Renderer filterRenderer = filter.GetComponent<Renderer>();

            if (filterRenderer.name.Contains("_Built") == false)
            {
                filterRenderer.enabled = false;
                continue;
            }

            if (filterRenderer.sharedMaterial == null)
            {
                continue;
            }

            if (filterRenderer.sharedMaterials.Length > 1)
            {
                continue;
            }

            CombineInstance ci = new CombineInstance
            {
                mesh = filter.sharedMesh,
                transform = myTransform * filter.transform.localToWorldMatrix
            };

            combines[filterRenderer.sharedMaterial.name].Add(ci);

            filterRenderer.enabled = false;
        }

        foreach (Material m in namedMaterials.Values)
        {
            GameObject go = new GameObject("CombinedMesh_" + transform.name);
            go.transform.parent = transform.parent;
            go.transform.localPosition = transform.position;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;

            meshes.Add(go);

            MeshFilter filter = go.AddComponent<MeshFilter>();
            filter.mesh.CombineMeshes(combines[m.name].ToArray(), true, true);

            MeshRenderer arenderer = go.AddComponent<MeshRenderer>();
            arenderer.material = m;


            if (GetComponent<MeshCollider>() == null)
            {
                gameObject.AddComponent<MeshCollider>();
            }

            GetComponent<MeshCollider>().sharedMesh = null;
            GetComponent<MeshCollider>().convex = true;
            GetComponent<MeshCollider>().sharedMesh = filter.mesh;
        }
    }

    private void SearchDeleteOldMeshes()
    {
        for (int i = 0; i < meshes.Count; i++)
        {
            Destroy(meshes[i]);
        }

        meshes.Clear();
    }
}
