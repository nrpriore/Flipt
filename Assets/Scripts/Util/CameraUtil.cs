using UnityEngine;					// Access to Camera class

// Contains utility methods for Unity Cameras 
public static class CameraUtil {

	// Sets ortho camera size and position for input width (assumes tile = 1 unit with center pivot 
	public static void SizeToWidth(int width) {
		float aspect = (float)Screen.width / Screen.height;
		float tempOrtho = width / (2f * aspect);
		float camY = tempOrtho - 0.5f;
		camY -= ((tempOrtho * 2f) - Mathf.Floor(tempOrtho * 2f)) / 2f;
		float camX = (tempOrtho * aspect) - 0.5f;

		Camera.main.transform.localPosition = new Vector3(camX, camY, -10f);
		Camera.main.orthographicSize = tempOrtho * 1.1f;  // 10% buffer
	}

}
