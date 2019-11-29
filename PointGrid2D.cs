using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PointGrid2D : MonoBehaviour {
	
	[Header("Size Settings")]
	public Vector2 size;
	public Vector2Int gridSize;

	//[Header("Color Settings")]
	private Color outlineColor = new Color(1f, 0f, 0f, 0.5f);
	private Color gridlineColor = new Color(1f, 1f, 1f, 0.5f);
	private Color gridPointColor = new Color(0f, 1f, 1f, 1f);

	//[Header("Measurement Settings")]
	private float gridPointSize = 0.1f;
	private float outlineOffset = 0.05f;
	private float gridlineSize = 2f;
		
	[SerializeField] private bool showGizmosUnselected;

	[HideInInspector] public Vector3 upLeftCorner;
	[HideInInspector] public Vector3 upRightCorner;
	[HideInInspector] public Vector3 downLeftCorner;
	[HideInInspector] public Vector3 downRightCorner;

	[HideInInspector] public List<Vector3> gridPointList;

	private float halfX;
	private float halfY;

	private int gridX;
	private int gridY;

	private float xPos;
	private float yPos;
	private float zPos;

	void Start ()
	{
	}

	void Update()
	{
		halfX = size.x / 2f;
		halfY = size.y / 2f;
		
		gridX = gridSize.x;
		gridY = gridSize.y;

		xPos = transform.position.x;
		yPos = transform.position.y;
		zPos = transform.position.z;

		UpdateCorners();
		UpdateDivisions();		

	}

	void UpdateCorners()
	{
		upLeftCorner =    new Vector3(xPos - halfX, yPos + halfY, zPos);
		upRightCorner =   new Vector3(xPos + halfX, yPos + halfY, zPos);
		downLeftCorner =  new Vector3(xPos - halfX, yPos - halfY, zPos);
		downRightCorner = new Vector3(xPos + halfX, yPos - halfY, zPos);
	}

	void UpdateDivisions()
	{
		gridPointList.Clear();
		gridPointList.TrimExcess();
				
		if (gridSize.y > 0)
			for (int i = 0; i <= gridY; i++)
			{
				float newY = Mathf.Lerp(downLeftCorner.y, upLeftCorner.y, (1f / gridY) * i);

				if (gridSize.x > 0)
					for (int ii = 0; ii <= gridX; ii++)
					{
						float newX = Mathf.Lerp(upLeftCorner.x, upRightCorner.x, (1f / gridX) * ii);
						gridPointList.Capacity++;
						gridPointList.Add(new Vector3(newX, newY, zPos));
					}
			}
	}

#if UNITY_EDITOR
	void OnDrawGizmosSelected()
	{
		if (!showGizmosUnselected) { DrawGizmos(); }
	}
	void OnDrawGizmos()
	{
		if (showGizmosUnselected) { DrawGizmos(); }
	}

	void DrawGizmos()
	{
		Gizmos.color = outlineColor;

		//draw outline

		Gizmos.DrawLine(OffsetCorners(upLeftCorner), OffsetCorners(upRightCorner));
		Gizmos.DrawLine(OffsetCorners(upRightCorner), OffsetCorners(downRightCorner));
		Gizmos.DrawLine(OffsetCorners(downRightCorner), OffsetCorners(downLeftCorner));
		Gizmos.DrawLine(OffsetCorners(downLeftCorner), OffsetCorners(upLeftCorner));

		//draw grid lines

		Handles.color = gridlineColor;

		if (gridSize.x > 0)
			for (int i = 0; i <= gridX; i++)
			{
				float newX = Mathf.Lerp(upLeftCorner.x, upRightCorner.x, (1f / gridX) * i);
				Handles.DrawDottedLine(new Vector3(newX, yPos + halfY, zPos), new Vector3(newX, yPos - halfY, zPos), gridlineSize);
			}

		if (gridSize.y > 0)
			for (int i = 0; i <= gridY; i++)
			{
				float newY = Mathf.Lerp(downLeftCorner.y, upLeftCorner.y, (1f / gridY) * i);
				Handles.DrawDottedLine(new Vector3(xPos + halfX, newY, zPos), new Vector3(xPos - halfX, newY, zPos), gridlineSize);
			}

		//draw points

		Gizmos.color = gridPointColor;

		for (int i = 0; i < gridPointList.Capacity; i++)
		{
			Gizmos.DrawCube(gridPointList[i], new Vector3(gridPointSize, gridPointSize, gridPointSize));
		}
	}
#endif
	Vector3 OffsetCorners(Vector3 input)
	{
		float x;
		float y;

		if (input.x >= xPos) { x = input.x + outlineOffset; }
		else
		{
			x = input.x - outlineOffset;
		}

		if (input.y >= yPos) { y = input.y + outlineOffset; }
		else
		{
			y = input.y - outlineOffset;
		}
		return new Vector3(x, y, input.z);
	}

}
