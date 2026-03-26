using UnityEngine;
using System.Collections.Generic;

public class BMFontImporter : MonoBehaviour
{
	public Font customFont;
	public Texture2D fontTexture;
	public TextAsset fntFile;

	void Start()
	{
		try
		{
			ImportFont();
		}
		catch (System.Exception ex)
		{
			Debug.LogError("BMFontImporter error: " + ex.Message + "\n" + ex.StackTrace);
		}
	}

	void ImportFont()
	{
		if (fntFile == null || fontTexture == null || customFont == null)
		{
			Debug.LogError("BMFontImporter: fntFile, fontTexture, and customFont must be assigned.");
			return;
		}

		List<CharacterInfo> characterInfoList = new List<CharacterInfo>();
		string[] lines = fntFile.text.Split('\n');

		// Parse the common line to get the baseline (base parameter)
		int baseline = 0;
		foreach (string line in lines)
		{
			string trimmedLine = line.Trim();
			if (trimmedLine.StartsWith("common "))
			{
				string[] parts = trimmedLine.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
				Dictionary<string, int> commonValues = new Dictionary<string, int>();
				foreach (string part in parts)
				{
					string[] kv = part.Split('=');
					if (kv.Length == 2)
					{
						int value;
						if (int.TryParse(kv[1], out value))
						{
							commonValues[kv[0]] = value;
						}
					}
				}
				if (commonValues.ContainsKey("base"))
				{
					baseline = commonValues["base"];
				}
				break;
			}
		}

		foreach (string line in lines)
		{
			string trimmedLine = line.Trim();
			if (trimmedLine.Length == 0 || !trimmedLine.StartsWith("char "))
				continue;

			string[] parts = trimmedLine.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
			Dictionary<string, int> values = new Dictionary<string, int>();

			foreach (string part in parts)
			{
				string[] kv = part.Split('=');
				if (kv.Length == 2)
				{
					int parsedValue;
					if (int.TryParse(kv[1], out parsedValue))
					{
						values[kv[0]] = parsedValue;
					}
				}
			}

			// Ensure required keys exist
			if (!values.ContainsKey("id") || !values.ContainsKey("x") || !values.ContainsKey("y") ||
				!values.ContainsKey("width") || !values.ContainsKey("height"))
				continue;

			int id = values["id"];
			int x = values["x"];
			int y = values["y"];
			int width = values["width"];
			int height = values["height"];
			int xoffset = values.ContainsKey("xoffset") ? values["xoffset"] : 0;
			int yoffset = values.ContainsKey("yoffset") ? values["yoffset"] : 0;
			int xadvance = values.ContainsKey("xadvance") ? values["xadvance"] : width;

			CharacterInfo charInfo = new CharacterInfo();
			charInfo.index = id;

			// Calculate UV coordinates (Unity's UV origin is bottom-left)
			float texWidth = fontTexture.width;
			float texHeight = fontTexture.height;
			charInfo.uvBottomLeft = new Vector2((float)x / texWidth, 1f - (float)(y + height) / texHeight);
			charInfo.uvTopRight = new Vector2((float)(x + width) / texWidth, 1f - (float)y / texHeight);

			// Calculate character geometry using the baseline
			charInfo.minX = xoffset;
			charInfo.maxX = xoffset + width;
			// Using baseline to compute vertical bounds:
			charInfo.maxY = baseline - yoffset;
			charInfo.minY = charInfo.maxY - height;
			charInfo.advance = xadvance;

			characterInfoList.Add(charInfo);
		}

		if (characterInfoList.Count == 0)
		{
			Debug.LogError("BMFontImporter: No valid character data found. Check the .fnt file format.");
			return;
		}

		customFont.characterInfo = characterInfoList.ToArray();
		Debug.Log("BMFontImporter: Successfully imported " + characterInfoList.Count + " characters.");
	}
}
