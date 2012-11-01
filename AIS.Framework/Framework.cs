// Time-stamp: <2009-11-12 13:03:54 GMT+3:30>
//
// SIMULAIS, versatile Artificial Immune System discrete event SIMULator
//
// Copyright (c) 2009 Mahdi Zamani, Mahnush Movahedi
// All rights reserved.
//
// Permission to use, copy, modify, and distribute this software and its
// documentation in source and binary forms for non-commercial purposes
// and without fee is hereby granted, provided that the above copyright
// notice appear in all copies and that both the copyright notice and
// this permission notice appear in supporting documentation. and that
// any documentation, advertising materials, and other materials related
// to such distribution and use acknowledge that the software was
// developed by the developers. The name of the developers may not be used to
// endorse or promote products derived from this software without
// specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED "AS IS" AND WITHOUT ANY EXPRESS OR IMPLIED
// WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED WARRANTIES OF
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
//
using System;

namespace AIS.Framework
{
	#region Enumerations

	public enum CytokineType { IL1, IL2, IL4 }
	public enum CellType { Helper, APC }

	// Important Note: tissue type names must be exactly equal to their corresponding class name
	// this is because in parent agent addressing the reflected class type name is sent
	public enum TissueType { Local, LymphNode, Thymus, BoneMarrow }

	public enum CellMaturationLevel { Immature, Semimature, Mature }
	public enum CellState { Alive, Apoptosis, Necrosis, Dead }
	public enum TissueState { Alive, Dead }
	public enum LogLevel { Minor = 0, Normal = 1, Major = 2 }

	#endregion

	#region Interfaces

	public interface IAgent
	{
		Address Address { get; }
		void HandleSignal(Signal signal);
		event EventHandler<LogEventArgs> LogEvent;
	}

	public interface ITissue : IAgent
	{
		TissueType Type { get; }
	}

	public interface ICell : IAgent
	{
	}

	/*public interface IParentAgent : IAgent
	{
		void AddChild(ChildAgent agent);
		bool RemoveChild(ChildAgent agent);
	}*/

	#endregion
}