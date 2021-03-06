﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Ninject;

namespace Odin.ToSeWebJob
{
    public class MyJobActivator : IJobActivator
    {
        protected readonly IKernel _kernel;

        public MyJobActivator(IKernel kernel)
        {
            _kernel = kernel;
        }

        public T CreateInstance<T>()
        {
            return _kernel.Get<T>();
        }
    }
}
