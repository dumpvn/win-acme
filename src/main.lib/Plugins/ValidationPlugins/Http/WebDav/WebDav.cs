﻿using PKISharp.WACS.Client;
using PKISharp.WACS.Services;
using System.Threading.Tasks;

namespace PKISharp.WACS.Plugins.ValidationPlugins.Http
{
    internal class WebDav : HttpValidation<WebDavOptions, WebDav>
    {
        private readonly WebDavClientWrapper _webdavClient;

        public WebDav(
            WebDavOptions options, 
            HttpValidationParameters pars,
            RunLevel runLevel, 
            IProxyService proxy,
            SecretServiceManager secretService) :
            base(options, runLevel, pars) => 
            _webdavClient = new WebDavClientWrapper(
                _options.Credential, 
                pars.LogService, 
                proxy, 
                secretService);

        protected override async Task DeleteFile(string path) => _webdavClient.Delete(path);

        protected override async Task DeleteFolder(string path) => _webdavClient.Delete(path);

        protected override async Task<bool> IsEmpty(string path) => !_webdavClient.IsEmpty(path);

        protected override char PathSeparator => '/';

        protected override async Task WriteFile(string path, string content) => _webdavClient.Upload(path, content);
        public override async Task CleanUp()
        {
            await base.CleanUp();
            _webdavClient.Dispose();
        }
    }
}
