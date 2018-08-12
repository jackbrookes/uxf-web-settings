using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UXF_Web_Settings
{
    public class WebUIController : MonoBehaviour
    {
		[SerializeField]
        private List<UXF.FormElementEntry> _participantDataPoints = new List<UXF.FormElementEntry>();
        /// <summary>
        /// List of datapoints you want to collect per participant. 
        /// </summary>
        public List<UXF.FormElementEntry> participantDataPoints { get { return _participantDataPoints; } }
		public List<Toggle> mustAgreeOptions;
        public UXF.FillableFormController ppInfoForm;
		public WebSessionSetup webSessionSetup;
		public Button startButton;

        void Start()
        {
			ppInfoForm.Generate(participantDataPoints, false); 
        }

		public void TrySetupSession()
		{
            var infoForm = ppInfoForm.GetCompletedForm();
			if (infoForm == null)
			{
				Debug.LogWarning("Form not complete correctly!");
				return;
			}

			bool agreeAll = true;
			foreach (var option in mustAgreeOptions)
			{
				bool agree = (bool) option.isOn;
				agreeAll &= agree;
			}
			
			if (!agreeAll)
			{
                Debug.LogWarning("Did not agree to requirements!");
				return;
			}
			// disable UI
			gameObject.SetActive(false);
            webSessionSetup.SetupSession(infoForm);
		}

		public void SendReady()
		{
			startButton.interactable = true;
		}

    }

}