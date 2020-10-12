// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using ProcessUtil.Models;
using Xunit;

namespace ProcessUtil.Test
{
    public class ProjectTests : IClassFixture<ProjectTests.Fixture>
    {
        private readonly Fixture _fixture;

        public ProjectTests(Fixture fixture)
        {
            _fixture = fixture;
        }

        #region Start project

        [Fact]
        public void Start_New_Project_Sets_Status_To_Active()
        {
            var project = _fixture.CreateNewProject();
            project.Start();
            Assert.Equal(Status.ACTIVE, project.Status);
        }

        [Fact]
        public void Start_Active_Project_Doesnt_Change_Status()
        {
            var project = _fixture.CreateActiveProject();
            project.Start();
            Assert.Equal(Status.ACTIVE, project.Status);
        }

        [Fact]
        public void Start_Closed_Project_Sets_Status_To_Active()
        {
            var project = _fixture.CreateClosedProject();
            project.Start();
            Assert.Equal(Status.ACTIVE, project.Status);
        }

        [Fact]
        public void Start_Removed_Project_Sets_Status_To_Active()
        {
            var project = _fixture.CreateRemovedProject();
            project.Start();
            Assert.Equal(Status.ACTIVE, project.Status);
        }

        #endregion

        #region Finish project

        [Fact]
        public void Finish_Active_Project_Sets_Status_To_Closed()
        {
            var project = _fixture.CreateActiveProject();
            project.Tasks.ForEach(task => task.Status = Status.CLOSED);
            project.Finish();
            Assert.Equal(Status.CLOSED, project.Status);
        }

        [Fact]
        public void Finish_New_Project_Throws_Exception()
        {
            var project = _fixture.CreateNewProject();
            Assert.Throws<InvalidOperationException>(() => project.Finish());
        }

        [Fact]
        public void Finish_Removed_Project_Throws_Exception()
        {
            var project = _fixture.CreateRemovedProject();
            Assert.Throws<InvalidOperationException>(() => project.Finish());
        }

        [Fact]
        public void Finish_Closed_Project_Doesnt_Change_Status()
        {
            var project = _fixture.CreateClosedProject();
            project.Finish();
            Assert.Equal(Status.CLOSED, project.Status);
        }

        #endregion

        #region Remove project

        [Fact]
        public void Remove_Closed_Project_Sets_Status_To_Removed()
        {
            var project = _fixture.CreateClosedProject();
            project.Remove();
            Assert.Equal(Status.REMOVED, project.Status);
        }

        [Fact]
        public void Remove_Removed_Project_Doesnt_Change_Status()
        {
            var project = _fixture.CreateRemovedProject();
            project.Remove();
            Assert.Equal(Status.REMOVED, project.Status);
        }

        [Fact]
        public void Remove_New_Project_Throws_Exception()
        {
            var project = _fixture.CreateNewProject();
            Assert.Throws<InvalidOperationException>(() => project.Remove());
        }

        [Fact]
        public void Remove_Active_Project_Throws_Exception()
        {
            var project = _fixture.CreateActiveProject();
            Assert.Throws<InvalidOperationException>(() => project.Remove());
        }

        #endregion

        #region Project task relation

        [Fact]
        public void Start_Task_In_New_Project_Throws_Exception()
        {
            var project = _fixture.CreateNewProject();
            Assert.Throws<InvalidOperationException>(() => project.Tasks.Single(p => p.Id == 5).Start());
        }

        [Fact]
        public void Can_Start_Task_In_Active_Project()
        {
            var project = _fixture.CreateActiveProject();
            var task = project.Tasks.First(p => p.Status == Status.NEW);
            task.Start();
            Assert.Equal(Status.ACTIVE, task.Status);
        }

        [Fact]
        public void Finish_Project_With_Active_Task_Throws_Exception()
        {
            var project = _fixture.CreateActiveProject();
            foreach (var task in project.Tasks.Skip(1))
            {
                task.Status = Status.CLOSED;
            }

            Assert.Throws<InvalidOperationException>(() => project.Finish());
        }

        #endregion

        public class Fixture
        {
            public Project CreateNewProject()
            {
                var project = new Project { Id = 1, Name = "Rick", Status = Status.NEW };
                project.Tasks = new List<Task>
                {
                    new Task { Id = 1, Name = "Never", Status = Status.NEW, Project = project, ProjectId = project.Id },
                    new Task { Id = 2, Name = "Gonna", Status = Status.NEW, Project = project, ProjectId = project.Id },
                    new Task { Id = 3, Name = "Give", Status = Status.NEW, Project = project, ProjectId = project.Id },
                    new Task { Id = 4, Name = "You", Status = Status.NEW, Project = project, ProjectId = project.Id },
                    new Task { Id = 5, Name = "Up", Status = Status.NEW, Project = project, ProjectId = project.Id }
                };
                return project;
            }

            public Project CreateActiveProject()
            {
                var project = new Project { Id = 1, Name = "Rick", Status = Status.ACTIVE };
                project.Tasks = new List<Task>
                {
                    new Task { Id = 1, Name = "Never", Status = Status.NEW, Project = project, ProjectId = project.Id },
                    new Task { Id = 2, Name = "Gonna", Status = Status.NEW, Project = project, ProjectId = project.Id },
                    new Task { Id = 3, Name = "Give", Status = Status.CLOSED, Project = project, ProjectId = project.Id },
                    new Task { Id = 4, Name = "You", Status = Status.ACTIVE, Project = project, ProjectId = project.Id },
                    new Task { Id = 5, Name = "Up", Status = Status.ACTIVE, Project = project, ProjectId = project.Id }
                };
                return project;
            }

            public Project CreateClosedProject()
            {
                var project = new Project { Id = 1, Name = "Rick", Status = Status.CLOSED };
                project.Tasks = new List<Task>
                {
                    new Task { Id = 1, Name = "Never", Status = Status.CLOSED, Project = project, ProjectId = project.Id },
                    new Task { Id = 2, Name = "Gonna", Status = Status.CLOSED, Project = project, ProjectId = project.Id },
                    new Task { Id = 3, Name = "Give", Status = Status.REMOVED, Project = project, ProjectId = project.Id },
                    new Task { Id = 4, Name = "You", Status = Status.CLOSED, Project = project, ProjectId = project.Id },
                    new Task { Id = 5, Name = "Up", Status = Status.REMOVED, Project = project, ProjectId = project.Id }
                };
                return project;
            }

            public Project CreateRemovedProject()
            {
                var project = new Project { Id = 1, Name = "Rick", Status = Status.REMOVED };
                project.Tasks = new List<Task>
                {
                    new Task { Id = 1, Name = "Never", Status = Status.CLOSED, Project = project, ProjectId = project.Id },
                    new Task { Id = 2, Name = "Gonna", Status = Status.CLOSED, Project = project, ProjectId = project.Id },
                    new Task { Id = 3, Name = "Give", Status = Status.REMOVED, Project = project, ProjectId = project.Id },
                    new Task { Id = 4, Name = "You", Status = Status.CLOSED, Project = project, ProjectId = project.Id },
                    new Task { Id = 5, Name = "Up", Status = Status.REMOVED, Project = project, ProjectId = project.Id }
                };
                return project;
            }
        }
    }
}
