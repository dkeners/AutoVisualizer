using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types.Transforms;
using Rhino.Geometry;
using Rhino.Render.ChangeQueue;
using Rhino.Runtime;

namespace AutoVisualizer.Component.StableDiffusion.ControlNet
{
    public class GHC_CN_ControlNet_to_SDOptions : Templates.GH_Component_SD_Options
    {
        /// <summary>
        /// Initializes a new instance of the ControlNet_to_SDOptions class.
        /// </summary>
        public GHC_CN_ControlNet_to_SDOptions()
          : base("ControlNet_to_SDOptions", "CNtoSD",
              "Convert a ControlNet request into proper Stable Diffusion options",
              "AutoVisualizer", "Stable Diffusion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("ControlNetRequest", "CNR", "ControlNet request", GH_ParamAccess.item, "{}");
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Generation Settings", "GS", "Settings for use in Stable Diffusion image generation.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string alwaysonScripts = GetInputValue(DA, 0, "{}");

            // Construct the dictionary
            var parameters = new
            {
                alwayson_scripts = Newtonsoft.Json.JsonConvert.DeserializeObject(alwaysonScripts)
            };

            string dataOut = Newtonsoft.Json.JsonConvert.SerializeObject(parameters);

            DA.SetData(0, dataOut);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                return Properties.Resources.SDGenerateOptions_ControlNet;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("145FCB4D-32FD-407E-8212-D406774B0BAC"); }
        }
    }
}