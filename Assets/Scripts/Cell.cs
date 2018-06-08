using UnityEngine; 

/// <summary>
/// Represents each cell in the cells grid.
/// </summary>
public class Cell
{ 
	public Vector2 m_position;

	public bool IsAlive{ get; set; }

	public Cell (Vector2 newPosition)
	{
		this.m_position = newPosition;
		IsAlive = false;
	}
	
}
