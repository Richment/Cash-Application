namespace LightSwitchApplication
{
	public enum ProcessVisibility : int
	{
		/// <summary>
		/// The window will be invisible.
		/// </summary>
		Hidden = 0,	  
		/// <summary>
		/// The window will be shown as normal.
		/// </summary>
		Normal = 1,
		/// <summary>
		/// The window will be shown minimized.
		/// </summary>
		Minimized = 2,
		/// <summary>
		/// The window will be shown maximized.
		/// </summary>
		Maximized = 3,
		/// <summary>
		/// The window will be shown at its recent position and size. The actual window remains active.
		/// </summary>
		RecentBackground = 4,
		/// <summary>
		/// The window will be shown at its current position and size.
		/// </summary>
		Current = 5,
		/// <summary>
		/// The window will be shown minimized. The actual window remains active.
		/// </summary>
		MinimizedBackground = 7,
		/// <summary>
		/// The window will be shown maximized. The actual window remains active.
		/// </summary>
		ApplicationDefault = 10
	};
}
