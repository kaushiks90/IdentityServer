using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Installer
{
    public interface IInstaller
    {
        void IInstallServices(IConfiguration configuration, IServiceCollection services);
    }
}
