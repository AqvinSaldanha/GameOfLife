using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Pool of alive cells (Gameobjects representing alive cells)
/// </summary>
public class CellsPool : MonoBehaviour
{
	
	Queue<GameObject> m_cellGameObjects;
	public GameObject m_cellGameObject;

	// game objects currently in use
	List<GameObject> m_activeCells;
 

	void Awake ()
	{
		m_activeCells = new List<GameObject> ();
		m_cellGameObjects = new Queue<GameObject> (); 

		// Instantiate 50 objects in the beginning
		for (int i = 0; i < 50; i++) {
			GameObject newCell = Instantiate (m_cellGameObject);
			newCell.transform.parent = transform;
			newCell.SetActive (false);
			m_cellGameObjects.Enqueue (newCell);
		}
	}

	/// <summary>
	/// Deactivate all the active gameobjects and return them to the pool (queue)
	/// </summary>
	public void DisableAll ()
	{ 
		foreach (GameObject cell in m_activeCells) { 
			cell.SetActive (false);
			m_cellGameObjects.Enqueue (cell);
		}
		m_activeCells.Clear ();
	}

	/// <summary>
	/// Deactivate gameobject at Position and return them to the pool (queue)
	/// </summary>
	/// <param name="position">Position.</param>
	public void DisableAt(Vector2 position)
	{
		foreach (GameObject cell in m_activeCells) {
			if (cell.transform.position.x == position.x && cell.transform.position.y == position.y) {
				cell.gameObject.SetActive (false);
				m_cellGameObjects.Enqueue (cell);
				m_activeCells.Remove (cell);
				break;
			}
		}
	}


	/// <summary>
	/// Get game object from pool(dequeue) and place it in position also add it to active cells list
	/// </summary>
	/// <param name="position">Position.</param>
	public void Enable (Vector2 position)
	{ 
		if (m_cellGameObjects.Count > 0) {
			GameObject cell = m_cellGameObjects.Dequeue ();
			cell.transform.position = position;
			cell.SetActive (true);
			m_activeCells.Add (cell);
		} else {
			GameObject newCell = Instantiate (m_cellGameObject, position, Quaternion.identity) as GameObject;
			newCell.transform.parent = transform;
			newCell.SetActive (true);
			m_activeCells.Add (newCell);
		}

	}


}
