using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace com.aqvin.conway
{
	/// <summary>
	/// This is the main class. It creates and maintains the world and the cells in it.
	/// </summary>
	public class GridManager : MonoBehaviour
	{
		/// <summary>
		/// This is used to keep a list of alive cells
		/// </summary>
		struct Index
		{
			public int x;
			public int y;

			public Index (int newX, int newY)
			{
				x = newX;
				y = newY;
			}
		}


		/// Temporary list that stores the index of the alive cells.
		List<Index> m_aliveCellsIndex;


		public int m_height, m_width;

		// This represents the main grid or world
		List<List<Cell>> m_cells;

		// Reference of alive game object pool
		public CellsPool m_cellsPool;

		// Reference to border prefab
		public GameObject m_border;

		// This is used to maintain speed of simulation
		float m_repeatSpeed = 1f;

		/* 
		 * These are used to restrict the number of loopings
		 * These variables keep positions of first and last alive cells in the grid 
		 * This helps in reduced cpu usage in smaller simulations
		 * */
		int m_first_Cell_X_index, m_first_Cell_Y_index;
		int m_last_Cell_X_index, m_last_Cell_Y_Index;

		void OnEnable ()
		{
			// Add UI event handlers
			HUD.StartClicked += StartSimulation;
			HUD.PauseClicked += PauseSimulation;
			HUD.ClearClicked += ClearCells;
			HUD.ChangeSpeed += ChangeSpeed;
			HUD.ShowPattern += SetPattern;
		}

		void OnDisable ()
		{
			// remove UI event handlers
			HUD.StartClicked -= StartSimulation;
			HUD.PauseClicked -= PauseSimulation;
			HUD.ClearClicked -= ClearCells;
			HUD.ChangeSpeed -= ChangeSpeed;
			HUD.ShowPattern -= SetPattern;
		}

		/// <summary>
		/// Update the speed of simulation.
		/// </summary>
		/// <param name="value">Value from the slider</param>
		void ChangeSpeed (float value)
		{ 
			m_repeatSpeed = 1f - value;
			if (m_repeatSpeed == 0) {
				m_repeatSpeed = 0.05f;
			}
		}
			
		/// <summary>
		/// Starts the simulation.
		/// </summary>
		void StartSimulation ()
		{
			Invoke ("ProcessCells", m_repeatSpeed);
		}

		/// <summary>
		/// Pauses the simulation.
		/// </summary>
		void PauseSimulation ()
		{
			CancelInvoke ();
		}

		/// <summary>
		/// Clears the cells.
		/// </summary>
		void ClearCells ()
		{
			ResetCells ();
		}

		/// <summary>
		/// In this method the world is created.
		/// </summary>
		void Start ()
		{

			// initialize these positions to be in the center of the world
			m_first_Cell_X_index = m_width / 2;
			m_last_Cell_X_index = m_width / 2;
			m_first_Cell_Y_index = m_height / 2;
			m_last_Cell_Y_Index = m_height / 2;


			m_aliveCellsIndex = new List<Index> ();


			// create 2D list to maintain the cells
			m_cells = new List<List<Cell>> (m_width * m_height);

			// Get center of the screen in world dimention
			Vector3 startPos = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width / 2, Screen.height / 2, 0));

			// Estimate the starting position of the world (Botton Left)
			// Subtract 0.5 to get center (position) of the first cell 
			startPos -= new Vector3 ((m_width / 2) - 0.5f, (m_height / 2) - 0.5f, 0);

			// Start populating the cell grid.
			float x = startPos.x;
			float y = startPos.y;
			for (int i = 0; i < m_height; i++) {
				List<Cell> newRow = new List<Cell> ();
				// create a row and populate it with cells
				for (int j = 0; j < m_width; j++) {
					Cell newCell = new	Cell (new Vector2 (x, y));
					newRow.Add (newCell);
					x++;
				}
				// Add the new row to the cell grid
				m_cells.Add (newRow);
				y++;
				x = startPos.x;
			}


			// These below lines create border around the grid
			GameObject borderLeft = Instantiate (m_border, new Vector3 (startPos.x, 0, 0), Quaternion.identity) as GameObject;
			borderLeft.transform.localScale = new Vector3 (1, m_height, 1);
			borderLeft.GetComponent<SpriteRenderer> ().color = Color.red;


			GameObject borderRight = Instantiate (m_border, new Vector3 (startPos.x + m_width - 1, 0, 0), Quaternion.identity) as GameObject;
			borderRight.transform.localScale = new Vector3 (1, m_height, 1);
			borderRight.GetComponent<SpriteRenderer> ().color = Color.red;



			GameObject borderBottom = Instantiate (m_border, new Vector3 (0, startPos.y, 0), Quaternion.identity) as GameObject;
			borderBottom.transform.localScale = new Vector3 (m_width, 1, 1);
			borderBottom.GetComponent<SpriteRenderer> ().color = Color.red;


			GameObject borderTop = Instantiate (m_border, new Vector3 (0, startPos.y + m_height - 1, 0), Quaternion.identity) as GameObject;
			borderTop.transform.localScale = new Vector3 (m_width, 1, 1);
			borderTop.GetComponent<SpriteRenderer> ().color = Color.red;


			// These below lines create a black background
			GameObject fill = Instantiate (m_border, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
			fill.transform.localScale = new Vector3 (m_width, m_height, 1);
			fill.GetComponent<SpriteRenderer> ().color = Color.black;


		}

		/// <summary>
		/// Clear Screen and create the new pattern specified by index
		/// </summary>
		/// <param name="index">Index</param>
		void SetPattern (int index)
		{
			ClearCells ();
			switch (index) {
			case 0: 
				SetPattern0 ();
				break;
			case 1: 
				SetPattern1 ();
				break;
			case 2: 
				SetPattern2 ();
				break;
			case 3: 
				SetPattern3 ();
				break;
			case 4: 
				SetPattern4 ();
				break;
			case 5: 
				SetPattern5 ();
				break;
			case 6: 
				SetPattern6 ();
				break;
			} 
		}

		/// <summary>
		/// Plus Sign
		/// </summary>
		void SetPattern0 ()
		{
			int x = m_width / 2;
			int y = m_height / 2;
			x = x - 1;
			y = y - 1;

			MakeAlive (x, y);
			MakeAlive (x, y + 1);
			MakeAlive (x, y + 2); 
		}

		/// <summary>
		/// R pattern
		/// </summary>
		void SetPattern1 ()
		{
			int x = m_width / 2;
			int y = m_height / 2;
			x = x - 1;
			y = y - 1;

			MakeAlive (x, y);

			MakeAlive (x + 1, y);
			MakeAlive (x + 1, y + 1);
			MakeAlive (x + 1, y - 1);

			MakeAlive (x + 2, y + 1);

		}

		/// <summary>
		/// Glider
		/// </summary>
		void SetPattern2 ()
		{
			int x = m_width / 2;
			int y = m_height / 2;
			x = x - 1;
			y = y - 1;

			MakeAlive (x, y);
			MakeAlive (x + 1, y - 1);
			MakeAlive (x + 1, y - 2);
			MakeAlive (x, y - 2);
			MakeAlive (x - 1, y - 2);
		}

		/// <summary>
		/// Wierd virus like pattern
		/// </summary>
		void SetPattern3 ()
		{
			int x = m_width / 2;
			int y = m_height / 2;
			x = x - 1;
			y = y - 1;

			for (int i = 0; i < 5; i++) {
				MakeAlive (x, y + i);
				MakeAlive (x + 4, y + i);
			}

			MakeAlive (x + 2, y + 4);
			MakeAlive (x + 2, y);

		}

		/// <summary>
		/// Synchronous pattern
		/// </summary>
		void SetPattern4 ()
		{
			int x = m_width / 2;
			int y = m_height / 2;
			x = x - 1;
			y = y - 1;

			MakeAlive (x, y);
			MakeAlive (x, y + 1);
			MakeAlive (x, y + 2);
			MakeAlive (x + 1, y);

			MakeAlive (x + 6, y);
			MakeAlive (x + 6, y + 1);
			MakeAlive (x + 6, y + 2);
			MakeAlive (x + 5, y);

			for (int i = 0; i < 5; i++) {
				MakeAlive (x + 2, y + 1 + i);
				MakeAlive (x + 4, y + 1 + i);
			}

			MakeAlive (x + 1, y + 4);
			MakeAlive (x + 1, y + 5);

			MakeAlive (x + 5, y + 4);
			MakeAlive (x + 5, y + 5);

		}

		/// <summary>
		/// 10 straight blocks in 2 sets.
		/// </summary>
		void SetPattern5 ()
		{
			int x = (m_width / 2) - 20;
			int y = m_height / 2;
			x = x - 1;
			y = y - 1;

			for (int i = 0; i < 10; i++) {
				MakeAlive (x + i, y);
				MakeAlive (x + 20 + i, y + 8);
			}
		}


		/// <summary>
		/// Popular infinite gun pattern.
		/// </summary>
		void SetPattern6 ()
		{
			// Please use a graph to trace it.
			int x = (m_width / 2) - 15;
			int y = m_height / 2;
			x = x - 1;
			y = y - 1;

			MakeAlive (x, y);
			MakeAlive (x, y + 1);
			MakeAlive (x + 1, y);
			MakeAlive (x + 1, y + 1);

			
			MakeAlive (x + 8, y);
			MakeAlive (x + 8, y - 1);

			MakeAlive (x + 9, y - 1);
			MakeAlive (x + 9, y + 1);

			MakeAlive (x + 10, y);
			MakeAlive (x + 10, y + 1);


			MakeAlive (x + 16, y - 1);
			MakeAlive (x + 16, y - 2);
			MakeAlive (x + 16, y - 3);

			MakeAlive (x + 17, y - 1);

			MakeAlive (x + 18, y - 2);

			MakeAlive (x + 22, y + 1);
			MakeAlive (x + 22, y + 2);

			MakeAlive (x + 23, y + 1);
			MakeAlive (x + 23, y + 3);

			MakeAlive (x + 24, y + 2);
			MakeAlive (x + 24, y + 3);

			MakeAlive (x + 24, y - 9);
			MakeAlive (x + 24, y - 10);

			MakeAlive (x + 25, y - 9);
			MakeAlive (x + 25, y - 11);

			MakeAlive (x + 26, y - 9);


			MakeAlive (x + 34, y + 2);
			MakeAlive (x + 34, y + 3);

			MakeAlive (x + 35, y + 2);
			MakeAlive (x + 35, y + 3);


			MakeAlive (x + 35, y - 4);
			MakeAlive (x + 35, y - 5);
			MakeAlive (x + 35, y - 6);

			MakeAlive (x + 36, y - 4);

			MakeAlive (x + 37, y - 5);

		}

		/// <summary>
		/// Just read mouse input to add/remove cell
		/// </summary>
		void Update ()
		{
			if (Input.GetMouseButtonDown (0)) {

				if (EventSystem.current.IsPointerOverGameObject ()) {
					return;
				}

				Vector2 newCellPos = Input.mousePosition;

				if (newCellPos.y > Screen.height - Screen.height / 10) {
					return;
				} 

				newCellPos = Camera.main.ScreenToWorldPoint (newCellPos);

				int newX, newY;
				newX = Mathf.FloorToInt (newCellPos.x);
				newY = Mathf.FloorToInt (newCellPos.y);

				newX = (m_width / 2) + newX;
				newY = (m_height / 2) + newY;


				if (newX > m_width - 1 || newY > m_height - 1 || newX < 1 || newY < 1) {
					// On Border
					return;
				}
	 
				if (!m_cells [newY] [newX].IsAlive) {
					MakeAlive (newX, newY);
				} else { 
					m_cells [newY] [newX].IsAlive = false;
					m_cellsPool.DisableAt (m_cells [newY] [newX].m_position);
				} 

			}
		}

		/// <summary>
		/// Make the cell at the specifies position alive. Also get a cell gameobject to represent an alive cell
		/// </summary>
		/// <param name="widthIndex">Horizontal index.</param>
		/// <param name="heightIndex">Vertical index.</param>
		void MakeAlive (int widthIndex, int heightIndex)
		{
			m_cells [heightIndex] [widthIndex].IsAlive = true;

			m_cellsPool.Enable (m_cells [heightIndex] [widthIndex].m_position);

			if (m_first_Cell_X_index > widthIndex) {
				m_first_Cell_X_index = widthIndex;
			}

			if (m_last_Cell_X_index < widthIndex) {
				m_last_Cell_X_index = widthIndex;
			}

			if (m_first_Cell_Y_index > heightIndex) {
				m_first_Cell_Y_index = heightIndex;
			}

			if (m_last_Cell_Y_Index < heightIndex) {
				m_last_Cell_Y_Index = heightIndex;
			}

		}

		/// <summary>
		/// Recreates the 'alive' cells.  
		/// </summary>
		void RecreateAliveCells ()
		{
			m_cellsPool.DisableAll ();
			m_first_Cell_X_index = m_width;
			m_last_Cell_X_index = 0;
			m_first_Cell_Y_index = m_height;
			m_last_Cell_Y_Index = 0;

			foreach (Index i in m_aliveCellsIndex) {
				MakeAlive (i.x, i.y);
			}
			print (m_first_Cell_X_index + " " + m_last_Cell_X_index + "  " + m_first_Cell_Y_index + " " + m_last_Cell_Y_Index);
		}



		/// <summary>
		/// This handles the main logic:
		/// Calculate all the cells that will remain alive in the next round (OR next generation) and store in aliveCellIndex list
		/// Kill all the cells
		/// Make all the cells that are also listed in aliveCellIndex alive.
		/// This way we can ignore checking death conditions for each cells
		/// </summary>
		void ProcessCells ()
		{ 
			// clear the alive list
			m_aliveCellsIndex.Clear ();
	 
			if (m_first_Cell_X_index < 3)
				m_first_Cell_X_index = 3;

			if (m_last_Cell_X_index >= m_width - 2)
				m_last_Cell_X_index = m_width - 3;

			if (m_first_Cell_Y_index < 3)
				m_first_Cell_Y_index = 3;

			if (m_last_Cell_Y_Index >= m_height - 2)
				m_last_Cell_Y_Index = m_height - 3;


			// Get count of alive neighbour for each cell in the alive cells area
			for (int i = m_first_Cell_Y_index - 2; i < m_last_Cell_Y_Index + 2; i++) {
				for (int j = m_first_Cell_X_index - 2; j < m_last_Cell_X_index + 2; j++) {
					int count = 0;
					count = GetNeighbourAliveCount (j, i); 
					// Check if the current cell becomes alive in the below conditions
					if (count == 3 && !m_cells [i] [j].IsAlive) {
						m_aliveCellsIndex.Add (new Index (j, i));
					}
					if ((count == 2 || count == 3) && m_cells [i] [j].IsAlive) {
						m_aliveCellsIndex.Add (new Index (j, i));
					} 
				}
			}
	 
			// Make all cells in the main cell grid dead
			ResetCells ();

			// Only the cells listed in aliveCellsIndex are made alive
			RecreateAliveCells ();

			// Invoke next cycle
			StartSimulation ();
		}


		/// <summary>
		/// Make all cells in the main cell grid dead
		/// </summary>
		void ResetCells ()
		{
			m_cellsPool.DisableAll ();
			if (m_first_Cell_X_index < 3)
				m_first_Cell_X_index = 3;

			if (m_last_Cell_X_index >= m_width - 2)
				m_last_Cell_X_index = m_width - 3;

			if (m_first_Cell_Y_index < 3)
				m_first_Cell_Y_index = 3;

			if (m_last_Cell_Y_Index >= m_height - 2)
				m_last_Cell_Y_Index = m_height - 3;


			for (int i = m_first_Cell_Y_index - 2; i < m_last_Cell_Y_Index + 2; i++) {
				for (int j = m_first_Cell_X_index - 2; j < m_last_Cell_X_index + 2; j++) {
					m_cells [i] [j].IsAlive = false;
				}
			}
		}

		/// <summary>
		/// Get count of all the neighbouring cells that are alive
		/// </summary>
		/// <returns>The number of neighbours that are alive.</returns>
		/// <param name="widthIndex">Horizontal index of current cell.</param>
		/// <param name="heightIndex">Vertical index of current cell.</param>
		int GetNeighbourAliveCount (int widthIndex, int heightIndex)
		{
			int count = 0;
	 
			// iterate through all the 8 cells around the cell.
			for (int i = heightIndex - 1; i <= heightIndex + 1; i++) {
				for (int j = widthIndex - 1; j <= widthIndex + 1; j++) {
					if (i == heightIndex && j == widthIndex) {
						// do nothing 
					} else if (m_cells [i] [j].IsAlive) {
						count++;
					} 
				} 
			} 
			return count;
		}
	 
	}
}
