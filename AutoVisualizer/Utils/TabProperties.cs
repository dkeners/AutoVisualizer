using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoVisualizer.Utils
{
    internal class TabProperties : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            var server = Grasshopper.Instances.ComponentServer;

            server.AddCategoryIcon("AutoVisualizer", Properties.Resources.SDGenerateOptions);
            server.AddCategorySymbolName("AutoVisualizer", 'A');
            server.AddCategoryShortName("AutoVisualizer", "AV");
            
            return GH_LoadingInstruction.Proceed;
        }
    }
}
