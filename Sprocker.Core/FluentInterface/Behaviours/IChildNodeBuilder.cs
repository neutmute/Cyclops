using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sprocker.Core.FluentInterface.Behaviours
{
    public interface IChildNodeBuilder
    {
        IChildNodeBuilder IsTransactional(bool isTransactional);
        //INodeBuilder ChildNode(string element); 
    }
}
