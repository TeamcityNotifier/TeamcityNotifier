namespace TeamcityNotifier.RestObject
{
    internal class EnumParser
    {
        public static Status GetStatusFor(string value)
        {
            var enumString = value.ToLower();

            if (enumString == "failure")
            {
                return Status.Failure;
            }

            if (enumString == "success")
            {
                return Status.Success;
            }

            return Status.Error;
        }
    }
}