using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MGUI.Core.Trait;

public class ParentTrait
{
    private readonly IControl parent;
    public PaddingTrait Padding { get; set; }

    public ParentTrait(IControl parent)
    {
        this.parent = parent;
    }
}
