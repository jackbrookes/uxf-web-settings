using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "settings_url", menuName = "UXF_S3/SettingsURL", order = 3)]

public class SettingsURL : ScriptableObject
{	
	[TextArea]
	public string url = "http://example.com/settings.json";

}
