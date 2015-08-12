using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
public class EnderMetricsManager : MonoBehaviour {
	

	public enum Requests{
		REGISTER		=0,
		SYNC			=1,
		REGISTERCHILD	=2,
		DASHBOARD		=3,
		GETCHILDLIST 	=4,
		AUTH			=5,
		TRACKSET		=6,
		HITS			=7,
		count 			=8




	}

	string app_token = "";
	string format ="json";

	string url	 	="http://api.endermetrics.com/v1";
	string []paths = {
		"/account/register",
		"/account/sync",
		"/child/register",
		"/account/dashboard",
		"/child/getall",
		"/auth/token",
		"/track/set"
	};



	string account_id ="";
	string session_token = "";
	string child_id="";
	float timer = 0;

	ArrayList hits = new ArrayList();

	public delegate void emDelegate(Requests pathID, string error, IDictionary response );


	public emDelegate [] delegates= new emDelegate[(int) Requests.count];


	static EnderMetricsManager _instance = null;

	public static EnderMetricsManager Instance{

		get{

			if(_instance == null)
			{
				GameObject emManager = new GameObject("emManager");
				_instance = emManager.AddComponent<EnderMetricsManager>();


			}
			return _instance;
		}
	}

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void Initialize(string _appToken)
	{

		app_token = _appToken;

		Debug.Log("App Token Initialized");

	}
	/* Register
	*	Los parámetros que se han de pasar serán los siguientes:
	*	format[obligatorio string]	 -	Este parámetro sirve para saber como hemos de mandar los parámetros, que será en formato JSON o ARRAY
	*	app_token[obligatorio string]-	Es el token de la app que se obtiene en la plataforma una vez registrada. En el JSON deberá ir dentro de params
	*/

	public void register(string _custom_id="",emDelegate _delegate = null)
	{
		Dictionary<string,string> _options = new Dictionary<string,string>();

		_options.Add("app_token",app_token);


		if(_custom_id != "")
		{
			_options.Add("custom_id",_custom_id);
		}


		delegates[(int)Requests.REGISTER] = _delegate;
		StartCoroutine(invokeEnderMetrics (Requests.REGISTER,_options));

	}
	/*SYNC
	 * Parametros:
	 * session_token[required string}	- es el identificador de sesión que se obtiene al autenticar la cuenta: auth/token
	 * data[required string]			-  En data se puede enviar un array de sets para que sean registrados en una sola petición.
	 */ 

	public void sync(emDelegate _delegate = null )
	{

		Dictionary<string,string> _options = new Dictionary<string,string>();
		
		_options.Add("app_token",app_token);
		
		delegates[(int)Requests.REGISTER] = _delegate;
		StartCoroutine(invokeEnderMetrics (Requests.SYNC,_options));
	}


	/* DASHBOARD
	 * 
	 */ 

	public void dashboard(emDelegate _delegate = null)
	{
		
		Dictionary<string,string> _options = new Dictionary<string,string>();
		
		_options.Add("app_token",app_token);
		
		delegates[(int)Requests.DASHBOARD] = _delegate;
		StartCoroutine(invokeEnderMetrics (Requests.DASHBOARD,_options));
	}

	/*
	 *REGISTER CHILD
	 *Los parámetros a pasar serán los siguientes:
￼￼￼￼￼*￼￼format[obligatorio string]	-	Este parámetro sirve para saber como hemos de mandar los parámetros, que será en formato JSON o ARRAY
￼￼￼￼ *	nick[obligatorio string]	-	En este punto no debe registrarse el nombre auténtico del niño, es recomendable dejar esto claro e incluirlo en el documento de términos de uso de la aplicación.
￼￼￼￼￼*	birthdate[opcional string]	-	Fecha de nacimiento del niño. Esta fecha es necesaria para hacer aproximaciones entre diferentes niños de la misma edad. El formato de fecha será YYYY-MM-DD
￼￼￼￼￼*	gender[opcional string]		-	Género del niño registrado. Este dato es necesario para hacer aproximaciones dentro de la app.
￼￼￼￼ *	account_id[obligatorio string] -Id de la cuenta en la que el niño será registrado.
	 */

	public void registerChild(string _nick, string _birthdate, string _gender, string _account_id, emDelegate _delegate = null)
	{
		Dictionary<string,string> _options = new Dictionary<string,string>();
		
		_options.Add("app_token",app_token);
		_options.Add("nick",_nick);
		_options.Add("birthdate",_birthdate);
		_options.Add("gender",_gender);
		_options.Add("account_id",_account_id);


		delegates[(int)Requests.REGISTERCHILD] = _delegate;
		StartCoroutine(invokeEnderMetrics (Requests.REGISTERCHILD,_options));
		
	}

	/*
	 * Get Child List
	 * 
	 * 
	 */

	public void getChildList(string _account_id, emDelegate _delegate = null)
	{
		Dictionary<string,string> _options = new Dictionary<string,string>();
		
		//_options.Add("app_token",app_token);

		_options.Add("account_id",_account_id);

		delegates[(int)Requests.GETCHILDLIST] = _delegate;
		StartCoroutine(invokeEnderMetrics (Requests.GETCHILDLIST,_options));
		
	}


