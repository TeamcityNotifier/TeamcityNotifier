﻿namespace TeamcityNotifier.Wrapper
{
    using System;

    internal class UriWrapper : IUri
    {
        private readonly Uri uri;

        public UriWrapper(Uri uri)
        {
            this.uri = uri;
        }

        public Uri ToUri()
        {
            return uri;
        }
    }
}