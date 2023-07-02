using UnityEngine;

namespace CameraHelper
{
	public static class Generic
	{
		public static Vector2 MousePositionInWorldSpace2D
		(this Camera camera)
		{
			Vector3 position = Input.mousePosition;
			
			return camera.ScreenToWorldPoint(new(
				position.x,
				position.y,
				position.z - camera.transform.position.z
			));
		}
		
		/// <summary>
		/// `WorldToScreenPoint` but it works with screen space camera.
		/// <br/>
		/// The resulting value is anchored to the center of the screen
		/// <br/>
		/// ([0, 0] at center.)
		/// </summary>
		/// <param name="camera">
		/// The main camera.
		/// </param>
		/// <param name="canvas">
		/// The UI canvas.
		/// </param>
		/// <param name="canvasTransform">
		/// The UI canvas' RectTransform.
		/// </param>
		/// <param name="target">
		/// The target position in world space.
		/// </param>
		/// <param name="canvasTransformRect">
		/// Use this as reference of the canvas' screen.
		/// </param>
		public static Vector2 WorldToCanvasPoint
		(this Camera camera,
		Canvas canvas,
		RectTransform canvasTransform,
		Vector3 target,
		out Rect canvasTransformRect)
		{
			Vector2 position = camera.WorldToScreenPoint(target);
			Rect canvasRect = canvas.pixelRect;
			canvasTransformRect = canvasTransform.rect;
			
			return new(
				// Raw World-to-Screen Position
				position.x *
				// CanvasScaler Aspect Ratio Correction
				canvasTransformRect.width / canvasRect.width -
				// CanvasScaler Offset Correction (Move to Center)
				canvasTransformRect.width * 0.5f,
				
				position.y *
				canvasTransformRect.height / canvasRect.height -
				canvasTransformRect.height * 0.5f
			);
		}
	}
}
