using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public static class SaveSystem
{
	static string path = Application.persistentDataPath + @"\SaveData.xml";
	public static void SaveGame(SaveInfo info)
	{
		using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
		{
			XmlSerializer serializer = new XmlSerializer(typeof(SaveInfo));
			serializer.Serialize(stream, info);
		}
	}

	public static SaveInfo LoadGame()
	{

		using (FileStream stream = new FileStream(path, FileMode.Open))
		{
			XmlSerializer ser = new XmlSerializer(typeof(SaveInfo));
			try
			{
				return (SaveInfo)ser.Deserialize(stream);
			}
			catch (System.Exception e)
			{
				if (e is System.InvalidCastException)
				{
					Debug.LogError("SaveSystem.LoadGame(): Saved data not correct type!\n" + e.Message);
				}
				else
				{
					Debug.LogError("SaveSystem.LoadGame(): Unforeseen exception generated!\n" + e.Message);
				}
				return null;
			}
		}
	}

}

public class SaveInfo
{
	int levelIndex, checkpointIndex;
}
