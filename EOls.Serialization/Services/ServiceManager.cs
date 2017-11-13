using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOls.Serialization.Services
{
    public sealed class ServiceManager
    {
        public static ServiceManager Instance { get; } = new ServiceManager();

        private ServiceManager()
        {
        }
    }
}