using UnityEngine;
using System.Collections;

public class GoogleAPI : MonoBehaviour {

	string url = "";
	float lat;
	float lon;
	LocationInfo myLocation;
	WWW www;

	// Use this for initialization
	void Start () {
		myLocation = new LocationInfo();
		//lat = myLocation.latitude;
		//lon = myLocation.longitude;

		lat = 100;
		lon = 100;


		//url ="http://maps.google.com/maps/api/staticmap?center=" + lat + "," + lon + "&zoom=14&size=800x600&maptype=hybrid&sensor=true";
		url ="http://maps.google.com/maps/api/staticmap?center="+ "36.6177238,127.4836622" +"&zoom=13&size=800x600&maptype=hybrid&sensor=true";
		StartCoroutine( Myyield(url) );

		Debug.Log( lat.ToString() );
		Debug.Log( lon.ToString() );
	}

	IEnumerator Myyield(string url)
	{
		this.www = new WWW(url);

		while(!www.isDone )
		{
			yield return new WaitForSeconds(1);
		}
		Texture2D tex = www.texture;
		if( tex )
		{
			Debug.Log( tex.ToString() );
		}
		GetComponent<MeshRenderer>().material.mainTexture = www.texture;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
