using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MGUI.Core.Trait;

public class PaddingTrait
{
    private int[] paddingExplicit = { 5, 5, 5, 5 };
    /// <summary>
    /// Padding from left, top, right, bottom
    /// </summary>
    public int[] Sides
    {
        get => paddingExplicit;
        set
        {
            paddingExplicit = value;
        }
    }

    /// <summary>
    /// Set all sides of padding to one value.
    /// Returns null if not all side of padding are the same.
    /// </summary>
    public int? Padding
    {
        get => FourSidesPaddingAreAllTheSame() ? paddingExplicit[0] : null;
        set
        {
            var pad = value ?? 0;
            paddingExplicit = new int[] { pad, pad, pad, pad };
        }
    }

    private bool FourSidesPaddingAreAllTheSame()
    {
        return paddingExplicit.Distinct().Take(2).Count() == 1;
    }

}
