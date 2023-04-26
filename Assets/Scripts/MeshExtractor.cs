using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshExtractor : MonoBehaviour
{
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) {
            CombineInstance[] bigCombine = new CombineInstance[(int)CountriesEnum.ZMB + 1];
            int j = 0;
            DisplayCountryNames.Instance.countryADM3s.ForEach(
                (string ADM3) => {
                    EarthEngineCountry eac = EarthEngineCountryController.Instance.GetCountryByADM3(ADM3);

                    CombineInstance[] combine = new CombineInstance[eac.gameObject.GetComponentsInChildren<LineRenderer>().Length];

                    int i = 0;
                    new List<LineRenderer>(eac.gameObject.GetComponentsInChildren<LineRenderer>()).ForEach(
                        (LineRenderer lr) => {
                            MeshFilter mf = lr.gameObject.AddComponent<MeshFilter>();
                            lr.BakeMesh(mf.mesh, Camera.main, true);
                            combine[i].mesh = mf.sharedMesh;
                            combine[i++].transform = lr.gameObject.transform.localToWorldMatrix;
                        });

                    GameObject go = new GameObject(eac.countryName);

                    Mesh mesh = go.AddComponent<MeshRenderer>().gameObject
                        .AddComponent<MeshFilter>().mesh = new Mesh();
                    mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                    mesh.CombineMeshes(combine);
                    Debug.Log(eac.countryName + ": " + mesh.vertexCount);

                    bigCombine[j].mesh = go.GetComponent<MeshFilter>().sharedMesh;
                    bigCombine[j++].transform = go.transform.localToWorldMatrix;
                });

            Mesh bigMesh = new GameObject()
                        .AddComponent<MeshRenderer>().gameObject
                        .AddComponent<MeshFilter>().mesh = new Mesh();
            bigMesh.CombineMeshes(bigCombine);
            AssetDatabase.CreateAsset(bigMesh, "Assets/Prefabs/Country Borders/allBorders.asset");
            AssetDatabase.SaveAssets();
        }
    }*/
}