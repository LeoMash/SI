﻿#if LEGACY
using Microsoft.AspNet.SignalR;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SImulator.Implementation.ButtonManagers.Web
{
#if LEGACY
    public sealed class ButtonHub : Hub
    {
        public string Press()
        {
            return WebManager2.Current.Press(this.Context.ConnectionId);
        }

        public string Test()
        {
            return "A";
        }
    }
#endif
}