	/* AUTH TOKEN
	 * 
	 */

	public void auth(string _account_id, string _child_id, emDelegate _delegate = null)
	{
		Debug.Log("Auth _account_id:" +  _account_id + " child_id: " + _child_id);
		Dictionary<string,string> _options = new Dictionary<string,string>();
		
		_options.Add("app_token",app_token);
		
		_options.Add("account_id",_account_id);
		_options.Add("child_id",_child_id);

		delegates[(int)Requests.AUTH] = _delegate;
		StartCoroutine(invokeEnderMetrics (Requests.AUTH,_options));
		
	}


	/* InitSet
	 * 
	 */
	public void InitSet()
	{
		timer = Time.time;
		hits.Clear();
	}
	/*Hits
	 * Add & getHitsString
	 */ 
	public void addHit(string skill_token, string result)
	{
		hits.Add("\"skill_token\":\""+skill_token+"\",\"result\":\""+result+"\"");
		
	}

	string getHitsString ()
	{
		string hitsString = "[";

		for(int i = 0; i< hits.Count; i++)
		{
			hitsString += ((i>0)?",":"")+"{" + hits[i] + "}";

		}
		hitsString += "]";

		Debug.Log("Hits: " + hitsString);

		return hitsString;
	}

	/*TRACKSET
	 * 
	 */ 
	Dictionary<string,string> TrackSet_options=null;
	public void trackSet(string _activity_token, int _level, string _result, emDelegate _delegate = null)
	{
		Dictionary<string,string> _options = new Dictionary<string,string>();
		Dictionary<string,string> _data = new Dictionary<string,string>();
		string _hits = getHitsString();
		float _time = 0;
		hits.Clear();


		if(timer != 0)
			_time = Time.time- timer;

		_data.Add("activity_token",_activity_token);
		_data.Add("level",_level.ToString());
		_data.Add("result",_result);
		_data.Add("hits",_hits);
		_data.Add("time",_time.ToString());

		_options.Add ("session_token", session_token);
		_options.Add("data",getParamsString(_data));

		delegates[(int)Requests.TRACKSET] = _delegate;

		if(session_token != "")
			invokeTrackSet(_options);
		else
		{
			TrackSet_options = _options;
			auth(account_id,child_id,ts_AuthDelegate);
		}
	}

	private void ts_AuthDelegate(Requests pathID, string error, IDictionary response )
	{
		if(error == "OK")
		{
			Debug.Log("ts_AuthDelegate");
			invokeTrackSet(TrackSet_options);
			TrackSet_options = null;
		}
	}

	private void invokeTrackSet(Dictionary<string,string> _options)
	{

		Debug.Log("invokeTrackSet - session : " + session_token);
		_options["session_token"] = session_token;
		StartCoroutine(invokeEnderMetrics (Requests.TRACKSET,_options));
	}

	IEnumerator invokeEnderMetrics(Requests pathID , Dictionary<string,string> _options)
	{
		string _parameters = getParamsString(_options);

		WWWForm form = new WWWForm();
		form.AddField("format", format);
		form.AddField("params", _parameters);

		Debug.Log("parameters: " + _parameters);

		string _urlRequest = url+paths[(int)pathID];
		//Debug.Log("url Request " + _urlRequest);
		WWW w = new WWW(_urlRequest, form);
		yield return w;
		
		
		if (!string.IsNullOrEmpty(w.error)) {
			print(w.error);

			handleError(w.error, pathID);
		}
		else {
			print("Finished: "+ w.text);

			IDictionary response = (IDictionary) Json.Deserialize(w.text);

			handlerResponse(response, pathID);
		}



	}

	void handlerResponse(IDictionary response, Requests pathID )
	{

		IDictionary data = null;
		switch(pathID)
		{
		case Requests.REGISTER:
			data = (IDictionary)response["data"];
			Debug.Log("data: " + data["account_id"]);
			account_id = (string) data["account_id"];
			break;

		case Requests.REGISTERCHILD:
			data = (IDictionary)response["data"];
			Debug.Log("data: " + data["child_id"]);
			child_id = (string) data["child_id"];
			break;

		case Requests.GETCHILDLIST:
			break;
		case Requests.AUTH:
			Debug.Log("session_token: " + response["session_token"]);
			session_token = (string)response["session_token"];
			break;

		}

		if(delegates[(int)pathID] != null)
			delegates[(int)pathID](pathID,"OK", response);

	}

	void handleError (string error, Requests pathID)
	{

		Debug.Log("Error  : " + error + " En request "+ pathID.ToString());
		if(delegates[(int)pathID] != null)
			delegates[(int)pathID](pathID, error, null);
	}

	string getParamsString( Dictionary<string,string> _options)
	{
		string _parameters="";


		foreach(KeyValuePair<string, string> param in _options)
		{
			string _key = "\""+param.Key + "\"";
			string _value = param.Value;

		if(!param.Key.Equals("data") && !param.Key.Equals("hits"))
				_value = "\""+_value+"\"";		


			_parameters += ((_parameters!="")?",":"")+ 
				_key+":"+ _value;
			
		}
		_parameters = "{" + _parameters + "}";

		
		return _parameters;
		
	}



	          
}
