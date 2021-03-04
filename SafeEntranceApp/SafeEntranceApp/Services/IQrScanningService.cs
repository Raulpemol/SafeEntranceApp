using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SafeEntranceApp.Services
{
    interface IQrScanningService
    {
        Task<string> ScanAsync();
    }
}
