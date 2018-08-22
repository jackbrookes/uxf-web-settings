using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

namespace UXF_Web_Settings
{
    public class WebSessionSetup : MonoBehaviour
    {
        public UXF.Session session;
        public string experimentName;
        // data goes Project folder (editor) or next to .exe file (build)
        public string saveFolderName = "UXF_Data"; 
        public string diskSettingsName = "web_settings.json";
        public SettingsURL settingsUrl;
        
        [Space]
        public WebUIController webUI;

        Dictionary<string, object> settings = null;

        string DiskSettingsPath
        {
            get { return Path.Combine(Application.streamingAssetsPath, diskSettingsName); }
        }

        string SaveFolderPath
        {
            get
            {
                string saveFolderPath = UXF.Extensions.CombinePaths(Application.dataPath, "..", saveFolderName);
                if (!Directory.Exists(saveFolderPath))
                    Directory.CreateDirectory(saveFolderPath);

                return saveFolderPath;
            }
        }

        void Awake()
        {
            StartCoroutine(SetupSettings(settingsUrl.url));
        }

        public void SetupSession(Dictionary<string, object> participantInfo)
        {
            string ppid = NewShortGUID();

            participantInfo.Add("ppid", ppid);
            participantInfo.Add("unique_device_id", SystemInfo.deviceUniqueIdentifier);
            participantInfo.Add("datetime", System.DateTime.Now.ToString());
            participantInfo.Add("in_unity_editor", Application.isEditor);

            session.Begin(
                experimentName,
                ppid,
                SaveFolderPath,
                1,
                participantDetails: participantInfo,
                settings: new UXF.Settings(settings)
            );

            session.WriteDictToSessionFolder(participantInfo, "participant_info");
        }

        IEnumerator SetupSettings(string attemptUrl)
        {
            UnityWebRequest www = UnityWebRequest.Get(attemptUrl);

            // yield until web request has finished
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
                settings = GetFallbackSettings();
                webUI.SendReady();
                yield break;
            }

            string jsonString;
            try
            {
                jsonString = www.downloadHandler.text;
                if (jsonString == string.Empty) throw new InvalidOperationException("Bad request");
                settings = (Dictionary<string, object>)MiniJSON.Json.Deserialize(jsonString);
            }
            catch (Exception e)
            {
                jsonString = string.Empty;
                settings = GetFallbackSettings();
                Debug.LogException(e);
            }

            if (jsonString != string.Empty)
            {
                // if all is good, update fallback settings file with newest online version
                Debug.LogFormat("Succesfully downloaded and parsed settings from {0}", attemptUrl);
                File.WriteAllText(DiskSettingsPath, jsonString);
            }
            webUI.SendReady();

        }


        Dictionary<string, object> GetFallbackSettings()
        {
            string jsonString = File.ReadAllText(DiskSettingsPath);
            return (Dictionary<string, object>)MiniJSON.Json.Deserialize(jsonString);
        }

        public static string NewShortGUID()
        {
            return Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "_");
        }

    }

}
