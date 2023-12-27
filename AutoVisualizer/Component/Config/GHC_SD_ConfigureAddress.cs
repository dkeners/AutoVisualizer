using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AutoVisualizer.Component.Config
{
    public class GHC_SD_ConfigureAddress : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ConfigureAddress class.
        /// </summary>
        public GHC_SD_ConfigureAddress()
          : base("ConfigureAddress", "ConAdd",
              "Configure the address to the API",
              "AutoVisualizer", "Settings")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("IP Address", "IP", "IP Address", GH_ParamAccess.item, "127.0.0.1");
            pManager.AddTextParameter("Port", "P", "Port number", GH_ParamAccess.item, "7860");
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Address", "A", "Address to the API", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.ConfigureAddress;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("850D1040-40B1-4C16-B9C9-80ACBE84E10A"); }
        }
    }
}