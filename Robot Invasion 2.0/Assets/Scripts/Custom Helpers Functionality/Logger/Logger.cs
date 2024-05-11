using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

[System.Serializable]
public class Log {
	public bool _showLog;
	public string _prefix;
	public Color _prefixColor;
	[Tooltip("Currently just for you to know which Logger is this")]
	public string logName;
	public string _hexColor;
}

public class Logger : MonoBehaviour {
	public Log[] _loggers;


	private void OnValidate() {
		for (int i = 0; i < _loggers.Length; i++)
		{
			// the line bellow: Updates the hex color code from the provided color for each logger
			_loggers[i]._hexColor = ($"#{ColorUtility.ToHtmlStringRGBA(_loggers[i]._prefixColor)}");
		}
	}

	public void Log(Object sender, string msg, int logIndex) {

		int i = logIndex;
		int listI = i - 1;

		// the line below: checks if the index passed on by the caller is valid / not above the logger count or not
		if (i > _loggers.Length)
		{
			Debug.LogError($"{i} isn't in bound");
			return;
		}

		// the line below: checks if the that specific logger doesn't want messages Logged or not
		if (!_loggers[listI]._showLog) return; 

		// the line below: does the actuall logging of the message that was passed in as a param
		Debug.Log($"<color={_loggers[listI]._hexColor}>{_loggers[listI]._prefix}</color>: {msg}", sender); 

	}
}