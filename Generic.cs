using UnityEngine;
using MathHelper;

namespace CameraHelper
{
	public static class Generic
	{
		public static Vector2 FrustumAt
		(this Camera camera,
		float distance)
		{
			float height =
				2.0f *
				distance *
				Mathf.Tan(
					camera.fieldOfView *
					0.5f *
					Mathf.Deg2Rad
				);
				
			return new(
				height * camera.aspect,
				height
			);
		}

		public static Vector2 MousePositionInWorldSpace2D
		(this Camera camera,
		Vector3 mousePosition) =>
			camera.ScreenToWorldPoint(new(
				mousePosition.x,
				mousePosition.y,
				mousePosition.z - camera.transform.position.z
			));

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
		/// <param name="worldPosition">
		/// The target position in world space.
		/// </param>
		/// <param name="canvasRect">
		/// Use this as reference of the canvas' screen.
		/// </param>
		public static Vector2 WorldToCanvasPoint
		(this Camera camera,
		Canvas canvas,
		RectTransform canvasTransform,
		Vector3 worldPosition,
		out Rect canvasRect)
		{
			Vector2 position = camera.WorldToScreenPoint(worldPosition);
			Rect canvasPixelRect = canvas.pixelRect;
			canvasRect = canvasTransform.rect;
			
			return new(
				// Raw World-to-Screen Position
				position.x *
				// CanvasScaler Aspect Ratio Correction
				canvasRect.width / canvasPixelRect.width -
				// CanvasScaler Offset Correction (Move to Center)
				canvasRect.width * 0.5f,
				
				position.y *
				canvasRect.height / canvasPixelRect.height -
				canvasRect.height * 0.5f
			);
		}
		
		/// <summary>
		/// Clamp world space canvas to screen space camera canvas.
		/// </summary>
		public static Vector2 ClampWSCToSSCC
		(this Camera camera,
		Vector2 worldPointWSC,
		RectTransform rtWSC,
		Canvas sscc,
		RectTransform rtSSCC)
		{
			Vector2 size = rtWSC.rect.size;
			Vector2 pivot = rtWSC.pivot;
			Rect rectSSCC = rtSSCC.rect;

			Vector2 canvasPoint =
				camera.WorldToCanvasPoint(
					sscc,
					rtSSCC,
					worldPointWSC
				);
			
			return
				camera.CanvasToWorldPoint(
					sscc,
					rtSSCC,
					new(
						Mathf.Clamp(
							canvasPoint.x,
							rectSSCC.x + pivot.x * size.x,
							-(
								rectSSCC.x +
								(1f - pivot.x) * size.x
							)
						),
						Mathf.Clamp(
							canvasPoint.y,
							rectSSCC.y + pivot.y * size.y,
							-(
								rectSSCC.y +
								(1f - pivot.y) * size.y
							)
						),
						-camera.transform.position.z
					)
				);
		}
		
		/// <summary>
		/// Clamp world space canvas to screen space camera canvas.
		/// </summary>
		public static Vector2 ClampWSCToSSCC
		(this Camera camera,
		RectTransform rtWSC,
		Canvas sscc,
		RectTransform rtSSCC)
		{
			Vector2 size = rtWSC.rect.size;
			Vector2 pivot = rtWSC.pivot;
			Rect rectSSCC = rtSSCC.rect;

			Vector2 canvasPoint =
				camera.WorldToCanvasPoint(
					sscc,
					rtSSCC,
					rtWSC.transform.position
				);
			
			return
				camera.CanvasToWorldPoint(
					sscc,
					rtSSCC,
					new(
						Mathf.Clamp(
							canvasPoint.x,
							rectSSCC.x + pivot.x * size.x,
							-(
								rectSSCC.x +
								(1f - pivot.x) * size.x
							)
						),
						Mathf.Clamp(
							canvasPoint.y,
							rectSSCC.y + pivot.y * size.y,
							-(
								rectSSCC.y +
								(1f - pivot.y) * size.y
							)
						),
						-camera.transform.position.z
					)
				);
		}
		
		public static Vector2 WorldToCanvasPoint
		(this Camera camera,
		Canvas canvas,
		RectTransform canvasTransform,
		Vector3 worldPosition) =>
			WorldToCanvasPoint(
				camera,
				canvas
				.CanvasToRectRatio(canvasTransform),
				canvasTransform.rect.size,
				worldPosition
			);
		
		public static Vector2 WorldToCanvasPoint
		(this Camera camera,
		Vector2 canvasRatio,
		Vector2 canvasSize,
		Vector3 worldPosition)
		{
			Vector2 position =
				camera.WorldToScreenPoint(worldPosition);
			
			return new(
				// Raw World-to-Screen Position
				position.x *
				// CanvasScaler Aspect Ratio Correction
				canvasRatio.x -
				// CanvasScaler Offset Correction (Move to Center)
				canvasSize.x * 0.5f,
				
				position.y *
				canvasRatio.y -
				canvasSize.y * 0.5f
			);
		}

		public static Vector2 CanvasToWorldPoint
		(this Camera camera,
		Canvas canvas,
		RectTransform canvasTransform,
		Vector3 target) =>
			CanvasToWorldPoint(
				camera,
				canvas
				.CanvasToRectRatio(canvasTransform),
				canvasTransform.rect.size,
				target
			);

		public static Vector2 CanvasToWorldPoint
		(this Camera camera,
		Vector2 canvasRatio,
		Vector2 canvasSize,
		Vector3 target) =>
			camera.ScreenToWorldPoint(new(
				(target.x + canvasSize.x * 0.5f) / canvasRatio.x,
				(target.y + canvasSize.y * 0.5f) / canvasRatio.y,
				target.z
			));
	}
}
