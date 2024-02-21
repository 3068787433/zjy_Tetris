using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 坐标扩展工具类
/// </summary>
public static class Vector3Extension
{
	/// <summary>
	/// 取整
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static Vector2 Round(this Vector3 value)
	{
		int x = Mathf.RoundToInt(value.x);
		int y = Mathf.RoundToInt(value.y);
		return new Vector2(x, y);
	}
}
