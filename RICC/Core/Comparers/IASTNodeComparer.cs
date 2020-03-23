using System;
using System.Collections.Generic;
using System.Text;

namespace RICC.Core.Comparers
{
    public interface IASTNodeComparer<T>
    {
        ComparerResult Result { get; }

        ComparerResult Compare(T n1, T n2);
    }
}
