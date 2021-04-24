using System;
using System.Collections.Generic;
using System.Text;

namespace SafeEntranceApp.Services
{
    public interface IBackgroundService
    {
        void Start(DateTime? dateTime);
        void Stop();
    }
}
