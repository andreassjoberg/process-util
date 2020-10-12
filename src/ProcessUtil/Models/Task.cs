// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
            // TODO: Implement
        }

        public virtual void Finish()
        {
            // TODO: Implement
        }

        public virtual void Remove()
        {
            // TODO: Implement
        }
    }
}
