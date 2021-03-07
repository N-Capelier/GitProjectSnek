using UnityEngine;

namespace Map
{
    /// <summary>
    /// Nico
    /// </summary>
    public class MapGrid : Singleton<MapGrid>
    {
        [Header("Variables")]
        [SerializeField] int width;
        [SerializeField] int height;
        [Range(0, 10)] public float cellSize = 1;
        int[,] cells;

        [Header("Display")]
        [SerializeField] bool displayGridLines = false;
        Clock refreshTimer;
        float refreshRate = 2f;

        #region Private methods

        private void Awake()
        {
            //CreateSingleton(true);
        }

        private void Start()
        {
            cells = new int[width, height];

            refreshTimer = new Clock(refreshRate);
            refreshTimer.ClockEnded += RefreshLines;
            RefreshLines();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                RefreshLines();
            }
        }

        private void OnDestroy()
        {
            refreshTimer.ClockEnded -= RefreshLines;
        }

        void RefreshLines()
        {
            if (!displayGridLines)
                return;
            Vector3 _offset = new Vector3(cellSize, 0, cellSize) * 0.5f;

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int z = 0; z < cells.GetLength(1); z++)
                {
                    Debug.DrawLine(GetWorldPos(x, z) - _offset, GetWorldPos(x, z + 1) - _offset, Color.white, refreshRate);
                    Debug.DrawLine(GetWorldPos(x, z) - _offset, GetWorldPos(x + 1, z) - _offset, Color.white, refreshRate);
                }
            }
            Debug.DrawLine(GetWorldPos(0, height) - _offset, GetWorldPos(width, height) - _offset, Color.white, refreshRate);
            Debug.DrawLine(GetWorldPos(width, 0) - _offset, GetWorldPos(width, height) - _offset, Color.white, refreshRate);

            refreshTimer.SetTime(refreshRate);
        }

        #endregion

        public Vector3 GetWorldPos(int x, int z)
        {
            return new Vector3(x + transform.position.x, transform.position.y, z + transform.position.z) * cellSize;
        }
    }
}