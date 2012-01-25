﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sprocker.Core.FluentInterface.Behaviours
{
    public interface IChildNodeBuilder<T>
    {
        IChildNodeBuilder<T> IsTransactional(bool isTransactional);
        INodeBuilder<T> MapChildNode();
    }
}