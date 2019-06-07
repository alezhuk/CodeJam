﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

using static CodeJam.Targeting.MethodImplOptionsExt;

namespace CodeJam.IO
{
	/// <summary>IO assertions class.</summary>
	[PublicAPI]
	public static class IoCode
	{
		/// <summary>Asserts that specified path is either absolute or relative not rooted path.</summary>
		/// <param name="path">The path.</param>
		/// <param name="argName">Name of the argument.</param>
		[DebuggerHidden, MethodImpl(AggressiveInlining)]
		[AssertionMethod]
		public static void IsWellFormedPath(
			[NotNull] string path,
			[NotNull, InvokerParameterName] string argName)
		{
			Code.NotNullNorEmpty(path, argName);
			if (!PathHelpers.IsWellFormedPath(path))
				throw IoCodeExceptions.ArgumentNotWellFormedPath(argName, path);
		}

		/// <summary>Asserts that specified path is well-formed full path.</summary>
		/// <param name="path">The path.</param>
		/// <param name="argName">Name of the argument.</param>
		[DebuggerHidden, MethodImpl(AggressiveInlining)]
		[AssertionMethod]
		public static void IsWellFormedAbsolutePath(
			[NotNull] string path,
			[NotNull, InvokerParameterName] string argName)
		{
			Code.NotNullNorEmpty(path, argName);
			if (!PathHelpers.IsWellFormedAbsolutePath(path))
				throw IoCodeExceptions.ArgumentNotWellFormedAbsolutePath(argName, path);
		}

		/// <summary>Asserts that specified path is well-formed full path.</summary>
		/// <param name="path">The path.</param>
		/// <param name="argName">Name of the argument.</param>
		[DebuggerHidden, MethodImpl(AggressiveInlining)]
		[AssertionMethod]
		public static void IsWellFormedRelativePath(
			[NotNull] string path,
			[NotNull, InvokerParameterName] string argName)
		{
			Code.NotNullNorEmpty(path, argName);
			if (!PathHelpers.IsWellFormedRelativePath(path))
				throw IoCodeExceptions.ArgumentRootedOrNotRelativePath(argName, path);
		}

		/// <summary>Asserts that specified path is well formed and ends with directory or volume separator chars.</summary>
		/// <param name="path">The path.</param>
		/// <param name="argName">Name of the argument.</param>
		[DebuggerHidden, MethodImpl(AggressiveInlining)]
		[AssertionMethod]
		public static void IsWellFormedContainerPath(
			[NotNull] string path,
			[NotNull, InvokerParameterName] string argName)
		{
			Code.NotNullNorEmpty(path, argName);
			if (!PathHelpers.IsWellFormedContainerPath(path))
				throw IoCodeExceptions.ArgumentNotVolumeOrDirectoryPath(argName, path);
		}

		/// <summary>Asserts that specified path is well-formed simple name.</summary>
		/// <param name="path">The path.</param>
		/// <param name="argName">Name of the argument.</param>
		[DebuggerHidden, MethodImpl(AggressiveInlining)]
		[AssertionMethod]
		public static void IsWellFormedSimpleName(
			[NotNull] string path,
			[NotNull, InvokerParameterName] string argName)
		{
			Code.NotNullNorEmpty(path, argName);
			if (!PathHelpers.IsWellFormedSimpleName(path))
				throw IoCodeExceptions.ArgumentNotSimpleName(argName, path);
		}

		#region IO path
		/// <summary>Asserts that specified file does exist.</summary>
		/// <param name="filePath">Path to the file.</param>
		/// <param name="argName">Name of the argument.</param>
		[DebuggerHidden, MethodImpl(AggressiveInlining)]
		[AssertionMethod]
		public static void FileExists(
			[NotNull] string filePath,
			[NotNull, InvokerParameterName] string argName)
		{
			Code.NotNull(filePath, argName);
			if (!File.Exists(filePath))
				throw Directory.Exists(filePath)
					? IoCodeExceptions.ArgumentDirectoryExistsFileExpected(argName, filePath)
					: IoCodeExceptions.ArgumentFileNotFound(argName, filePath);
		}

		/// <summary>Asserts that specified directory does exist.</summary>
		/// <param name="directoryPath">Path to the directory.</param>
		/// <param name="argName">Name of the argument.</param>
		[DebuggerHidden, MethodImpl(AggressiveInlining)]
		[AssertionMethod]
		public static void DirectoryExists(
			[NotNull] string directoryPath,
			[NotNull, InvokerParameterName] string argName)
		{
			if (!Directory.Exists(directoryPath))
				throw File.Exists(directoryPath)
					? IoCodeExceptions.ArgumentFileExistsDirectoryExpected(argName, directoryPath)
					: IoCodeExceptions.ArgumentDirectoryNotFound(argName, directoryPath);
		}

		/// <summary>Asserts that specified path is not a path to existent file or a directory.</summary>
		/// <param name="path">The path.</param>
		[DebuggerHidden, MethodImpl(AggressiveInlining)]
		[AssertionMethod]
		public static void PathIsFree([NotNull] string path)
		{
			if (Directory.Exists(path))
				throw IoCodeExceptions.DirectoryExists(path);
			if (File.Exists(path))
				throw IoCodeExceptions.FileExists(path);
		}
		#endregion
	}
}