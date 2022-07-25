﻿using Microsoft.Owin.Hosting;
using Owin;
using SICore;
using SIPackages.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Web.Http;

namespace SIGame.ViewModel.Web
{
    public sealed class WebManager : ShareBase
    {
        private const int AddressIsUsedErrorCode = 10048;

        internal static WebManager Current;

        private class Startup
        {
            public void Configuration(IAppBuilder appBuilder)
            {
                HttpConfiguration config = new HttpConfiguration();
                config.Routes.MapHttpRoute(
                    name: "Media",
                    routeTemplate: "data/{file}",
                    defaults: new { controller = "Resource", action = "Get" }
                );

                appBuilder.UseWebApi(config);
            }
        }

        private bool _disposed = false;

        private readonly int _multimediaPort = -1;
        private IDisposable _web = null;

        public WebManager(int multimediaPort)
        {
            _multimediaPort = multimediaPort;
            Init();
        }

        private bool Init()
        {
            lock (_filesSync)
            {
                if (_disposed)
                {
                    return false;
                }

                if (_web == null)
                {
                    var options = new StartOptions
                    {
                        ServerFactory = "Nowin",
                        Port = _multimediaPort
                    };

                    try
                    {
                        _web = WebApp.Start<Startup>(options);
                    }
                    catch (TargetInvocationException exc)
                        when (exc.InnerException is SocketException socketException
                        && socketException.NativeErrorCode == AddressIsUsedErrorCode)
                    {
                        throw new PortIsUsedException();
                    }

                    Current = this;
                }
            }

            return true;
        }

        internal StreamInfo GetFile(string file)
        {
            Func<StreamInfo> response = null;

            lock (_filesSync)
            {
                if (!_files.TryGetValue(Uri.UnescapeDataString(file), out response) && !_files.TryGetValue(file, out response))
                {
                    return null;
                }
            }

            return response();
        }

        public override string MakeUri(string file, string category)
        {
            var uri = Uri.EscapeDataString(file);
            return $"http://localhost:{_multimediaPort}/data/{uri}";
        }

        public override void StopUri(IEnumerable<string> toRemove)
        {
            lock (_filesSync)
            {
                if (_disposed)
                    return;

                base.StopUri(toRemove);
            }
        }

        public override void Dispose()
        {
            lock (_filesSync)
            {
                StopUri(_files.Keys.ToArray());

                if (_web != null)
                {
                    _web.Dispose();
                    _web = null;
                }

                _disposed = true;

                Current = null;
            }
        }
    }
}
