using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JellyVerticeInfo : MonoBehaviour
{
    public Transform originTransform;
    public Vector3 originLocalPosition;
}

public class JellyScript : MonoBehaviour {

    MeshFilter meshFilter;
    Mesh mesh;
    Vector3[] originVertices;
    Vector3[] calculateVertices;

    List<GameObject> vertexBoxList = new List<GameObject>();

    public Vector3 testLocation = new Vector3( 0, 2, 0 );
    
	// Use this for initialization
	void Start () {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter)
        {
            mesh = meshFilter.mesh;
            originVertices = mesh.vertices;   
            calculateVertices = mesh.vertices;

            for (int vIndex = 0; vIndex < mesh.vertexCount; ++vIndex)
            {
                GameObject go = new GameObject();
                go.name = "Test" + vIndex;
                go.transform.parent = gameObject.transform;

                go.transform.localPosition = originVertices[vIndex];
                go.AddComponent<BoxCollider>().size = new Vector3(0.01f, 0.01f, 0.01f);
                

                JellyVerticeInfo jellyInfo = go.AddComponent<JellyVerticeInfo>();
                //jellyInfo.transform.parent = go.transform.parent;
                jellyInfo.originLocalPosition = go.transform.localPosition;

                Rigidbody body = go.AddComponent<Rigidbody>();
                body.useGravity = false;
                //body.isKinematic = true;
                body.angularDrag = 0;

                vertexBoxList.Add( go );
            }
            //mesh.vertices = originVertices;
            meshFilter.mesh = mesh;
        }
        else
        {
            Debug.Log( gameObject.name + "no meshFilter" );
        }
        MeshCollider meshcollider = GetComponent<MeshCollider>();
        if( meshcollider != null ) 
            meshcollider.sharedMesh = mesh;
	}
	
	// Update is called once per frame
	void Update () {
        
        //transform.position += (testLocation - transform.position) * Time.deltaTime;

        shapeMemory();
        mesh.vertices = calculateVertices;
	}

    void shapeMemory()
    {
        for (int vIndex = 0; vIndex < mesh.vertexCount; ++vIndex)
        {
            Rigidbody body = vertexBoxList[vIndex].GetComponent<Rigidbody>();
            if (body)
            {
                Vector3 result = (vertexBoxList[vIndex].GetComponent<JellyVerticeInfo>().originLocalPosition - vertexBoxList[vIndex].transform.localPosition);
                body.AddForce((result * (Physics.gravity.magnitude )) * Time.deltaTime, ForceMode.Impulse);
            }

            calculateVertices[vIndex] = vertexBoxList[vIndex].transform.localPosition;
            Debug.Log(vertexBoxList[vIndex].GetComponent<JellyVerticeInfo>().originLocalPosition);
            Debug.DrawLine(vertexBoxList[vIndex].transform.position, vertexBoxList[vIndex].GetComponent<JellyVerticeInfo>().transform.position, Color.red, 1000);
        }
        
    }
}

