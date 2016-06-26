﻿using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

using JetBrains.Annotations;

namespace CodeJam
{
	/// <summary>
	/// Wraps Mashal.AllocHGlobal and Marshal.FreeHGlobal.
	/// </summary>
	[PublicAPI]
	[SecurityCritical]
	public class HGlobal : CriticalFinalizerObject, IDisposable
	{
		private IntPtr _buffer;

		/// <summary>
		/// Allocates memory from the unmanaged memory of the process by using the specified number of bytes.
		/// </summary>
		/// <param name="cb">The required number of bytes in memory.</param>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public HGlobal(int cb)
		{
			_buffer = Marshal.AllocHGlobal(cb);
			Length = cb;
		}

		/// <summary>
		/// Takes ownership over given pointer.
		/// </summary>
		/// <param name="ptr">Point to memory allocated by <see cref="Marshal.AllocHGlobal(System.IntPtr)"/></param>
		public HGlobal(IntPtr ptr)
		{
			_buffer = ptr;
			Length = 0;
		}

		/// <summary>
		/// Finalizer.
		/// </summary>
		~HGlobal()
		{
			DisposeInternal();
		}

		/// <summary>
		/// Dispose method to free all resources.
		/// </summary>
		public void Dispose()
		{
			DisposeInternal();
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Length
		/// </summary>
		public int Length { get; }

		/// <summary>
		/// Pointer to data.
		/// </summary>
		public IntPtr Data => _buffer;

		/// <summary>
		/// Internal Dispose method.
		/// </summary>
		private void DisposeInternal()
		{
			if (_buffer != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(_buffer);
				_buffer = IntPtr.Zero;
			}
		}
	}
}
