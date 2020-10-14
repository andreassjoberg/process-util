// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace ProcessUtil.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public Status Status { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public virtual void Start()
        {
            if (Project.Status != Status.ACTIVE)
            {
                throw new InvalidOperationException("Can't start tast without an active project");
            }
            else
                this.Status = Status.ACTIVE;
        }

        public virtual void Finish()
        {
            if (Status == Status.NEW || Status == Status.REMOVED)
            {
                throw new InvalidOperationException("Can't change status from Closed to `New` or `Removed`");
            }
            else
                Status = Status.CLOSED;
        }

        public virtual void Remove()
        {
            if (Status == Status.NEW || Status == Status.ACTIVE)
            {
                throw new InvalidOperationException("Can't change status from Removed to `New` or `Active`");
            }
            else
                Status = Status.REMOVED;
        }
    }
}
