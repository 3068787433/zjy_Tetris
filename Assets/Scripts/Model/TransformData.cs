using System;
using UnityEngine;

/// <summary>
/// Transform数据类
/// </summary>
[Serializable]
public class TransformData
{
    public int i;
    public int j;
	public float x;
	public float y;
	public float z;
    public Color color;

	public TransformData() { }

	public TransformData(Transform transform)
	{
		this.x = transform.position.x;
		this.y = transform.position.y;
		this.z = transform.position.z;
		this.color = transform.GetComponent<SpriteRenderer>().color;
	}
}
