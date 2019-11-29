using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class VertexFollow : MonoBehaviour
{
	[Header("Cloth")]
	public bool targetSelf;
	public Transform clothTransformTarget;
	public Cloth clothTarget;

	[Space]
	[Header("Attachment")]
	public Transform attachmentTransform;

	public int clothTargetVertex;

	[Space]
	[Header("Editor")]

	[SerializeField] private bool showVerts = true;
	[SerializeField] private bool showGizmosUnselected = true;
	[Range(0.0f, 1.0f)]
	[SerializeField] private float color;
	[SerializeField] private float cubeSize = 0.01f;
	[SerializeField] private float normalRaySize = 0.05f;

	private Transform startTransform;

	void Start()
	{
		if (targetSelf)
		{
			clothTransformTarget = GetComponent<Transform>();
			clothTarget = GetComponent<Cloth>();
		}
	}

	void Update()
	{
		Vector3 vert = GetVertPosition(clothTargetVertex);
		attachmentTransform.position = vert;
	}

	Vector3 GetVertPosition(int vertIndex)
	{
		vertIndex = Mathf.Clamp(vertIndex, 0, clothTarget.vertices.Length - 1);
		return clothTransformTarget.position + clothTransformTarget.TransformDirection(clothTarget.vertices[vertIndex]);
	}
	Vector3 GetVertEndDrawPosition(int vertIndex, float size)
	{
		return clothTransformTarget.position + clothTransformTarget.TransformDirection(clothTarget.vertices[vertIndex]) + clothTransformTarget.TransformDirection(clothTarget.normals[vertIndex]) * size;
	}

#if UNITY_EDITOR
	
	void OnDrawGizmosSelected()
	{
		if (!showGizmosUnselected && showVerts && clothTransformTarget != null && clothTarget != null) { DrawGizmos(); }
	}
	void OnDrawGizmos()
	{
		if (showGizmosUnselected && showVerts && clothTransformTarget != null && clothTarget != null) { DrawGizmos(); }
	}

	void DrawGizmos()
	{
		Color rgb = Color.HSVToRGB(color, 0.75f, 1f,false);
		for (int i = 0; i < clothTarget.vertices.Length; i++)
		{
			Vector3 vertPosition = GetVertPosition(i);
			Vector3 vertNormalLineEnd = GetVertEndDrawPosition(i, normalRaySize);
						
			Gizmos.color = rgb + new Color(0f,0f,0f,-0.4f);
			Gizmos.DrawCube(vertPosition, Vector3.one * cubeSize);

			Gizmos.color = rgb + new Color(0f, 0f, 0f, -0.7f);
			Gizmos.DrawLine(vertPosition, vertNormalLineEnd);
		}

		Vector3 selectedVertPos = GetVertPosition(clothTargetVertex);
		Vector3 selectedVertNormalDrawEndPos = GetVertEndDrawPosition(clothTargetVertex, normalRaySize * 1.5f);

		Gizmos.color = Color.white;

		Gizmos.DrawCube(selectedVertPos, Vector3.one * cubeSize * 1.5f);
		Gizmos.DrawLine(selectedVertPos, selectedVertNormalDrawEndPos);
	}
#endif
}