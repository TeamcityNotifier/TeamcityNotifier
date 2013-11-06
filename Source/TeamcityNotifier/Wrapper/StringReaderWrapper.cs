namespace TeamcityNotifier.Wrapper
{
    using System;
    using System.IO;

    internal class StringReaderWrapper : IStringReader
    {
        private StringReader stringReader;
        private volatile bool disposed;

        public StringReaderWrapper(StringReader stringReader)
        {
            this.stringReader = stringReader;
        }
        
        public StringReader ToStringReader()
        {
            return stringReader;
        }


        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.stringReader != null)
                {
                    this.stringReader.Dispose();
                }
            }

            this.stringReader = null;
            this.disposed = true;
        }
    }
}