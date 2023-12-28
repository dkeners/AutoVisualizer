using System;
using System.Collections.Generic;
using Eto.Forms;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AutoVisualizer.Component
{
    public class CaptureOptions : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CaptureOptions class.
        /// </summary>
        public CaptureOptions()
          : base("CaptureOptions", "Capt Opt",
              "Change options for the capture components",
              "AutoVisualizer", "Capture")
        {
        }

        /// <summary>
        /// Set the exposure to secondary.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Grid", "G", "Show grid", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("World Axes", "WA", "Show world axes", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("CPlaneAxes", "CPA", "Show cplane axes", GH_ParamAccess.item, true);

            // set all optional
            for (var i =0; i < pManager.ParamCount; i++)
            {
                pManager[i].Optional = true;
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Options", "O", "Options for the capture.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool showGrid = true;
            bool showWorldAxes = true;
            bool showCPlaneAxes = true;

            DA.GetData(0, ref showGrid);
            DA.GetData(1, ref showWorldAxes);
            DA.GetData(2, ref showCPlaneAxes);

            dynamic options = new
            {
                grid = showGrid,
                worldAxes = showWorldAxes,
                cplaneAxes = showCPlaneAxes
            };

            // Send out a JSON string of the options
            DA.SetData(0, Newtonsoft.Json.JsonConvert.SerializeObject(options));
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                return Properties.Resources.CaptureOptions;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("A823FE30-8A34-402C-BAC2-E9256082E46E"); }
        }
    }
}