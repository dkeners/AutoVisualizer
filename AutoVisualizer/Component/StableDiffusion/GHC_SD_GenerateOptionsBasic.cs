using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AutoVisualizer.Component.StableDiffusion
{
    public class GHC_SD_GenerateOptionsBasic : Templates.GH_Component_SD_Options
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public GHC_SD_GenerateOptionsBasic()
          : base(" SDGenerateSettingsBasic", " SD GenSetB",
              "Configure important settings for Stable Diffusion image generation",
              "AutoVisualizer", "Stable Diffusion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            Params.Input[
                pManager.AddTextParameter("Prompt", "P", "Prompt for SD generation", GH_ParamAccess.item)
                ].Optional = true;
            Params.Input[pManager.AddTextParameter("Negative Prompt", "NP", "Negative Prompt for SD generation", GH_ParamAccess.item)].Optional = true;
            Params.Input[
                pManager.AddTextParameter("Styles", "St", "styles of the prompt", GH_ParamAccess.item)
                ].Optional = true;
            pManager.AddNumberParameter("Seed", "S", "Seed used for generation", GH_ParamAccess.item, -1);
            pManager.AddNumberParameter("Subseed", "Ss", "Subseed used for generation", GH_ParamAccess.item, -1);
            pManager.AddNumberParameter("Subseed Strength", "Ss-s", "Strength of Subseed in generation", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Seed resize from h", "Sr-h", "Get similar results as image with height h", GH_ParamAccess.item, -1);
            pManager.AddNumberParameter("Seed resize from w", "Sr-w", "Get similar results as image with height w", GH_ParamAccess.item, -1);
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
            pManager.AddBooleanParameter("restore_faces", "RF", "Restore faces", GH_ParamAccess.item, true);
            Params.Input[
                pManager.AddTextParameter("tiling", "TL", "Tiling", GH_ParamAccess.item)
                ].Optional = true;
            pManager.AddBooleanParameter("do_not_save_samples", "DNSS", "Do not save samples", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("do_not_save_grid", "DNSG", "Do not save grid", GH_ParamAccess.item, false);
            pManager.AddNumberParameter("denoising_strength", "DS", "Denoising strength", GH_ParamAccess.item, (double)0);
            Params.Input[
                pManager.AddTextParameter("override_settings", "OS", "Override Settings", GH_ParamAccess.item)
                ].Optional = true;
            pManager.AddBooleanParameter("override_settings_restore_afterwards", "OSRA", "Override Settings Restore Afterwards", GH_ParamAccess.item, true);
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
            double subseed = GetInputValue(DA, 4, (double)-1);
            double subseedStrength = GetInputValue(DA, 5, (double)0);
            double seedResizeFromH = GetInputValue(DA, 6, (double)-1);
            double seedResizeFromW = GetInputValue(DA, 7, (double)-1);
            string samplerName = GetInputValue(DA, 8, "Euler a");
            int batchSize = GetInputValue(DA, 9, 1);
            int nIter = GetInputValue(DA, 10, 1);
            int steps = GetInputValue(DA, 11, 50);
            double cfgScale = GetInputValue(DA, 12, (double)7.0);
            int width = GetInputValue(DA, 13, 512);
            int height = GetInputValue(DA, 14, 512);
            bool restoreFaces = GetInputValue(DA, 15, true);
            string tiling = GetInputValue(DA, 16, (string)null);
            bool doNotSaveSamples = GetInputValue(DA, 17, false);
            bool doNotSaveGrid = GetInputValue(DA, 18, false);
            double denoisingStrength = GetInputValue(DA, 19, (double)0);
            string overrideSettings = GetInputValue(DA, 20, (string)null);
            bool overrideSettingsRestoreAfterwards = GetInputValue(DA, 21, true);
            bool disableExtraNetworks = GetInputValue(DA, 22, false);
            bool enableHr = GetInputValue(DA, 23, false);
            double hrScale = GetInputValue(DA, 24, (double)2.0);
            string samplerIndex = GetInputValue(DA, 25, "Euler");
            bool sendImages = GetInputValue(DA, 26, true);
            bool saveImages = GetInputValue(DA, 27, false);


            // Construct the dictionary
            var parameters = new
            {
                prompt,
                negative_prompt = negPrompt,
                styles,
                seed,
                subseed,
                subseed_strength = subseedStrength,
                seed_resize_from_h = seedResizeFromH,
                seed_resize_from_w = seedResizeFromW,
                sampler_name = samplerName,
                batch_size = batchSize,
                n_iter = nIter,
                steps,
                cfg_scale = cfgScale,
                width,
                height,
                restore_faces = restoreFaces,
                tiling,
                do_not_save_samples = doNotSaveSamples,
                do_not_save_grid = doNotSaveGrid,
                denoising_strength = denoisingStrength,
                override_settings = overrideSettings,
                override_settings_restore_afterwards = overrideSettingsRestoreAfterwards,
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
                return Properties.Resources.SDGenerateOptions_Basic;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("868ce0b3-70a6-4528-bba6-9df6f165a274"); }
        }
    }
}