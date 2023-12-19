using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace AutoVisualizer
{
    public class AutoVisualizerInfo : GH_AssemblyInfo
    {
        public override string Name => "AutoVisualizer";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("f07a37c7-4e8a-4321-a5c6-e137fe99c469");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}