// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcessUtil.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public Status Status { get; set; }

        public virtual List<Task> Tasks { get; set; }

        public virtual void Start()
        {
            this.Status = Status.ACTIVE;
        }

        public virtual void Finish()
        {


            if (Status == Status.NEW || Status == Status.REMOVED)
            {
                throw new InvalidOperationException("Can't change status from Closed to `New` or `Removed`");
            }

            else if (Status == Status.ACTIVE)
            {
                foreach (var task in Tasks)
                {
                    if (task.Status != Status.CLOSED && task.Status != Status.REMOVED)
                    {
                        throw new InvalidOperationException("Can't close a porject with ");
                    }

                }

                Status = Status.CLOSED;

            }

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
