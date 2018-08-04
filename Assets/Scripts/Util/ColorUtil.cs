using UnityEngine;					// Access to Color class

// Contains utility methods for Unity Colors
public static class ColorUtil {

	public static readonly Color[] TROPHY_COLOR = {HexToColor("FFFFFF"), HexToColor("CD7F32"), HexToColor("BCC6CC"), HexToColor("FFD700")};
	public static readonly Color[] TILE_COLOR = {HexToColor("1F325B"), HexToColor("FFFFFF")};
	public static readonly Color HINT_COLOR = HexToColor("FDFF5B");

	// Used to covert a hex string into a Unity Color
	public static Color HexToColor(string hex) {
		hex = hex.Replace ("0x", ""); 	// In case the string is formatted 0xFFFFFF
		hex = hex.Replace ("#", ""); 	// In case the string is formatted #FFFFFF
		byte a = 255;					// Assume fully visible unless specified in hex
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);

		// Only use alpha if the string has enough characters
		if(hex.Length == 8){
			a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
		}
		
		return new Color32(r,g,b,a);
	}

}
