﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bloom.Project
{
	public class ProjectModel
	{
		private readonly string _pathToProjectDirectory;

		public ProjectModel(string pathToProjectDirectory)
		{
			_pathToProjectDirectory = pathToProjectDirectory;
		}
	}
}
