﻿namespace LICC.Core.Common
{
    public abstract class BaseWarning : BaseIssue
    {
        public override string ToString() => $"WRN {base.ToString()}";
    }
}
