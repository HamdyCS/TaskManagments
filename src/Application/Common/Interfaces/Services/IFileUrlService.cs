using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Services
{
    public interface IFileUrlService
    {
        public string GetUrl(string path);
    }
}
