using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnderMetricsTest : MonoBehaviour {


	void Start () {
		
		
		
		
		
	}


	string account_id ="";
	string child_id="";
	string session_token="";

	void OnGUI() {


		if (GUI.Button(new Rect(10, 10, 100, 30),"Initialize")){	
			//Initialize, 
			//Parametros: Initialize(string _appToken)
			EnderMetricsManager.Instance.Initialize("beb249d25f17a07f1f35d43ced92fdcb");
		}
		
		if (GUI.Button(new Rect(10, 45, 100, 30),"Register"))
		{
			//Llamado a servicio Register, 
			//Parametro : register(emDelegate _delegate = null)
			EnderMetricsManager.Instance.register("123456", RegisterDelegate);
		}

		if (GUI.Button(new Rect(10, 80, 100, 30),"Sync"))
		{
			//llamado a servicio sync (no implementado en web)
			EnderMetricsManager.Instance.sync(SyncDelegate);
		}

		if (GUI.Button(new Rect(10, 125, 100, 30),"Register Child"))
		{
			//llamado a registerChild 
			//Parametros : registerChild(string _nick, string _birthdate, string _gender, string _account_id, emDelegate _delegate = null)
			EnderMetricsManager.Instance.registerChild("prueba","22/07/2015","FEMALE",account_id,RegisterChildDelegate);

		}

		if (GUI.Button(new Rect(Screen.width-110, 10, 100, 30),"Get Child List"))
		{
			//lamado a getChildList
			//Parametros : getChildList(string _account_id, emDelegate _delegate = null)
			EnderMetricsManager.Instance.getChildList(account_id,GetChildLstDelegate);
		}

		if (GUI.Button(new Rect(Screen.width-110, 45, 100, 30),"Auth"))
		{
			//llamado a AUTH
			//Parametros: auth(string _account_id, string _child_id, emDelegate _delegate = null)
			EnderMetricsManager.Instance.auth(account_id, child_id,AuthDelegate);
		}

		if (GUI.Button (new Rect (Screen.width - 110, 80, 100, 30), "Add Hit")) 
		{
			//llamado a AddHit..
			//este metodo se debe llamar cada vez que se quiera registrar un hit
			//Parametros: addHit(string skill_token, string result)
			EnderMetricsManager.Instance.addHit("546c99d737dbb932c2185e43f305436f","SUCCESS");

		}

		if (GUI.Button (new Rect (Screen.width - 110, 115, 100, 30), "Track Set")) 
		{
		
			//llamado trackset
			//Parametros: trackSet(string _activity_token, int _level, string _result, int _time = 0, emDelegate _delegate = null)
			EnderMetricsManager.Instance.trackSet ("49689b6fecee50f5bb7317ac4db7ac9b", 1, "SUCCESS", TrackSetDelegate);
		}

		if (GUI.Button (new Rect (Screen.width - 110, 150, 100, 30), "Init Set")) 
		{

			//llamado InitSet
			//Parametros: Ninguno
			//Limpia lista de hits
			EnderMetricsManager.Instance.InitSet();
		}


	}

	public void RegisterDelegate(EnderMetricsManager.Requests pathID, string error, IDictionary response )
	{
		if(error == "OK")
		{
			IDictionary data = (IDictionary)response["data"];
			Debug.Log("data: " + data["account_id"]);
			account_id = (string) data["account_id"];

			Debug.Log("RegisterDelegate");
		}
	}

	public void SyncDelegate(EnderMetricsManager.Requests pathID, string error, IDictionary response )
	{
		Debug.Log("SyncDelegate");
	}

	public void RegisterChildDelegate(EnderMetricsManager.Requests pathID, string error, IDictionary response )
	{
		if(error == "OK")
		{
			IDictionary data = (IDictionary)response["data"];
			Debug.Log("data: " + data["child_id"]);
			child_id = (string) data["child_id"];

			Debug.Log("RegisterChildDelegate");
		}
	}

	public void GetChildLstDelegate(EnderMetricsManager.Requests pathID, string error, IDictionary response )
	{
		if(error == "OK")
		{
			IDictionary data = (IDictionary)response["data"];
			Debug.Log("data: " + data["child_id"]);
			child_id = (string) data["child_id"];
			
			Debug.Log("!!GetChildLstDelegate");
		}
	}

	public void AuthDelegate(EnderMetricsManager.Requests pathID, string error, IDictionary response )
	{
		if(error == "OK")
		{
			session_token = (string)response["session_token"];
			Debug.Log("AuthDelegate");
		}
	}

	public void TrackSetDelegate(EnderMetricsManager.Requests pathID, string error, IDictionary response )
	{
		Debug.Log("TrackSetDelegate");
	}

	// Update is called once per frame
	void Update () {
	
	}




}
