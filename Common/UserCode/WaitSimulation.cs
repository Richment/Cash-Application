namespace Common
{
	using System;
	using System.Diagnostics;
	using System.Threading;

	/// <summary>
	/// Notifies a waiting thread that an event has occurred. This class cannot be inherited. 
	/// </summary>
	public sealed class WaitSimulation : IDisposable
	{
		private sealed class StateObject
		{
			public bool TimedOut;
		};

		private readonly object syncLock;
		private bool signal, waiting, disposed;

		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
		/// </value>
		public bool IsDisposed
		{
			[DebuggerStepThrough]
			get
			{
				return disposed;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WaitSimulation"/> class.
		/// </summary>
		[DebuggerHidden]
		public WaitSimulation()
		{
			syncLock = new Object();
		}
		/// <summary>
		/// Finalizes an instance of the <see cref="WaitSimulation"/> class.
		/// </summary>
		[DebuggerHidden]
		~WaitSimulation()
		{
			lock (syncLock)
			{
				Dispose();
			}
			GC.KeepAlive(syncLock);
			GC.ReRegisterForFinalize(this);
		}

		/// <summary>
		/// Waits until  this <see cref="WaitSimulation"/> instance was signaled once.
		/// </summary>
		/// <exception cref="System.ObjectDisposedException">WaitSimulation</exception>
		[DebuggerStepThrough]
		public void WaitOne()
		{
			lock (syncLock)
			{
				if (disposed)
					throw new ObjectDisposedException("WaitSimulation");

				waiting = true;
				while (!signal)
					Monitor.Wait(syncLock);
				signal = false;
				waiting = false;
			}
		}

		/// <summary>
		/// Waits until  this <see cref="WaitSimulation" /> instance was signaled or time exceeded.
		/// </summary>
		/// <param name="milliseconds">Time to wait in milliseconds or, If set to <c>-1</c>, infinitely.</param>
		/// <returns><c>true</c> if signaled before time is elapsed; otherwise, <c>false</c>.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">milliseconds</exception>
		/// <exception cref="System.ObjectDisposedException">WaitSimulation</exception>
		[DebuggerStepThrough]
		public bool WaitOne(int milliseconds)
		{
			if (milliseconds < 0)
			{
				if (milliseconds == -1)
				{
					WaitOne();
					return true;
				}
				throw new ArgumentOutOfRangeException("milliseconds");
			}

			lock (syncLock)
			{
				if (disposed)
					throw new ObjectDisposedException("WaitSimulation");

				StateObject state = new StateObject();

				using (Timer timer = new Timer(OnTick, state, milliseconds, -1))
				{
					waiting = true;
					while (!signal && !state.TimedOut)
						Monitor.Wait(syncLock);
				}

				signal = false;
				waiting = false;

				return !state.TimedOut;
			}
		}

		private void OnTick(object state)
		{
			StateObject tmp = state as StateObject;
			lock (syncLock)
			{
				tmp.TimedOut = true;
				Monitor.Pulse(syncLock);
			}
		}

		/// <summary>
		/// Signals this instance.
		/// </summary>
		[DebuggerStepThrough]
		public void Signal()
		{
			lock (syncLock)
			{
				if (disposed)
					return;

				signal = true;
				Monitor.Pulse(syncLock);
			}
		}


		/// <summary>
		/// Frees all allocated memory assigned to this <see cref="WaitSimulation"/> instance.
		/// </summary>
		[DebuggerHidden]
		public void Dispose()
		{
			lock (syncLock)
			{
				disposed = true;
				if (waiting)
				{
					signal = true;
					Monitor.Pulse(syncLock);
				}

				GC.SuppressFinalize(this);
			}
		}
	};
}
