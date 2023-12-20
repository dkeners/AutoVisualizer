using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AutoVisualizer.Component.StableDiffusion
{
    public class GHC_SD_GenerateOptionsEssential : Templates.GH_Component_SD_Options
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public GHC_SD_GenerateOptionsEssential()
          : base("SDGenerateSettingsEssential", "SD GenSetB",
              "Configure important settings for Stable Diffusion image generation",
              "AutoVisualizer", "Stable Diffusion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            Params.Input[pManager.AddTextParameter("Prompt", "P", "Prompt for SD generation", GH_ParamAccess.item)].Optional = true;
            Params.Input[pManager.AddTextParameter("Negative Prompt", "NP", "Negative Prompt for SD generation", GH_ParamAccess.item)].Optional = true;
            Params.Input[
                pManager.AddTextParameter("Styles", "St", "styles of the prompt", GH_ParamAccess.item)
                ].Optional = true;
            pManager.AddNumberParameter("Seed", "S", "Seed used for generation", GH_ParamAccess.item, -1);
            Params.Input[
                pManager.AddTextParameter("Sampler Name", "SN", "Sampler to use for image generation", GH_ParamAccess.item, "Euler a")
                ].Optional = true;
            // Add the remaining parameters
            pManager.AddIntegerParameter("batch_size", "BS", "Batch size", GH_ParamAccess.item, (int)1);
            pManager.AddIntegerParameter("n_iter", "NI", "Number of iterations", GH_ParamAccess.item, (int)1);
            pManager.AddIntegerParameter("steps", "ST", "Number of steps", GH_ParamAccess.item, (int)50);
            pManager.AddNumberParameter("cfg_scale", "CS", "CFG scale", GH_ParamAccess.item, 7.0);
            pManager.AddIntegerParameter("width", "W", "Image width", GH_ParamAccess.item, (int)512);
            pManager.AddIntegerParameter("height", "H", "Image height", GH_ParamAccess.item, (int)512);
            Params.Input[
                pManager.AddTextParameter("tiling", "TL", "Tiling", GH_ParamAccess.item)
                ].Optional = true;
            pManager.AddBooleanParameter("do_not_save_samples", "DNSS", "Do not save samples", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("do_not_save_grid", "DNSG", "Do not save grid", GH_ParamAccess.item, false);
            pManager.AddNumberParameter("denoising_strength", "DS", "Denoising strength", GH_ParamAccess.item, (double)0);

            pManager.AddBooleanParameter("disable_extra_networks", "DEN", "Disable Extra Networks", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("enable_hr", "EHR", "Enable HR", GH_ParamAccess.item, false);
            pManager.AddNumberParameter("hr_scale", "HRS", "HR Scale", GH_ParamAccess.item, 2.0);
            Params.Input[
                pManager.AddTextParameter("sampler_index", "SI", "Sampler Index", GH_ParamAccess.item, "Euler")
                ].Optional = true;
            pManager.AddBooleanParameter("send_images", "SI", "Send Images", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("save_images", "SvI", "Save Images", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Generation Settings", "GS", "Settings for use in Stable Diffusion image generation.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            // Retrieve input values using the helper method
            string prompt = GetInputValue(DA, 0, (string)null);
            string negPrompt = GetInputValue(DA, 1, (string)null);
            string styles = GetInputValue(DA, 2, (string)null);
            double seed = GetInputValue(DA, 3, (double)-1);
            string samplerName = GetInputValue(DA, 4, "Euler a");
            int batchSize = GetInputValue(DA, 5, 1);
            int nIter = GetInputValue(DA, 6, 1);
            int steps = GetInputValue(DA, 7, 50);
            double cfgScale = GetInputValue(DA, 8, (double)7.0);
            int width = GetInputValue(DA, 9, 512);
            int height = GetInputValue(DA, 10, 512);
            string tiling = GetInputValue(DA, 11, (string)null);
            bool doNotSaveSamples = GetInputValue(DA, 12, false);
            bool doNotSaveGrid = GetInputValue(DA, 13, false);
            double denoisingStrength = GetInputValue(DA, 14, (double)0);
            bool disableExtraNetworks = GetInputValue(DA, 15, false);
            bool enableHr = GetInputValue(DA, 16, false);
            double hrScale = GetInputValue(DA, 17, (double)2.0);
            string samplerIndex = GetInputValue(DA, 18, "Euler");
            bool sendImages = GetInputValue(DA, 19, true);
            bool saveImages = GetInputValue(DA, 20, false);


            // Construct the dictionary
            var parameters = new
            {
                prompt,
                negative_prompt = negPrompt,
                styles,
                seed,
                sampler_name = samplerName,
                batch_size = batchSize,
                n_iter = nIter,
                steps,
                cfg_scale = cfgScale,
                width,
                height,
                tiling,
                do_not_save_samples = doNotSaveSamples,
                do_not_save_grid = doNotSaveGrid,
                denoising_strength = denoisingStrength,
                disable_extra_networks = disableExtraNetworks,
                enable_hr = enableHr,
                hr_scale = hrScale,
                sampler_index = samplerIndex,
                send_images = sendImages,
                save_images = saveImages
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
                // return Resources.IconForThisComponent;
                return Properties.Resources.SDGenerateOptions_Essential;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f1da5575-85d4-4e3f-9647-7f88fbca1f16"); }
        }
    }
}