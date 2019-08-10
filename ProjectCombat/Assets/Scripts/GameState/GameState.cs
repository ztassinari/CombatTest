using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class GameState
{
	private const string SettingsRelativePath = "/settings";
	public static GameSettings Settings { get; private set; } = new GameSettings();



	static GameState()
	{
		LoadSettings();
	}



	public static GameSettings LoadSettings()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + SettingsRelativePath, FileMode.OpenOrCreate);
		try
		{
			Settings = bf.Deserialize(file) as GameSettings;
		} catch {}
		file.Close();
		return Settings;
	}
	public static void SaveSettings()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + SettingsRelativePath, FileMode.OpenOrCreate);
		try
		{
			bf.Serialize(file, Settings);
		} catch {}
		file.Close();
	}
}