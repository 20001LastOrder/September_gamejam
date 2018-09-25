using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Request : MonoBehaviour {
	public static IEnumerator Get(string url, Action<Dictionary<string,string>> callback) {
		using (UnityWebRequest www = UnityWebRequest.Get(url)) {
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError) {
				Debug.LogError(www.error);
			} else {
				if (callback != null) {
					callback.Invoke(JsonConvert.DeserializeObject<Dictionary<string, string>>(www.downloadHandler.text));
				}
			}
		}
	}
}
